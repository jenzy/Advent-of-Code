using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Days
{
    /*

     */
    internal class Day07 : IDay
    {
        public object Part1(string input)
        {
            var graph = Parse(input);
            var result = new List<string>();

            var set = new HashSet<string>();
            var all = graph.SelectMany(x => x).Concat(graph.Select(x => x.Key)).Distinct().ToHashSet();

            while (all.Any())
            {
                var step = graph.Where(g => all.Contains(g.Key) && g.All(p => set.Contains(p)))
                                .Select(g => g.Key)
                                .Concat(graph.SelectMany(x => x).Where(x => !graph.Contains(x) && !set.Contains(x)))
                                .OrderBy(x => x)
                                .First();
                all.Remove(step);
                set.Add(step);
                result.Add(step);
            }

            return string.Join("", result);
        }

        public object Part2(string input)
        {
            var graph = Parse(input);
            var result = new List<string>();

            var set = new HashSet<string>();
            var all = graph.SelectMany(x => x).Concat(graph.Select(x => x.Key)).Distinct().ToHashSet();

            int second = 0;
            var workers = new Dictionary<string, int>();

            while (all.Any())
            {
                foreach (var stepDone in workers.Where(x => x.Value == second).Select(x=>x.Key).ToList())
                {
                    set.Add(stepDone);
                    all.Remove(stepDone);
                    result.Add(stepDone);
                    workers.Remove(stepDone);
                }

                if (workers.Count < 5)
                {
                    while (workers.Count < 5)
                    {
                        var step = graph.Where(g => all.Contains(g.Key) && g.All(p => set.Contains(p)))
                                        .Select(g => g.Key)
                                        .Concat(graph.SelectMany(x => x).Where(x => !graph.Contains(x) && !set.Contains(x)))
                                        .Where(x => !workers.ContainsKey(x))
                                        .OrderBy(x => x)
                                        .FirstOrDefault();

                        if (step == null)
                            break;

                        if (step != null)
                        {
                            workers[step] = second + 60 + (int) step[0] - (int) 'A' + 1;
                        }

                    }
                    second++;
                }
                else
                {
                    second = workers.Values.Min();
                }
            }

            return second - 1;
        }

        private static ILookup<string, string> Parse(string input)
        {
            return input.Split("\n")
                        .Select(x => Regex.Match(x, @"Step (?<P>[A-Z]) must be finished before step (?<S>[A-Z]) can begin."))
                        .Where(m => m.Success)
                        .ToLookup(m => m.Groups["S"].Value, m => m.Groups["P"].Value);
        }
    }
}
