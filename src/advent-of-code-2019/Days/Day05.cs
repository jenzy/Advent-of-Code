using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day05 : DayBase
    {
        public override object Part1()
        {
            var limits = Parse(Input);
            int count = 0;
            return count;
        }

        public override object Part2()
        {
            var limits = Parse(Input);
            int count = 0;
            return count;
        }

        private static List<int> Parse(string input) => input.Split("-").Select(int.Parse).ToList();

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(5);
            Assert.Equal(1675, day.Part1());
            Assert.Equal(1142, day.Part2());
        }
    }
}
