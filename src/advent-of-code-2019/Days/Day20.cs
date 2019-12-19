using AdventOfCode.Y2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day20 : DayBase
    {
        public override object Part1()
        {
            var program = Parse(Input).ToList();
            return -1;
        }

        public override object Part2()
        {
            var program = Parse(Input).ToList();
            return -1;
        }

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(20);
            Assert.Equal(141, day.Part1());
            Assert.Equal(15641348, day.Part2());
        }
    }
}