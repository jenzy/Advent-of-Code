using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day16 : IDay
    {
        public void Part1(string input)
        {
            var list = "abcdefghijklmnop".ToList();
            list = input.Split(',').Aggregate(list, Dance);
            Console.WriteLine("Result: " + string.Concat(list));
        }

        public void Part2(string input)
        {
            var list = "abcdefghijklmnop".ToList();
            var cycles = new OrderedDictionary { { "abcdefghijklmnop", "abcdefghijklmnop" } };

            for (int i = 0; i < 1_000_000_000; i++)
            {
                list = input.Split(',').Aggregate(list, Dance);
                var str = string.Concat(list);
                if (cycles.Contains(str))
                    break;
                cycles.Add(str, str);
            }

            Console.WriteLine("Result: " + cycles[1_000_000_000 % cycles.Count]);
        }

        private static List<char> Dance(List<char> list, string input)
        {
            if (input.StartsWith("s"))
            {
                int n = int.Parse(input.Substring(1));
                return list.Skip(list.Count - n).Concat(list.Take(list.Count - n)).ToList();
            }
            if (input.StartsWith("x"))
            {
                int iSlash = input.IndexOf('/');
                return Swap(list, int.Parse(input.Substring(1, iSlash - 1)), int.Parse(input.Substring(iSlash + 1)));
            }
            if (input.StartsWith("p"))
            {
                int iSlash = input.IndexOf('/');
                return Swap(list, list.IndexOf(input[1]), list.IndexOf(input[iSlash + 1]));
            }

            return null;
        }

        private static List<T> Swap<T>(List<T> list, int i, int j)
        {
            var t = list[i];
            list[i] = list[j];
            list[j] = t;
            return list;
        }
    }
}
