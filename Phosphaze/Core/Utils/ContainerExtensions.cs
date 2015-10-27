using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze.Core.Utils
{
    public static class DictionaryExtensions
    {

        public static void Merge<K, V>(this Dictionary<K, V> self, Dictionary<K, V> other)
        {
            foreach (KeyValuePair<K, V> p in other)
                self[p.Key] = p.Value;
        }

    }

    public static class ArrayExtensions
    {

        public static bool AllEqual<T>(this T[] self, T item)
        {
            foreach (T t in self)
                if (!t.Equals(item))
                    return false;
            return true;
        }

    }
}
