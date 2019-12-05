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

    public class Day06 : DayBase
    {
        public override object Part1() => new Day02.Intcode(Parse(Input), Enumerable.Repeat(1, 1)).Run().Output.Last();
        
        public override object Part2() => new Day02.Intcode(Parse(Input), Enumerable.Repeat(5, 1)).Run().Output.Last();

        private static IEnumerable<int> Parse(string input) => input.Split(",").Select(int.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(6);
            Assert.Equal(4601506, day.Part1());
            Assert.Equal(5525561, day.Part2());
        }
    }
}
