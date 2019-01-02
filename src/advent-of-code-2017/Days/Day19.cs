using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day19 : IDay
    {
        private enum Direction { Up, Down, Left, Right }

        private string resultPart1 = "";
        private int resultPart2;

        public void Part1(string input)
        {
            Solve(input);
            Console.WriteLine("Result: " + resultPart1);
        }

        public void Part2(string input)
        {
            Console.WriteLine("Result: " + (resultPart2+1));
        }

        private void Solve(string input)
        {
            var grid = input.Split('\n').Select(x => x.TrimEnd('\r')).ToArray();
            var coord = (x: grid[0].IndexOf('|'), y: 0);
            var direction = Direction.Down;

            bool ended = false;
            while (!ended)
                ended = Move();

            bool Move()
            {
                var nc = StepInDirection(coord, direction);
                var nextChar = nc.y < 0 || nc.y >= grid.Length || nc.x < 0 || nc.x >= grid[nc.y].Length ? ' ' : grid[nc.y][nc.x];

                if (nextChar == ' ')
                {
                    if (grid[coord.y][coord.x] != '+')
                        return true;

                    foreach (var dir in GetPossibleDirections(direction))
                    {
                        var possibleCoord = StepInDirection(coord, dir);
                        if (grid[possibleCoord.y][possibleCoord.x] != ' ')
                        {
                            direction = dir;
                            return false;
                        }
                    }

                    throw new Exception("what");
                }

                coord = nc;
                resultPart2++;

                if (nextChar != '|' && nextChar != '-' && nextChar != '+')
                    resultPart1 += nextChar;

                return false;
            }
        }

        private static IEnumerable<Direction> GetPossibleDirections(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                case Direction.Down:
                    yield return Direction.Left;
                    yield return Direction.Right;
                    break;

                case Direction.Left:
                case Direction.Right:
                    yield return Direction.Up;
                    yield return Direction.Down;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static (int x, int y) StepInDirection((int x, int y) coord, Direction dir)
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
    }
}
