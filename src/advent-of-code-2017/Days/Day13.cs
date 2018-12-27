using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day13 : IDay
    {
        public void Part1(string input)
        {
            var scanners = input.Split('\n')
                                .Select(l => l.Split(": "))
                                .Select(s => new Scanner { Position = int.Parse(s[0]), Depth = int.Parse(s[1]) });

            int result = scanners.Where(sc => sc.Get() == 0).Sum(sc => sc.Position * sc.Depth);

            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            var scanners = input.Split('\n')
                                .Select(l => l.Split(": "))
                                .Select(s => new Scanner { Position = int.Parse(s[0]), Depth = int.Parse(s[1]) })
                                .ToList();

            int delay = 0;
            while (true)
            {
                bool caught = scanners.Any(scanner => scanner.Get(delay) == 0);
                if (caught)
                    delay++;
                else
                    break;
            }

            Console.WriteLine("Result: " + delay);
        }

        private class Scanner
        {
            public int Depth { get; set; }

            public int Position { get; set; }

            public int Get(int delay = 0) => (Position + delay) % (2 * Depth - 2);
        }
    }
}
