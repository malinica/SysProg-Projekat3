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
        private ReplaySubject<IssueComment> issueStream;
        private List<IDisposable> subscriptions;
        private HttpListener listener;
        private GitHubAPI api;
        private Cache cache;


        public Server()
        {
            _listenerThread = new Thread(Listen);
            subscriptions = new List<IDisposable>();
            cache = new Cache();
            _running = false;
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            issueStream = new ReplaySubject<IssueComment>(TimeSpan.FromSeconds(30));
            api = new GitHubAPI();
        }

        public void Answer(HttpStatusCode code, string data, HttpListenerContext context)
        {
            try
            {
                var response = context.Response;
                byte[] buffer;
                response.StatusCode = (int)code;
                buffer = Encoding.UTF8.GetBytes(data);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
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

        public void Listen()
        {
            try
            {
                while (_running)
                {
                    var context = listener.GetContext();
                    Task.Run(() => HandleRequest(context));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
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
        }

        public IDisposable Subscribe(IObserver<IssueComment> observer)
        {
            var subscription = issueStream
                .SubscribeOn(NewThreadScheduler.Default)
                .ObserveOn(ThreadPoolScheduler.Instance)
                .Subscribe(observer);
            subscriptions.Add(subscription);
            return subscription;
        }

        private async void HandleRequest(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                if (request.HttpMethod != "GET" || request.RawUrl.Contains("favicon.ico"))
                {
                    Answer(HttpStatusCode.BadRequest, "Bad request", context);
                    return;
                }

                string[] parts = request.RawUrl.Split('/');
                if (parts.Length != 3)
                {
                    Answer(HttpStatusCode.BadRequest, "Bad parameters of the request", context);
                    return;
                }
                var owner = parts[1];
                var type = parts[2];
                if (cache.ImaKljuc(owner + "/" + type))
                {
                    var rep = cache.CitajIzKesa(owner + "/" + type);
                    /*
                    foreach (var i in rep.issueList)
                    {
                        foreach(var c in i._comments)
                        {

                        issueStream.OnNext(c);
                        }
                    }
                    */
                    Answer(HttpStatusCode.OK, rep.ToString(), context);

                    return;
                }
                else
                {
                    var issue = await api.Search(owner, type, issueStream);
                    Answer(HttpStatusCode.OK, issue.ToString(), context);
                    cache.DodajUKes(owner + "/" + type, issue);

                }
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
