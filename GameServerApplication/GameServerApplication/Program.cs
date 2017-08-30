using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace GameServerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //MServer server = MServer.Instance;
            //server.Start(new IPEndPoint(IPAddress.Parse("192.168.1.10"), 26680));

            MasterApplication application = new MasterApplication(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26680));


            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
