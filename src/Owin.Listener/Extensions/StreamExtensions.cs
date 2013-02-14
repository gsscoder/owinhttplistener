using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Owin.Listener.Extensions
{
    static class StreamExtensions
    {
        public static async Task<ArraySegment<byte>> ToByteArrayAsync(this NetworkStream stream)
        {
            var buffer = new byte[1024];
            var offset = 0;
            var bytesTotal = 0;
            while (stream.DataAvailable)
            {
                var temp = new byte[256];
                var bytesRead = await stream.ReadAsync(temp, 0, temp.Length);
                if (bytesRead == 0)
                {
                    break;
                }
                bytesTotal += bytesRead;
                if (bytesTotal >= buffer.Length)
                {
                    var extended = new byte[buffer.Length * 2];
                    Buffer.BlockCopy(buffer, 0, extended, 0, buffer.Length);
                    buffer = extended;
                }
                Buffer.BlockCopy(temp, 0, buffer, offset, bytesRead);
                offset = bytesTotal;
            }
            return new ArraySegment<byte>(buffer, 0, bytesTotal);
        }
    }
}
