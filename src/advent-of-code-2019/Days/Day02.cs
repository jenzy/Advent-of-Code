using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2018.Days
{
    /*

     */

    public class Day02 : DayBase
    {
        public override object Part1() => Parse(Input).Select(x => (x / 3) - 2).Sum();

        public override object Part2() => Parse(Input).Select(x => Fuel(x) - x).Sum();

        private static int Fuel(int mass) => mass <= 0 ? 0 : (mass + Fuel((mass / 3) - 2));

        private static IEnumerable<int> Parse(string input) => input.Split("\n").Select(int.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(2);
            Assert.Equal(3336439, day.Part1());
            Assert.Equal(5001791, day.Part2());
        }
    }
}
