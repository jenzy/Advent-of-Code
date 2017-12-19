using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace AdventOfCode2017.Days
{
    internal class Day19 : IDay
    {
        enum Direction
        {
            Up, Down, Left, Right
        }

        string[] grid;
        private (int x, int y) coord;
        private Direction direction = Direction.Down;
        private string collected = "";
        private int stepCount = 0;

        public void Part1(string input)
        {
            grid = input.Split('\n').Select(x => x.TrimEnd('\r')).ToArray();
            coord.x = grid[0].IndexOf('|');

            bool ended = false;
            while (!ended)
            {
                ended = Move();
            }

            Console.WriteLine("Result: " + collected);
        }

        public void Part2(string input)
        {
            grid = input.Split('\n').Select(x => x.TrimEnd('\r')).ToArray();
            coord.x = grid[0].IndexOf('|');
            coord.y = 0;
            stepCount = 1;

            bool ended = false;
            while (!ended)
            {
                ended = Move();
            }

            Console.WriteLine("Result: " + stepCount);
        }

        private bool Move()
        {
            var nc = MoveCoords(coord, direction);
            var curChar = grid[coord.y][coord.x];

            var nextChar = nc.y < 0 || nc.y >= grid.Length || nc.x < 0 || nc.x >= grid[nc.y].Length
                               ? ' '
                               : grid[nc.y][nc.x];

            if (nextChar == ' ')
            {
                if (curChar == '+')
                {
                    var dirs = GetPossibleDirections();
                    var nc1 = MoveCoords(coord, dirs.d1);
                    if (grid[nc1.y][nc1.x] != ' ')
                    {
                        direction = dirs.d1;
                        return false;
                    }
                    var nc2 = MoveCoords(coord, dirs.d2);
                    if (grid[nc2.y][nc2.x] != ' ')
                    {
                        direction = dirs.d2;
                        return false;
                    }

                    throw new Exception("what");
                }
                else
                {
                    return true;
                }
            }
            else if (nextChar == '|' || nextChar == '-' || nextChar == '+')
            {
                coord = nc;
                stepCount++;
            }
            else
            {
                collected += nextChar;
                coord = nc;
                stepCount++;
            }

            return false;
        }

        private (Direction d1, Direction d2) GetPossibleDirections()
        {
            switch (direction)
            {
                case Direction.Up:
                case Direction.Down:
                    return (Direction.Left, Direction.Right);
                case Direction.Left:
                case Direction.Right:
                    return (Direction.Up, Direction.Down);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static (int x, int y) MoveCoords((int x, int y) coord, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return (coord.x, coord.y - 1);
                case Direction.Down:
                    return (coord.x, coord.y + 1);
                case Direction.Left:
                    return (coord.x-1, coord.y);
                case Direction.Right:
                    return (coord.x+1, coord.y);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }

        private static int Common(string input, int offset) =>
            input.Where((cur, i) => cur == input[(i + offset) % input.Length])
                 .Sum(t => (int) char.GetNumericValue(t));
    }
}
