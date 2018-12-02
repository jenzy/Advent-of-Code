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
    }
}
