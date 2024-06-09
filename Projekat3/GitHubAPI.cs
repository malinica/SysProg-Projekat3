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
using VaderSharp;



namespace Projekat3
{
    public class GitHubAPI
    {
        const string API_KEY = "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";
        const string BASE_URL = "https://api.github.com/repos";
        HttpClient client;
        SentimentIntensityAnalyzer analyzer;
        List<IssueComment> comments;


        public GitHubAPI()
        {
            analyzer = new SentimentIntensityAnalyzer();
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
            comments = new List<IssueComment>();

        }
        public void Search(string owner, string type, IScheduler scheduler, Subject<IssueComment> stream)
        {
            Observable.Start(async () =>
            {
                try
                {
                    var response = await client.GetAsync($"{BASE_URL}/{owner}/{type}/issues");
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseJSON = JArray.Parse(responseString);

                    if (responseJSON == null)
                    {
                        stream.OnCompleted();
                        return;
                    }

                    foreach (JObject item in responseJSON)
                    {
                        var idIssue = (string)item["number"];

                        var commentsResponse = await client.GetAsync($"{BASE_URL}/{owner}/{type}/issues/{idIssue}/comments");
                        var commentsResponseString = await commentsResponse.Content.ReadAsStringAsync();

                        var responseJSON2 = JArray.Parse(commentsResponseString);
                        foreach (var item2 in responseJSON2)
                        {
                            var textComment = (string)item2["body"];
                            var analyze = analyzer.PolarityScores(textComment);
                            var obj = new IssueComment(idIssue, owner, type, textComment, analyze.Positive, analyze.Neutral, analyze.Negative);
                            stream.OnNext(obj);
                        }
                    }
                    stream.OnCompleted();
                }
                catch (Exception ex)
                {
                    stream.OnError(ex); 
                }
            }, scheduler);
        }

    }
}

