using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day03 : DayBase
    {
        public override object Part1()
        {
            throw new Exception("no result");
        }

        public override object Part2()
        {
            throw new Exception("no result");
        }

        private static IEnumerable<int> Parse(string input) => input.Split(",").Select(int.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(3);
            Assert.Equal(3716293, day.Part1());
            Assert.Equal(6429, day.Part2());
        }
    }
}
