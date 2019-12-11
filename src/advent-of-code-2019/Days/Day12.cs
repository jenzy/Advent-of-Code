using AdventOfCode.Common;
using AdventOfCode.Y2019.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day12 : DayBase
    {
        public override object Part1()
        {
            return -1;
        }

        public override object Part2()
        {
            var sb = new StringBuilder("\n");
            return sb.ToString();
        }

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(11);
            Assert.Equal(1747, day.Part1());
            Assert.Equal(1, day.Part2());
        }
    }
}
