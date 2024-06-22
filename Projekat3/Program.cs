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
            server.Subscribe(observer1);
            //server.Subscribe(observer2);
            server.Start();

            Task.Delay(20000).ContinueWith(_ =>
            {
                server.Subscribe(observer2);
                Console.WriteLine("Observer 2 je sada pretplaćen na server događaje.");
            });

            Console.WriteLine("Press Enter to stop the server...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            server.Stop();
            /*
            POSLATI SLEDECI ZAHTEV ODMAH PRI POKRETANJU, OPSLUZUJE SE OBSERVER 1
            http://localhost:8080/4ian/gdevelop
            NAKON NEKOLIKO SEKUNDI SUBSCRIBUJE SE I OBSERVER 2 I ON DOBIJA NOTIFIKACIJE OD PRE
            NASTAVITI SA RADOM DALJE SLANJEM OSTALIH UPITA I NA KRAJU KLIKNUTI ENTE
            http://localhost:8080/RangeNetworks/dev
            http://localhost:8080/malinica/sysprog-projekat2
            */
        }
    }
}
