using System;
using System.Threading.Tasks;

namespace Projekat3
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var server = new Server();
            var observer1 = new IssueCommentObserver(1);
            var observer2 = new IssueCommentObserver(2);
            var observer3 = new IssueCommentObserver(3);
            var sub1=server.Subscribe(observer1);
            //var sub2=server.Subscribe(observer2);
            var sub3=server.Subscribe(observer3);
            server.Start();

            Task.Delay(20000).ContinueWith(_ =>
            {
                server.Unsubscribe(sub3);
                var sub2=server.Subscribe(observer2);
            });

            Console.WriteLine("Press Enter to stop the server...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            server.Stop();
            /*
            http://localhost:8080/4ian/gdevelop
            http://localhost:8080/RangeNetworks/dev
            http://localhost:8080/malinica/sysprog-projekat2
            */
        }
    }
}
