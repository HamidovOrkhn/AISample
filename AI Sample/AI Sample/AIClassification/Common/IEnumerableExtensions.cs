using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_Sample.AIClassification.Common
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> HasElements<T>(this IEnumerable<T> array, Action action)
        {
            if (array.Any())
                action();
            return array;
        }
        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach (var item in array)
                action(item);
        }
    }
}
