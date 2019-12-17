using AdventOfCode.Y2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day18 : DayBase
    {
        public override object Part1()
        {
            return -1;
        }

        public override object Part2()
        {
            return -1;
        }

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);


        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(18);
            Assert.Equal(8408, day.Part1());
            Assert.Equal(1168948L, day.Part2());
        }
    }
}