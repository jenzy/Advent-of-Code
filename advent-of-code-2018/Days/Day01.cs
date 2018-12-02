using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days
{
    internal class Day01 : IDay
    {
        public string Part1(string input) => Parse(input).Sum().ToString();

        public string Part2(string input)
        {
            var set = new HashSet<int>();
            int sum = 0;

            foreach(var i in Parse(input).RepeatForever())
            {
                if (set.Contains(sum += i))
                    break;

                set.Add(sum);
            }

            return sum.ToString();
        }

        private static IEnumerable<int> Parse(string input) => input.Split("\n").Select(int.Parse);
    }
}
