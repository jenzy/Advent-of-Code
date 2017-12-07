using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day7 : IDay
    {
        public void Part1(string input)
        {
//            input = @"pbga (66)
//xhth (57)
//ebii (61)
//havc (66)
//ktlj (57)
//fwft (72) -> ktlj, cntj, xhth
//qoyq (66)
//padx (45) -> pbga, havc, qoyq
//tknk (41) -> ugml, padx, fwft
//jptl (61)
//ugml (68) -> gyxo, ebii, jptl
//gyxo (61)
//cntj (57)";

            var all = ParseInput(input);

            var notFirst = all.Values
                              .Where(p => p.OthersNames != null)
                              .SelectMany(p => p.OthersNames)
                              .ToHashSet();

            var first = all.Values
                           .First(p => p.OthersNames != null && !notFirst.Contains(p.Name));


            Console.WriteLine("Result: " + first.Name);
        }

        public void Part2(string input)
        {
            //Console.WriteLine("Result: " + result);
        }

        private static Dictionary<string, Prog> ParseInput(string input)
        {
            var all = input.Split('\n')
                           .Select(i => new Prog(i))
                           .ToDictionary(p => p.Name);

            return all;
        }

        class Prog
        {
            public int Weight { get; set; }

            public string Name { get; set; }

            public List<Prog> Others { get; set; }

            public List<string> OthersNames { get; set; }

            public string Input { get; set; }

            public Prog(string input)
            {
                var iParenL = input.IndexOf("(", StringComparison.InvariantCulture);
                var iParenR = input.IndexOf(")", StringComparison.InvariantCulture);
                Name = input.Substring(0, iParenL).Trim();
                Weight = int.Parse(input.Substring(iParenL+1, iParenR - iParenL - 1).Trim());
                var iArrow = input.IndexOf(">", StringComparison.InvariantCulture);
                OthersNames = input.Substring(iArrow + 1).Split(',').Select(x => x.Trim()).ToList();
            }
        }

    }
}
