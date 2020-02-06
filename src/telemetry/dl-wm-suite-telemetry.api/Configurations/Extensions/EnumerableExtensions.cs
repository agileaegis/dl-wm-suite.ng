using System;
using System.Collections.Generic;

namespace dl.wm.suite.telemetry.api.Configurations.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}