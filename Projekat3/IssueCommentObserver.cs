using System;
using System.Threading;

namespace Projekat3
{
    public class IssueCommentObserver : IObserver<IssueComment>
    {
        public int ID { get; set; }
        public IssueCommentObserver() { }

        public IssueCommentObserver(int id)
        {
            ID = id;
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
            Console.WriteLine($" Thread {Thread.CurrentThread.ManagedThreadId}, observer {ID} \n");
            Console.WriteLine(value.GetCommentDetails());
        }
    }
}
