using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day22 : IDay
    {
        public void Part1(string input) => Run(input, 10_000, state => state == 0 ? 2 : 0);

        public void Part2(string input) => Run(input, 10_000_000, state => (state + 1) % 4);

        private static void Run(string input, int bursts, Func<int, int> nextState)
        {
            var grid = Parse(input);
            int direction = 0, result = 0;
            var coord = (x: 0, y: 0);

            int GetState((int x, int y) c) => grid.TryGetValue(c, out int val) ? val : 0;

            for (int burst = 0; burst < bursts; burst++)
            {
                direction = GetDirection(GetState(coord), direction);
                grid[coord] = nextState(GetState(coord));
                if (grid[coord] == 2)
                    result++;
                coord = Move(coord, direction);
            }

            Console.WriteLine("Result: " + result);
        }

        private static int GetDirection(int state, int direction)
        {
            // state: 0 clean, 1 weakened, 2 infected, 3 flagged
            switch (state)
            {
                case 0: return (direction + 3) % 4;
                case 2: return (direction + 1) % 4;
                case 3: return (direction + 2) % 4;
                default: return direction;
            }
        }
        private static (int x, int y) Move((int x, int y) c, int dir)
        {
            // direction: 0 up, 1 right, 2 down, 3 left
            switch (dir)
            {
                case 0: return (c.x, c.y - 1);
                case 1: return (c.x + 1, c.y);
                case 2: return (c.x, c.y + 1);
                case 3: return (c.x - 1, c.y);
                default: return c;
            }
        }

        private static Dictionary<(int x, int y), int> Parse(string input)
        {
            var rows = input.Split('\n').Select(x => x.Trim()).ToList();
            int mid = rows.Count / 2;
            var grid = new Dictionary<(int x, int y), int>();

            for (int i = 0; i < rows.Count; i++)
                for (int j = 0; j < rows[i].Length; j++)
                    grid[(j - mid, i - mid)] = rows[i][j] == '#' ? 2 : 0;

            return grid;
        }
    }
}
