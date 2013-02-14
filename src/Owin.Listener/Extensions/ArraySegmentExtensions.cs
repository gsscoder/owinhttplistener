using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owin.Listener.Extensions
{
    static class ArraySegmentExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this ArraySegment<T> segment)
        {
            return segment.Array.Take(segment.Count);
        }

        public static T[] ToArray<T>(this ArraySegment<T> segment)
        {
            return segment.ToEnumerable().ToArray();
        }
    }
}
