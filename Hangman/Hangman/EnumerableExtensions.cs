using System;
using System.Collections.Generic;
using System.Text;

namespace Hangman
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(int, T)> EachWithIndex<T>(this IEnumerable<T> enumerable)
        {
            int i = 0;
            foreach (var item in enumerable)
            {
                yield return (i, item);
                i++;
            }
        }
    }
}
