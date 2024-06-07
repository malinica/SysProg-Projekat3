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

            //4ian a za repozitorijum gdevelop
            //malinica a za repozitorijum sysprog-projekat2

            Console.WriteLine("Enter the owner: ");
                string owner = Console.ReadLine();

                Console.WriteLine("Enter the name of repository: ");
                string type = Console.ReadLine();
                server.Serach(owner, type);
                Console.ReadKey();
            
        }
    }
}
