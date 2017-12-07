using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
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
            var first = ParseInput(input);

            Prog node = first;
            while (true)
            {
                var weights = node.Others.GroupBy(o => o.TotalWeight).ToList();
                if (weights.Count == 1)
                {
                    throw new Exception("all equal");
                }
                else
                {
                    var odd = weights.Single(w => w.Count() == 1).First();
                    var oddWeights = odd.Others.GroupBy(o => o.TotalWeight).ToList();
                    if (oddWeights.Count == 1)
                    {
                        var goods = weights.Single(w => w.Count() > 1).First();
                        Console.WriteLine(goods.TotalWeight - odd.Others.Sum(p => p.TotalWeight));
                        return;
                    }
                    else
                    {
                        node = odd;
                    }
                }
            }
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

            public int Weight { get; set; }

            //public int TotalWeight { get; set; }

            public int TotalWeight
            {
                get
                {
                    if (totalWeight == null)
                    {
                        totalWeight = Weight + (Others?.Sum(p => p.TotalWeight) ?? 0);
                    }

                    return totalWeight ?? 0;
                }
            }

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
                if (iArrow > 0)
                    OthersNames = input.Substring(iArrow + 1).Split(',').Select(x => x.Trim()).ToList();
            }
        }

    }
}
