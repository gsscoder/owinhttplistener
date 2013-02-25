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
using System.IO;
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

    public class OwinHttpListener : IDisposable
    {
        private readonly AppFunc _appFunc;
        private readonly CancellationTokenSource _cts;
        private readonly TcpListener _listener;
        private int _started;
        private int _stopped;

        public OwinHttpListener(AppFunc appFunc, IPAddress local, int port)
        {
            _appFunc = appFunc;

            UriPrefix = string.Concat("http://", local.ToString(), ":", port);

            _listener = new TcpListener(local, port);
            _cts = new CancellationTokenSource();
        }

        public OwinHttpListener(AppFunc appFunc, int port)
            : this(appFunc, new IPAddress(new byte[] {0, 0, 0, 0}), port)
        {
        }

        internal static string UriPrefix { get; private set; }

        public void Start()
        {
            if (Interlocked.CompareExchange(ref _started, 1, 0) != 0)
            {
                throw new InvalidOperationException("Listener is already started.");
            }

            _listener.Start();
        }

        public async Task ListenAsync()
        {
            Trace.WriteLine("listening started");

            if (_cts.Token.IsCancellationRequested)
            {
                Trace.WriteLine("cancelled");
                return;
            }

            var socket = await _listener.AcceptSocketAsync();

            Action accept = async () =>
                {
                    if (!socket.Connected)
                    {
                        Disconnect(null, socket);
                        return;
                    }

                    var stream = new NetworkStream(socket);

                    if (!stream.DataAvailable)
                    {
                        Disconnect(stream, socket);
                        return;
                    }

                    var environment = await CreateEnvironmentAsync(stream);

                    await _appFunc(environment);

                    var response = (Stream)environment[OwinConstants.ResponseBody];
                    if (response != null && response.Length > 0)
                    {
                        if (response.CanSeek)
                        {
                            response.Seek(0, SeekOrigin.Begin);
                        }
                        await response.CopyToAsync(stream);
                    }

                    Disconnect(stream, socket);
                };

            ThreadPool.QueueUserWorkItem(_ =>
                {
                    Trace.WriteLine(string.Format("accepted at {0}", DateTime.Now.ToLongTimeString()));
                    accept();
                });
    
            await ListenAsync();
        }

        private static async Task<IDictionary<string, object>> CreateEnvironmentAsync(NetworkStream stream)
        {
            var buffer = await stream.ToByteArrayAsync();
            var requestStream = new MemoryStream(buffer.ToArray());
            var request = await Request.FromStreamAsync(requestStream);;
            request.Body = requestStream;

            var response = new OwinResponse(request)
                {
                    Body = new MemoryStream()
                };

            return request.Dictionary;
        }

        private static void Disconnect(Stream stream, Socket socket)
        {
            try
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (socket != null)
                {
                    socket.Close();
                }
            }
            catch
            {
                Trace.WriteLine("problem disposing socket");
            }
        }

        public void Stop()
        {
            if (_started == 0)
            {
                return;
            }
            if (Interlocked.CompareExchange(ref _stopped, 1, 0) != 0)
            {
                return;
            }
            try
            {
                _cts.Cancel();
                _listener.Stop();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}