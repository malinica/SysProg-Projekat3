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



namespace Projekat3
{
    public class Server : IObservable<IssueComment>
    {
        const string API_KEY = "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";
        HttpClient client;
        public Subject<IssueComment> issueStream;

        const string BASE_URL = "https://api.github.com/repos";


        public Server()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
            issueStream = new Subject<IssueComment>();
        }
        public void Serach(string owner, string type)
        {
            client.GetAsync($"{BASE_URL}/{owner}/{type}/issues").ContinueWith(async (task) =>
            {
                try
                {
                    var response = task.Result;
                    if (!(response.IsSuccessStatusCode))
                        throw new Exception($"From thread:  {Thread.CurrentThread.ManagedThreadId}, Querry failed. Reason: {response.ReasonPhrase}");
                    var responseSTRING = await response.Content.ReadAsStringAsync();
                    var responseJSON = JArray.Parse(responseSTRING);
                    foreach (JObject item in responseJSON)
                    {
                        var idIssue = (string)item["number"];
                        var commentsResponse = await client.GetAsync($"{BASE_URL}/{owner}/{type}/issues/{idIssue}/comments");
                        var commentsResponseString = await commentsResponse.Content.ReadAsStringAsync();

                        var responseJSON2 = JArray.Parse(commentsResponseString);
                        foreach (var item2 in responseJSON2)
                        {
                            var issueObj = new IssueComment((string)item2["id"], new Issue(idIssue, (string)item["user"]["login"], (string)item["title"]), (string)item2["body"]);

                            Console.WriteLine($"From thread:  {Thread.CurrentThread.ManagedThreadId}");
                            issueStream.OnNext(issueObj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    issueStream.OnError(ex);
                }
            issueStream.OnCompleted();
            });
        }
        public IDisposable Subscribe(IObserver<IssueComment> observer)
        {
            return issueStream
                .ObserveOn(ThreadPoolScheduler
                .Instance).Subscribe(observer);
        }




    }
}
