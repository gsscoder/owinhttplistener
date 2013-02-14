using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin.Types;

namespace Owin.Listener.Demo
{
    class OwinApplication
    {
        public async Task ProcessRequest(IDictionary<string, object> environment)
        {
            //await Task.Run(() => {
                Trace.WriteLine("ENTER OwinApplication::ProcessRequest");
                var requestPath = environment.TryGet<string>(OwinConstants.RequestPath);
                if (!string.IsNullOrEmpty(requestPath))
                {
                    Trace.WriteLine(requestPath);
                }
                var ns = environment.TryGet<Stream>(OwinConstants.ResponseBody);
                if (ns != null)
                {
                    var method = environment.TryGet<string>(OwinConstants.RequestMethod);
                    if (!string.IsNullOrEmpty(method))
                    {
                        Trace.WriteLine(method);
                    }
                    //{
                    //    var buffer = method.ToUpperInvariant() == "GET"
                    //        ? GetResponseWithContent()
                    //        : GetResponseEmpty();
                    var buffer = GetResponseWithContent();
                        await ns.WriteAsync(buffer, 0, buffer.Length);
                    //}
                }
                else
                {
                    Trace.WriteLine("ERROR: no response stream");
                }
            //});
        }

        private static byte[] GetResponseEmpty()
        {
            var builder = new StringBuilder();
            builder.Append("HTTP/1.1 200 OK\r\n\r\n");
            return Encoding.UTF8.GetBytes(builder.ToString());
        }

        private static byte[] GetResponseWithContent()
        {
            var builder = new StringBuilder();
            builder.Append("HTTP/1.1 200 OK\r\n");
            builder.Append("Content-Type: text/html\r\n");
            builder.Append("Date: ");
            builder.Append(DateTime.Now.ToUniversalTime().ToString("R"));
            builder.Append("\r\n");
            builder.Append("Cache-Control: no-cache\r\n\r\n");
            builder.Append("<html><head><title>Owin Application</title></head>\r\n");
            builder.Append("<body><p>\r\n");
            builder.Append("Hello, from OWIN AppFunc; ");
            builder.Append(DateTime.Now.ToLongTimeString());
            builder.Append("\r\n");
            builder.Append("</p></body>\r\n");
            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}
