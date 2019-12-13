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

    public class Day14 : DayBase
    {
        public override object Part1()
        {
            var output = new Intcode(Parse(Input)).Run().Output;

            var grid = new Dictionary<(int x, int y), int>();
            
            return grid.Values.Count(x => x == 2);
        }

        public override object Part2()
        {
            var memory = Parse(Input).ToList();
            memory[0] = 2;
            var game = new Intcode(memory);
            var grid = new Dictionary<(int x, int y), int>();
            bool manual = false;
            int xBall = -1, xPaddle = -1, score = 0;

            return score;
        }

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(14);
            Assert.Equal(320, day.Part1());
            Assert.Equal(15156, day.Part2());
        }
    }
}
