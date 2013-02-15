#region License
//
// Owin Http Listener: OwinHttpLister.cs
//
// Author:
//   Giacomo Stelluti Scala (gsscoder@gmail.com)
//
// Copyright (C) 2013 Giacomo Stelluti Scala
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
#endregion
#region Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpHelpers;
using Owin.Listener.Extensions;
using Owin.Types;
#endregion

namespace Owin.Listener
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class OwinHttpListener
    {
        public OwinHttpListener(AppFunc appFunc, IPAddress localEp, int port)
        {
            _appFunc = appFunc;
            LocalEndPoint = localEp;
            Port = port;
            UriPrefix = string.Concat("http://", Dns.GetHostName(), ":", port);
            _listener = new TcpListener(localEp, port);
            _cts = new CancellationTokenSource();
        }

        public IPAddress LocalEndPoint { get; private set; }

        public int Port { get; private set; }

        internal static string UriPrefix { get; private set; }

        public void Start()
        {
            _listener.Start();
        }

        public async Task ListenAsync()
        {
            Trace.WriteLine("ENTER ListenAsync");

            if (_cts.Token.IsCancellationRequested)
            {
                Trace.WriteLine("cancelled");
                return;
            }

            var socket = await _listener.AcceptSocketAsync();
            var stream = new NetworkStream(socket);

            //Trace.WriteLine(Encoding.UTF8.GetString(buffer.Array.Take(buffer.Count).ToArray()));

            var environment = await CreateEnvironmentAsync(stream);

            await _appFunc(environment);

            stream.Close();
            socket.Close();

            await ListenAsync();
        }

        private static async Task<IDictionary<string, object>> CreateEnvironmentAsync(NetworkStream stream)
        {
            var request = await Request.FromStreamAsync(stream);
            request.Body = stream;

            var response = new OwinResponse(request)
                {
                    Body = stream
                };

            return request.Dictionary;
        }

        public void Stop()
        {
            _cts.Cancel();
            _listener.Stop();
        }

        private readonly AppFunc _appFunc;
        private readonly CancellationTokenSource _cts;
        private readonly TcpListener _listener;
    }
}
