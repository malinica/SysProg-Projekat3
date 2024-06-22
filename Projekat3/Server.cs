using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Net;
using Projekat2;

namespace Projekat3
{
    public class Server : IObservable<IssueComment>
    {
        private readonly Thread _listenerThread;
        private bool _running;
        private ReaderWriterLockSlim _lock;
        private ReplaySubject<IssueComment> issueStream;
        private List<IDisposable> subscriptions;
        private HttpListener listener;
        private GitHubAPI api;
        private Cache cache;


        public Server()
        {
            _lock = new ReaderWriterLockSlim();
            _listenerThread = new Thread(Listen);
            subscriptions = new List<IDisposable>();
            cache = new Cache();
            _running = false;
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            issueStream = new ReplaySubject<IssueComment>(TimeSpan.FromSeconds(30));
            api = new GitHubAPI();
        }

        public async Task Answer(HttpStatusCode code, string data, HttpListenerContext context)
        {
            try
            {
                var response = context.Response;
                byte[] buffer;
                response.StatusCode = (int)code;
                buffer = Encoding.UTF8.GetBytes(data);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Start()
        {
            try
            {
                Console.WriteLine("Server started");
                _running = true;
                listener.Start();
                _listenerThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void Listen()
        {
            try
            {
                while (_running)
                {
                    var context = await listener.GetContextAsync();
                    if (_running)
                    {
                        await HandleRequest(context);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            _lock.EnterWriteLock();
            try
            {
                issueStream.OnCompleted();
                issueStream.Dispose();
                foreach (var s in subscriptions)
                {
                    s.Dispose();
                }
                listener.Stop();
                _running = false;
                _listenerThread.Interrupt();
                _listenerThread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public IDisposable Subscribe(IObserver<IssueComment> observer)
        {
            var subscription = issueStream
                .SubscribeOn(NewThreadScheduler.Default)
                .ObserveOn(ThreadPoolScheduler.Instance)
                .Subscribe(observer);

            _lock.EnterWriteLock();
            try
            {
                subscriptions.Add(subscription);
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            return subscription;
        }

        public void Unsubscribe(IDisposable sub)
        {
            _lock.EnterWriteLock();
            try
            {
                var subscriptionToRemove = subscriptions.Find(s => s.Equals(sub));
                if (subscriptionToRemove != null)
                {
                    subscriptions.Remove(subscriptionToRemove);
                    subscriptionToRemove.Dispose();
                    Console.WriteLine("Subscription removed \n");
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                if (request.HttpMethod != "GET" || request.RawUrl.Contains("favicon.ico"))
                {
                    await Answer(HttpStatusCode.BadRequest, "Bad request", context);
                    return;
                }

                string[] parts = request.RawUrl.Split('/');
                if (parts.Length != 3)
                {
                    await Answer(HttpStatusCode.BadRequest, "Bad parameters of the request", context);
                    return;
                }
                var owner = parts[1];
                var type = parts[2];
                if (cache.ImaKljuc(owner + "/" + type))
                {
                    var rep = cache.CitajIzKesa(owner + "/" + type);
                    await Answer(HttpStatusCode.OK, rep.ToString(), context);

                    return;
                }
                else
                {
                    var list = await api.Search(owner, type);
                    Notify(issueStream, list);
                    var Rep = new Repository(owner, type);
                    Rep.AddIssues(list);
                    await Answer(HttpStatusCode.OK, Rep.ToString(), context);
                    cache.DodajUKes(owner + "/" + type, Rep);

                }
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Notify(ReplaySubject<IssueComment> stream, List<Issue> list)
        {
            try
            {
                foreach (var item in list)
                    foreach (var comment in item._comments)
                        if (comment != null)
                            stream.OnNext(comment);
            }
            catch (Exception ex)
            {
                stream.OnError(ex);
            }
        }
    }

}
