using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*
     */

    public class Day15 : DayBase
    {
        public override object Part1() => 0;

        public override object Part2()
        {
            
            return 0;
        }
        private static Dictionary<string, Reaction> Parse(string input)
        {
            return input.Split('\n')
                        .Select(line => line.Split(" => "))
                        .Select(spl => new Reaction(
                            new Chemical(spl[1]),
                            spl[0].Split(", ").Select(i => new Chemical(i))))
                        .ToDictionary(x => x.Output.Name);
        }

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(15);
            Assert.Equal(216477L, day.Part1());
            Assert.Equal(11788286L, day.Part2());
        }
    }
}
