using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpHelpers;
using Owin.Listener.Extensions;
using Owin.Types;

namespace Owin.Listener
{
    class RequestCallbacks : IHttpParserCallbacks 
    {
        //public bool HasError { get; private set; }

        public OwinRequest Request;

        public void OnMessageBegin()
        {
            Request = OwinRequest.Create();
        }

        public void OnRequestLine(string method, string uriString, string version)
        {
            Request.Method = method;
            Uri uri;
            if (Uri.TryCreate(string.Concat(OwinHttpListener.UriPrefix, uriString), UriKind.Absolute, out uri))
            {
                SetUri(uri);
            }
            Request.Protocol = version;
        }

        public void OnResponseLine(string version, int? code, string reason)
        {
            throw new NotSupportedException();
        }

        public void OnHeaderLine(string name, string value)
        {
            Request.SetHeader(name, value);
        }

        public void OnBody(ArraySegment<byte> data)
        {
            Request.Body = new MemoryStream(data.ToArray());
        }

        public void OnMessageEnd()
        {
        }

        private void SetUri(Uri uri)
        {
            Request.Host = uri.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped);
            Request.PathBase = String.Empty;
            Request.Path = uri.LocalPath;
            Request.Scheme = uri.Scheme;
            Request.QueryString = uri.Query.Length > 0 ? uri.Query.Substring(1) : String.Empty;
        }
    }
}
