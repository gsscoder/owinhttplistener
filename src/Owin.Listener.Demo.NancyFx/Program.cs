namespace Owin.Listener.Demo.NancyFx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Nancy;
    using Nancy.Owin;

    class Program
    {
        static void Main(string[] args)
        {
            var nancyOwin = new NancyOwinHost(EmptyApp, new DefaultNancyBootstrapper());

            var listener = new OwinHttpListener(nancyOwin.Invoke, new IPAddress(new byte[] { 0, 0, 0, 0 }), 8899);

            listener.Start();
            listener.ListenAsync().Wait();

            Console.ReadLine();

            listener.Stop();
        }

        static Task EmptyApp(IDictionary<string, object> env)
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }
    }
}
