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
      //  List<IssueComment> analyzedIssues;
        public int ID { get; set; }
        public IssueCommentObserver()
        {
        //    analyzedIssues = new List<IssueReview>();
        }

        public IssueCommentObserver(int id)
        {
            ID = id;
//            analyzedIssues = new List<IssueReview>();
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
//            Thread.Sleep(ID * 1000);
//            analyzedIssues.Add(value);
            Console.WriteLine(value.GetCommentDetails());
            Console.WriteLine($" Thread {Thread.CurrentThread.ManagedThreadId}, observer {ID}");
        }
    }
}
