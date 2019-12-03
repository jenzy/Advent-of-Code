using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day04 : DayBase
    {
        public override object Part1()
        {
            return -1;
        }

        public override object Part2()
        {
            return -1;
        }

        private static List<string[]> Parse(string input) => input.Split("\n").Select(x => x.Split(",")).ToList();

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(4);
            Assert.Equal(3247, day.Part1());
            Assert.Equal(48054, day.Part2());
        }
    }
}
