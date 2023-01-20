using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ExceptNull<T>(this IEnumerable<T?> source)
            where T : class
        {
            return source.Where(x => x != null)!;
        }
    }
}
