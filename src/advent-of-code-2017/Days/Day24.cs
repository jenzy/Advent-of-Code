using System;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day24 : IDay
    {
        public void Part1(string input)
        {
            int Build(IImmutableList<(int a, int b)> parts, int connect, int strength) =>
                parts.Where(p => p.a == connect || p.b == connect)
                     .Select(part => Build(parts.Remove(part), part.a == connect ? part.b : part.a, strength + part.a + part.b))
                     .Concat(Enumerable.Repeat(strength, 1))
                     .Max();

            Console.WriteLine("Result: " + Build(Parse(input), 0, 0));
        }

        public void Part2(string input)
        {
            (int length, int strength) Build(ImmutableList<(int a, int b)> parts, int connect, int length, int strength) =>
                parts.Where(p => p.a == connect || p.b == connect)
                     .Select(part => Build(parts.Remove(part), part.a == connect ? part.b : part.a, length + 1, strength + part.a + part.b))
                     .Concat(Enumerable.Repeat((length, strength), 1))
                     .OrderByDescending(x => x.length)
                     .ThenByDescending(x => x.strength)
                     .First();

            Console.WriteLine("Result: " + Build(Parse(input), 0, 0, 0).strength);
        }

        private static ImmutableList<(int a, int b)> Parse(string input) =>
            input.Split('\n')
                 .Select(x => x.Trim().Split('/'))
                 .Select(x => (a: int.Parse(x[0]), b: int.Parse(x[1]))).ToImmutableList();
    }
}
