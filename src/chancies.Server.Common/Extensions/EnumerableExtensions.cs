using System;
using System.Collections.Generic;

namespace chancies.Server.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
        }
    }
}
