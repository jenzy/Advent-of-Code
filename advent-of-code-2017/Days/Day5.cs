using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day5 : IDay
    {
        public void Part1(string input)
        {
            var list = input.Split('\n').Select(x => int.Parse(x.Trim())).ToList();

            int i = 0, result = 0;
            while (true)
            {
                var newI = i + list[i];
                if (newI < 0 || newI >= list.Count)
                    break;

                list[i]++;
                i = newI;
                result++;
            }
            

            Console.WriteLine("Result: " + (result + 1));
        }

        public void Part2(string input)
        {
            var list = input.Split('\n').Select(x => int.Parse(x.Trim())).ToList();

            int i = 0, result = 0;
            while (true)
            {
                var newI = i + list[i];
                if (newI < 0 || newI >= list.Count)
                    break;

                if (list[i] >= 3)
                    list[i]--;
                else
                    list[i]++;

                i = newI;
                result++;
            }

            Console.WriteLine("Result: " + (result + 1));
        }
    }
}
