using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day1 : IDay
    {
        public void Part1(string input)
        {
            Console.WriteLine("Result: " + Common(input, 1));
        }

        public void Part2(string input)
        {
            Console.WriteLine("Result: " + Common(input, input.Length/2));
        }

        private static int Common(string input, int offset) =>
            input.Where((cur, i) => cur == input[(i + offset) % input.Length])
                 .Sum(t => (int) char.GetNumericValue(t));
    }
}
