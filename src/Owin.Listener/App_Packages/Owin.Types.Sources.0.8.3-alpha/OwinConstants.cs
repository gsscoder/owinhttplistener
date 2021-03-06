// <copyright file="OwinConstants.cs" company="Microsoft Open Technologies, Inc.">
// Copyright 2013 Microsoft Open Technologies, Inc. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
namespace Owin.Types
{
    [System.CodeDom.Compiler.GeneratedCode("App_Packages", "")]
    internal static class OwinConstants
    {
        #region OWIN v1.0.0 - 3.2.1. Request Data
        // http://owin.org/spec/owin-1.0.0.html

        public const string RequestScheme = "owin.RequestScheme";
        public const string RequestMethod = "owin.RequestMethod";
        public const string RequestPathBase = "owin.RequestPathBase";
        public const string RequestPath = "owin.RequestPath";
        public const string RequestQueryString = "owin.RequestQueryString";
        public const string RequestProtocol = "owin.RequestProtocol";
        public const string RequestHeaders = "owin.RequestHeaders";
        public const string RequestBody = "owin.RequestBody";

        #endregion

        #region OWIN v1.0.0 - 3.2.2. Response Data
        // http://owin.org/spec/owin-1.0.0.html

        public const string ResponseStatusCode = "owin.ResponseStatusCode";
        public const string ResponseReasonPhrase = "owin.ResponseReasonPhrase";
        public const string ResponseProtocol = "owin.ResponseProtocol";
        public const string ResponseHeaders = "owin.ResponseHeaders";
        public const string ResponseBody = "owin.ResponseBody";

        #endregion

        #region OWIN v1.0.0 - 3.2.3. Other Data
        // http://owin.org/spec/owin-1.0.0.html

        public const string CallCancelled = "owin.CallCancelled";
        
        public const string OwinVersion = "owin.Version";

        #endregion

        #region OWIN Key Guidelines and Common Keys - 6. Common keys
        // http://owin.org/spec/CommonKeys.html

        internal static class CommonKeys
        {
            public const string ClientCertificate = "ssl.ClientCertificate";
            public const string RemoteIpAddress = "server.RemoteIpAddress";
            public const string RemotePort = "server.RemotePort";
            public const string LocalIpAddress = "server.LocalIpAddress";
            public const string LocalPort = "server.LocalPort";
            public const string IsLocal = "server.IsLocal";
            public const string TraceOutput = "host.TraceOutput";
            public const string Addresses = "host.Addresses";
            public const string Capabilities = "server.Capabilities";
            public const string OnSendingHeaders = "server.OnSendingHeaders";
        }

        #endregion

        #region SendFiles v0.3.0
        // http://owin.org/extensions/owin-SendFile-Extension-v0.3.0.htm

        internal static class SendFiles
        {
            // 3.1. Startup

            public const string Version = "sendfile.Version";
            public const string Support = "sendfile.Support";
            public const string Concurrency = "sendfile.Concurrency";

            // 3.2. Per Request

            public const string SendAsync = "sendfile.SendAsync";
        }

        #endregion

        #region Opaque v0.3.0
        // http://owin.org/extensions/owin-OpaqueStream-Extension-v0.3.0.htm

        internal static class Opaque
        {
            // 3.1. Startup

            public const string Version = "opaque.Version";

            // 3.2. Per Request

            public const string Upgrade = "opaque.Upgrade";

            // 5. Consumption

            public const string Stream = "opaque.Stream";
            //public const string Version = "opaque.Version"; // redundant, declared above
            public const string CallCancelled = "opaque.CallCancelled";
        }

        #endregion

        #region WebSocket v0.4.0
        // http://owin.org/extensions/owin-OpaqueStream-Extension-v0.3.0.htm

        internal static class WebSocket
        {
            // 3.1. Startup

            public const string Version = "websocket.Version";

            // 3.2. Per Request

            public const string Accept = "websocket.Accept";

            // 4. Accept

            public const string SubProtocol = "websocket.SubProtocol";

            // 5. Consumption

            public const string SendAsync = "websocket.SendAsync";
            public const string ReceiveAsync = "websocket.ReceiveAsync";
            public const string CloseAsync = "websocket.CloseAsync";
            // public const string Version = "websocket.Version"; // redundant, declared above
            public const string CallCancelled = "websocket.CallCancelled";
            public const string ClientCloseStatus = "websocket.ClientCloseStatus";
            public const string ClientCloseDescription = "websocket.ClientCloseDescription";
        }

        #endregion
    }
}
