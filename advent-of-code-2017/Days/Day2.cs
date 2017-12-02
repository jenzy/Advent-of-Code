using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day2 : IDay
    {
        public void Part1(string input)
        {
            int sum = ParseInput(input).Sum(row => row.Max() - row.Min());

            Console.WriteLine("Result: " + sum);
        }

        public void Part2(string input)
        {
            int sum = ParseInput(input)
                .Select(row => (from a in row
                                from b in row
                                where a > b && a % b == 0
                                select a / b).First())
                .Sum();

            Console.WriteLine("Result: " + sum);
        }

        private static List<List<int>> ParseInput(string input) =>
            input.Split('\n')
                 .Select(l => l.Split('\t')
                               .Select(int.Parse)
                               .ToList())
                 .ToList();
    }
}
