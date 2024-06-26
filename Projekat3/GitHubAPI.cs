﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using Newtonsoft.Json.Linq;
using System.Reactive.Linq;
using VaderSharp;
using System.Collections.Generic;

namespace Projekat3
{
    public class GitHubAPI
    {
        private const string API_KEY = "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";
        private const string BASE_URL = "https://api.github.com/repos";
        private HttpClient client;
        private SentimentIntensityAnalyzer analyzer;

        public GitHubAPI()
        {
            analyzer = new SentimentIntensityAnalyzer();
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));

        }

        public async Task<List<Issue>> Search(string owner, string type)
        {
            try
            {
                var response = await client.GetAsync($"{BASE_URL}/{owner}/{type}/issues");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseJSON = JArray.Parse(responseString);
                List<Issue> iList = new List<Issue>();
                if (responseJSON == null)
                {
                    return null;
                }

                foreach (JObject item in responseJSON)
                {
                    var idIssue = (string)item["number"];
                    var login = (string)item["user"]["login"];
                    var iText = (string)item["title"];
                    try
                    {
                        var commentsResponse = await client.GetAsync($"{BASE_URL}/{owner}/{type}/issues/{idIssue}/comments");
                        commentsResponse.EnsureSuccessStatusCode();
                        var commentsResponseString = await commentsResponse.Content.ReadAsStringAsync();
                        var obj = new Issue(idIssue, iText, login);
                        var responseJSON2 = JArray.Parse(commentsResponseString);

                        foreach (var item2 in responseJSON2)
                        {
                            var textComment = (string)item2["body"];
                            var analyze = analyzer.PolarityScores(textComment);
                            var commObj = new IssueComment(textComment, analyze.Positive, analyze.Neutral, analyze.Negative);
                            obj.AddComment(commObj);
                            // Console.WriteLine($"Emitovano sa threada {Thread.CurrentThread.ManagedThreadId}");
                        }
                        iList.Add(obj);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error while searching comments for issue");
                        return null;
                    }
                }
                return iList;

            }
            catch (Exception)
            {
                Console.WriteLine("Error while searching issue");
                return null;
            }
        }
    }
}