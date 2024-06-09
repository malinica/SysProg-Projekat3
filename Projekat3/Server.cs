using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using Newtonsoft.Json.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Threading;
using System.Net;
using Octokit;
using System.Reactive;



namespace Projekat3
{
    public class Server : IObservable<IssueComment>
    {
        private Subject<IssueComment> issueStream;
        private HttpListener listener;
        private GitHubAPI api;
        private IDisposable subscription1;
        private IDisposable subscription2;


        public Server()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            issueStream = new Subject<IssueComment>();
            api = new GitHubAPI();
        }
        public void Answer(HttpStatusCode code, string data, HttpListenerContext context)
        {
            var response = context.Response;
            byte[] buffer;
            response.StatusCode = (int)code;
            buffer = Encoding.UTF8.GetBytes(data);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
        public void Start()
        {
            Console.WriteLine("Server started");
            listener.Start();
            while (true)
            {
                var context = listener.GetContext();
                Task.Run(() => HandleRequest(context));
            }
        }
        public void Stop()
        {
            listener.Stop();
            subscription1.Dispose();
            subscription2.Dispose();
        }
        public IDisposable Subscribe(IObserver<IssueComment> observer)
        {
                 return issueStream
                //.SubscribeOn(ThreadPoolScheduler.Instance)
                .ObserveOn(ThreadPoolScheduler.Instance)
                .Subscribe(observer);
        }

        private void HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            try
            {
            if (request.HttpMethod != "GET" || request.RawUrl.Contains("favicon.ico"))
            {
                Answer(HttpStatusCode.BadRequest, "Bad request", context);
                return;
            }
           
            string[] parts = request.RawUrl.Split('/');
            if (parts.Length != 3)
            {
                Answer(HttpStatusCode.BadRequest, null, context);
                return;
            }
            var owner = parts[1];
            var type = parts[2];

            

            api.Search(owner, type, issueStream);
            Answer(HttpStatusCode.OK, "Processing...", context);
            return;
            }
catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
