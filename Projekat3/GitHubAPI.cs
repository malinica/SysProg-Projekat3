using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using Newtonsoft.Json.Linq;
using System.Reactive.Linq;
using VaderSharp;

namespace Projekat3
{
    public class GitHubAPI
    {
        const string API_KEY = "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";
        const string BASE_URL = "https://api.github.com/repos";
        HttpClient client;
        SentimentIntensityAnalyzer analyzer;

        public GitHubAPI()
        {
            analyzer = new SentimentIntensityAnalyzer();
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));

        }
        public async Task<Repository> Search(string owner, string type, ReplaySubject<IssueComment> stream)
        {
            try
            {
                Repository r = new Repository();
                var response = await client.GetAsync($"{BASE_URL}/{owner}/{type}/issues");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseJSON = JArray.Parse(responseString);

                if (responseJSON == null)
                {
                    return null;
                }

                foreach (JObject item in responseJSON)
                {
                    var idIssue = (string)item["number"];
                    try
                    {
                        var commentsResponse = await client.GetAsync($"{BASE_URL}/{owner}/{type}/issues/{idIssue}/comments");
                        commentsResponse.EnsureSuccessStatusCode();
                        var commentsResponseString = await commentsResponse.Content.ReadAsStringAsync();
                        var obj = new Issue(idIssue, type);
                        var responseJSON2 = JArray.Parse(commentsResponseString);

                        foreach (var item2 in responseJSON2)
                        {
                            var textComment = (string)item2["body"];
                            var analyze = analyzer.PolarityScores(textComment);
                            var commObj = new IssueComment(textComment, analyze.Positive, analyze.Neutral, analyze.Negative);
                            obj.AddComment(commObj);
                            // Console.WriteLine($"Emitovano sa threada {Thread.CurrentThread.ManagedThreadId}");
                            stream.OnNext(commObj);
                        }
                        r.AddIssue(obj);
                    }
                    catch (Exception ex)
                    {
                        stream.OnError(ex);
                        return null;
                    }
                }
                return r;

            }
            catch (Exception ex)
            {
                stream.OnError(ex);
                return null;
            }
        }
    }
}