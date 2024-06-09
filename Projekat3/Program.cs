using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            server.Subscribe(observer1);
            server.Subscribe(observer2);
            server.Start();
            //http://localhost:8080/malinica/sysprog-projekat2
            //http://localhost:8080/4ian/gdevelop
        }
    }
}
