using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day23 : IDay
    {
        public void Part1(string input)
        {
            var prog = new Day18.Prog(Day18.Parse(input).ToList(), new Dictionary<string, long>());
            prog.Run();
            Console.WriteLine("Result: " + prog.CountMul);
        }

        public void Part2(string input)
        {
            var commands = Day18.Parse(input).Take(8).ToList();
            var prog = new Day18.Prog(commands, new Dictionary<string, long> { ["a"] = 1 });
            prog.Run();

            long b = prog.GetRegister("b"), c = prog.GetRegister("c"), h = 0;

            for (int i = (int) b; i <= c; i += 17)
                if (Enumerable.Range(2, i - 2).FirstOrDefault(j => i % j == 0) != 0)
                    h++;

            Console.WriteLine("Result: " + h);
        }
    }
}
