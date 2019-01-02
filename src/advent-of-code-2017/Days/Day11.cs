using System;
using System.Linq;
using static System.Math;

namespace AdventOfCode.Y2017.Days
{
    internal class Day11 : IDay
    {
        public void Part1(string input)
        {
            var directions = input.Split(",").Select(s => s.Trim()).ToList();

            var coord = directions.Aggregate((x: 0, y: 0), Move);

            Console.WriteLine("Result: " + Distance(coord));
        }

        public void Part2(string input)
        {
            var directions = input.Split(",").Select(s => s.Trim()).ToList();

            int dist = 0;
            var coord = (x:0, y:0);

            foreach (var direction in directions)
            {
                coord = Move(coord, direction);
                dist = Max(dist, Distance(coord));
            }

            Console.WriteLine("Result: " + dist);
        }

        private static (int x, int y) Move((int x, int y) coord, string direction)
        {
            switch (direction)
            {
                case "n":
                    return (coord.x, coord.y + 1);
                case "s":
                    return (coord.x, coord.y - 1);
                case "ne":
                    return (coord.x + 1, coord.y);
                case "sw":
                    return (coord.x - 1, coord.y);
                case "se":
                    return (coord.x + 1, coord.y - 1);
                case "nw":
                    return (coord.x - 1, coord.y + 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }

        private static int Distance((int x, int y) coord) =>
            Sign(coord.x) == Sign(coord.y) 
            ? Abs(coord.x + coord.y) 
            : Max(Abs(coord.x), Abs(coord.y));
    }
}
