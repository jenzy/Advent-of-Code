using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using AdventOfCode.Common;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day09 : DayBase
    {
        public override object Part1()
        {
            var sb = new StringBuilder("\n");
            return sb.ToString();
        }

        public override object Part2()
        {
            var sb = new StringBuilder("\n");
            return sb.ToString();
        }

        private static IEnumerable<byte> Parse(string input) => input.Select(c => (byte)(c - '0'));

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(9);
            Assert.Equal(1742, day.Part1());
            Assert.Equal(1, day.Part2());
        }
    }
}
