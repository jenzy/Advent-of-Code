using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*


     */

    public class Day17 : DayBase
    {
        public override object Part1()
        {
            var data = Parse(Input);

            for (int round = 1; round <= 100; round++)
            {
                var newData = new int[data.Length];
                Parallel.For(0, data.Length, i => newData[i] = Math.Abs(data.Zip(GetPattern(i + 1, data.Length), (d, p) => d * p).Sum()) % 10);
                data = newData;
            }

            return string.Join(string.Empty, data.Take(8));
        }

        public override object Part2()
        {
            var data = Parse(Input);
            int offset = int.Parse(string.Join(string.Empty, data.Take(7)));
            data = data.AsEnumerable().RepeatForever().Skip(offset).Take((10_000 * data.Length) - offset).ToArray();
            if (offset <= data.Length / 2)
                throw new Exception("offest not in second half");

            for (int round = 0; round < 100; round++)
            {
                for (int i = data.Length - 2; i >= 0; --i)
                    data[i] = Math.Abs(data[i + 1] + data[i]) % 10;
            }

            return string.Join(string.Empty, data.Take(8));
        }

        private static int[] Parse(string input) => input.Select(x => (int)(x - '0')).ToArray();

        private static IEnumerable<int> GetPattern(int round, int len)
        {
            return Generate().RepeatForever().Skip(1).Take(len);

            IEnumerable<int> Generate()
            {
                foreach (var num in new[] { 0, 1, 0, -1 })
                {
                    for (int i = round; i > 0; --i)
                        yield return num;
                }
            }
        }

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(17);
            Assert.Equal("19944447", day.Part1());
            Assert.Equal("81207421", day.Part2());
        }
    }
}
