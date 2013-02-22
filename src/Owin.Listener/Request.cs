#region License
//
// Owin Http Listener: SharedAssemblyInfo.cs
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
using System.IO;
using System.Threading.Tasks;
using Owin.Types;
using HttpHelpers;
#endregion

namespace Owin.Listener
{
    static class Request
    {
        public static async Task<OwinRequest> FromStreamAsync(Stream stream)
        {
            var request = OwinRequest.Create();
            var result = await HttpParser.ParseMessageAsync(stream,
                (method, uriString, version) =>
                    {
                        request.Method = method;
                        Uri uri;
                        if (Uri.TryCreate(string.Concat(OwinHttpListener.UriPrefix, uriString), UriKind.Absolute, out uri))
                        {
                            request = SetUri(request, uri);
                        }
                        request.Protocol = version;
                    },
                (header, value) =>
                        request.AddHeader(header, value));
            return request;
        }

        private static OwinRequest SetUri(OwinRequest request, Uri uri)
        {
            request.Host = uri.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped);
            request.PathBase = String.Empty;
            request.Path = uri.LocalPath;
            request.Scheme = uri.Scheme;
            request.QueryString = uri.Query.Length > 0 ? uri.Query.Substring(1) : String.Empty;
            return request;
        }
    }
}
