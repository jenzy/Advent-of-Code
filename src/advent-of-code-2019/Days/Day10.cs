using AdventOfCode.Y2019.Common;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day10 : DayBase
    {
        public override object Part1() => string.Join(", ", new Intcode(Parse(Input), Enumerable.Repeat(1L, 1)).Run().Output);

        public override object Part2() => string.Join(", ", new Intcode(Parse(Input), Enumerable.Repeat(2L, 1)).Run().Output);

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(10);
            Assert.Equal("3454977209", day.Part1());
            Assert.Equal("50120", day.Part2());
        }
    }
}

