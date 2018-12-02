using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    public static class Utilities
    {
        public static IEnumerable<T> RepeatForever<T>(this T item)
        {
            for(;;)
                yield return item;
        }

        public static IEnumerable<T> RepeatForever<T>(this IEnumerable<T> seq) => seq.RepeatForever<IEnumerable<T>>().SelectMany(x => x);

        public static IEnumerable<(T1 a, T2 b)> Zip<T1, T2>(this IEnumerable<T1> items1, IEnumerable<T2> items2) => items1.Zip(items2, (a, b) => (a, b));
    }
}
