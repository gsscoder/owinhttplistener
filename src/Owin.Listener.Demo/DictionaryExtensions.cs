using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owin.Listener.Demo
{
    static class DictionaryExtensions
    {
        public static T TryGet<T>(this IDictionary<string, object> dictionary, string key)
            where T : class
        {
            return dictionary.ContainsKey(key) ? dictionary[key] as T : null;
        }
    }
}
