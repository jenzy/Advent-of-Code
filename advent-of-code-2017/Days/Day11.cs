using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day11 : IDay
    {
        public void Part1(string input)
        {
            var directions = input.Split(",").Select(s => s.Trim()).ToList();

            int x = 0, y = 0;

            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case "n":
                        y++;
                        break;

                    case "s":
                        y--;
                        break;

                    case "ne":
                        x++;
                        break;

                    case "sw":
                        x--;
                        break;

                    case "se":
                        x++;
                        y--;
                        break;

                    case "nw":
                        x--;
                        y++;
                        break;
                }
            }

            var dx = x - 0;
            var dy = y - 0;

            int dist = 0;
            if (Math.Sign(dx) == Math.Sign(dy))
                dist = Math.Abs(dx + dy);
            else
                dist = Math.Max(Math.Abs(dx), Math.Abs(dy));

            Console.WriteLine("Result: " + dist);
        }

        public void Part2(string input)
        {
            var directions = input.Split(",").Select(s => s.Trim()).ToList();

            int x = 0, y = 0, dist = 0;

            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case "n":
                        y++;
                        break;

                    case "s":
                        y--;
                        break;

                    case "ne":
                        x++;
                        break;

                    case "sw":
                        x--;
                        break;

                    case "se":
                        x++;
                        y--;
                        break;

                    case "nw":
                        x--;
                        y++;
                        break;
                }

                dist = Math.Max(dist, distance(x, y));
            }

            Console.WriteLine("Result: " + dist);
        }

        public static int distance(int x, int y)
        {
            int dist = 0;
            if (Math.Sign(x) == Math.Sign(y))
                dist = Math.Abs(x + y);
            else
                dist = Math.Max(Math.Abs(x), Math.Abs(y));
            return dist;
        }
    }
}
