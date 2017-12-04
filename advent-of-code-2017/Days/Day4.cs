using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day4 : IDay
    {
        public void Part1(string input)
        {
            var result = input.Split('\n')
                              .Select(line => line.TrimEnd().Split(' '))
                              .Count(phrases => phrases.Distinct().Count() == phrases.Length);

            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            var result = input.Split('\n')
                              .Select(line => line.TrimEnd()
                                                  .Split(' ')
                                                  .Select(p => string.Concat(p.OrderBy(c => c)))
                                                  .ToList())
                              .Count(phrases => phrases.Distinct().Count() == phrases.Count);

            Console.WriteLine("Result: " + result);
        }
    }
}
