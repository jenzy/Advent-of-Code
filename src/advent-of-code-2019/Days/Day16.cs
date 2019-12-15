using AdventOfCode.Common;
using AdventOfCode.Y2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day16 : DayBase
    {
        public override object Part1()
        {
            return 0;
        }

        public override object Part2()
        {
            return 0;
        }

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(16);
            Assert.Equal(248, day.Part1());
            Assert.Equal(382, day.Part2());
        }
    }
}
