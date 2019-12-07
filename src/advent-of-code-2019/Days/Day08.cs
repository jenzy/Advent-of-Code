using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day08 : DayBase
    {
        public override object Part1()
        {
            var orbits = Parse(Input);
            return Recurse("COM", 0);
            
            int Recurse(string curr, int depth) => depth + orbits[curr].Sum(orb => Recurse(orb.planet, depth + 1));
        }

        public override object Part2()
        {
            return null;
        }

        private static ILookup<string, (string center, string planet)> Parse(string input) => input.Split("\n").Select(x => x.Split(")")).ToLookup(x => x[0], x => (center: x[0], planet: x[1]));

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(8);
            Assert.Equal(254447, day.Part1());
            Assert.Equal(445, day.Part2());
        }
    }
}
