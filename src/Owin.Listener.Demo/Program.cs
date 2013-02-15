using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Owin.Listener.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new OwinHttpListener(new OwinApplication().ProcessRequest, 8899);
            listener.Start();

            listener.ListenAsync().Wait();

            Console.ReadKey();

            listener.Stop();
        }
    }
}
