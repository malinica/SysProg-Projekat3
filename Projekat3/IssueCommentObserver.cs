using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VaderSharp;

namespace Projekat3
{
    public class IssueCommentObserver : IObserver<IssueComment>
    {
        SentimentIntensityAnalyzer analyzer;
        List<IssueReview> analyzedIssues;
        public int ID { get; set; }
        public IssueCommentObserver()
        {
            analyzer = new SentimentIntensityAnalyzer();
            analyzedIssues = new List<IssueReview>();
        }

        public IssueCommentObserver(int id)
        {
            ID = id;
            analyzer = new SentimentIntensityAnalyzer();
            analyzedIssues = new List<IssueReview>();
        }


        public void OnCompleted()
        {
            Console.WriteLine("Done");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(IssueComment value)
        {
            //Thread.Sleep(ID * 1000);
            var analyze = analyzer.PolarityScores($"{value.commentText}");
            analyzedIssues.Add(new IssueReview(value, analyze.Positive, analyze.Negative, analyze.Neutral));
            Console.WriteLine($"Issue: {value.Issue.title},commentID: {value.commentID}, comment: {value.commentText}, positive: {analyze.Positive}, negative: {analyze.Negative}, neutral: {analyze.Neutral} ");
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}, observer {ID}");
        }
    }
}
