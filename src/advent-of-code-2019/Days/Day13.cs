using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day13 : DayBase
    {
        public override object Part1()
        {
            return -1;
        }

        public override object Part2()
        {
            return -1;
        }

        private static IEnumerable<Moon> Parse(string input)
        {
            return input.Split('\n')
                        .Select(x => new Moon { Position = Regex.Matches(x, @"(-?\d+)")
                                                                .Select(c => int.Parse(c.Value))
                                                                .ToArray() });
        }

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(13);
            Assert.Equal(10635, day.Part1());
            Assert.Equal(583523031727256, day.Part2());
        }
    }
}
