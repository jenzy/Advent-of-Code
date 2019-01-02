using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day7 : IDay
    {
        public void Part1(string input)
        {
            var first = ParseInput(input);
            Console.WriteLine("Result: " + first.Name);
        }

        public void Part2(string input)
        {
            int result;
            var node = ParseInput(input);

            while (true)
            {
                var weights = node.Others.GroupBy(o => o.TotalWeight).ToList();
                if (weights.Count == 1)
                    throw new Exception("all equal");

                var different = weights.Single(w => w.Count() == 1).First();
                var differentWeights = different.Others.GroupBy(o => o.TotalWeight).ToList();
                if (differentWeights.Count == 1)
                {
                    var okProgs = weights.Single(w => w.Count() > 1).First();
                    result = okProgs.TotalWeight - different.Others.Sum(p => p.TotalWeight);
                    break;
                }

                node = different;
            }

            Console.WriteLine("Result: " + result);
        }

        private static Prog ParseInput(string input)
        {
            var all = input.Split('\n')
                           .Select(i => new Prog(i))
                           .ToDictionary(p => p.Name);

            foreach (var prog in all.Values.Where(p => p.OthersNames != null))
                prog.Others = prog.OthersNames.Select(n => all[n]).ToList();

            var notFirst = all.Values
                              .Where(p => p.OthersNames != null)
                              .SelectMany(p => p.OthersNames)
                              .ToHashSet();

            return all.Values.Single(p => p.OthersNames != null && !notFirst.Contains(p.Name));
        }

        private class Prog
        {
            private int? totalWeight;

            private int Weight { get; }

            public int TotalWeight => (int) (totalWeight ?? (totalWeight = Weight + (Others?.Sum(p => p.TotalWeight) ?? 0)));

            public string Name { get; }

            public List<Prog> Others { get; set; }

            public List<string> OthersNames { get; }

            public Prog(string input)
            {
                var iParenL = input.IndexOf("(", StringComparison.InvariantCulture);
                var iParenR = input.IndexOf(")", StringComparison.InvariantCulture);
                Name = input.Substring(0, iParenL).Trim();
                Weight = int.Parse(input.Substring(iParenL+1, iParenR - iParenL - 1).Trim());
                var iArrow = input.IndexOf(">", StringComparison.InvariantCulture);
                if (iArrow > 0)
                    OthersNames = input.Substring(iArrow + 1).Split(',').Select(x => x.Trim()).ToList();
            }
        }

    }
}
