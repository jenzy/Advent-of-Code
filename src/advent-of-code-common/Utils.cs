using System.Collections.Generic;

namespace AdventOfCode.Common
{
    public static class Utils
    {
        public static IEnumerable<long> Range(long start, long count, long step = 1)
        {
            long end = start + count;
            for (long i = start; i < end; i += step)
                yield return i;
        }

        public static IEnumerable<int> Range(int start, int count, int step = 1)
        {
            int end = start + count;
            for (int i = start; i < end; i += step)
                yield return i;
        }
    }
}
