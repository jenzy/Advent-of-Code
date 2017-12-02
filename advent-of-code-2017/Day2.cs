using System;
using System.Linq;

namespace AdventOfCode2017
{
    internal class Day2
    {
        public void Part1(string input)
        {
            var data = input.Split('\n')
                            .Select(l => l.Split('\t')
                                          .Select(int.Parse)
                                          .ToList())
                                          .ToList();

            int sum = 0;
            foreach(var line in data)
            {
                int min = int.MaxValue, max = int.MinValue;
                foreach (var val in line)
                {
                    min = Math.Min(min, val);
                    max = Math.Max(max, val);
                }
                sum += max - min;
            }

            Console.WriteLine("Result: " + sum);
        }

        public void Part2(string input)
        {
            var data = input.Split('\n')
                            .Select(l => l.Split('\t')
                                          .Select(int.Parse)
                                          .ToList())
                                          .ToList();

            int sum = 0;
            foreach (var line in data)
            {
                bool found = false;
                for (int i = 0; i < line.Count && !found; i++)
                {
                    for (int j = i + 1; j < line.Count && !found; j++)
                    {
                        int max = Math.Max(line[i], line[j]);
                        int min = Math.Min(line[i], line[j]);
                        if (max % min == 0)
                        {
                            sum += (max / min);
                            found = true;
                        }
                    }
                }
            }

            Console.WriteLine("Result: " + sum);
        }
    }
}
