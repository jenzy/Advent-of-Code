using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day3 : IDay
    {
        private enum Direction
        {
            Up, Left, Down, Right
        }

        /*
        37  36  35  34  33  32  31
        38  17  16  15  14  13  30
        39  18   5   4   3  12  29
        40  19   6   1   2  11  28
        41  20   7   8   9  10  27
        42  21  22  23  24  25  26
        43  44  45  46  47  48  49
        */

        public void Part1(string input)
        {
            int number = int.Parse(input);

            // Find closest (smaller) odd root
            int root = (int)Math.Floor(Math.Sqrt(number));
            root = root % 2 == 0 ? root - 1 : root;

            // Init for 1 cell right of the found root
            int n = root * root + 1;
            var coord = (x: root / 2 + 1, y: -(root / 2));
            var size = (x: root+1, y: root+1);
            var dir = Direction.Up;

            int? Move()
            {
                int d = number - n;
                switch (dir)
                {
                    case Direction.Up:
                        if (d < size.y)
                        {
                            return Math.Abs(coord.x) + Math.Abs(coord.y + d);
                        }
                        else
                        {
                            n += size.y - 1;
                            coord.y += (size.y - 1);
                            size.x++;
                            dir = Direction.Left;
                        }
                        break;

                    case Direction.Down:
                        if (d < size.y)
                        {
                            return Math.Abs(coord.x) + Math.Abs(coord.y - d);
                        }
                        else
                        {
                            n += size.y - 1;
                            coord.y -= (size.y - 1);
                            size.x++;
                            dir = Direction.Right;
                        }
                        break;

                    case Direction.Left:
                        if (d < size.x)
                        {
                            return Math.Abs(coord.x - d) + Math.Abs(coord.y);
                        }
                        else
                        {
                            n += size.x - 1;
                            coord.x -= (size.x - 1);
                            size.y++;
                            dir = Direction.Down;
                        }
                        break;

                    case Direction.Right:
                        if (d < size.x)
                        {
                            return Math.Abs(coord.x + d) + Math.Abs(coord.y);
                        }
                        else
                        {
                            n += size.x - 1;
                            coord.x += (size.x - 1);
                            size.y++;
                            dir = Direction.Up;
                        }
                        break;
                }
                return null;
            }

            int? result;
            while ((result = Move()) == null) {}

            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            int number = int.Parse(input);

            var neigbours = (from dx in Enumerable.Range(-1, 3)
                             from dy in Enumerable.Range(-1, 3)
                             where dx != 0 || dy != 0
                             select (dx: dx, dy: dy)).ToList();

            var grid = new Dictionary<(int x, int y), int> { [(0, 0)] = 1 };
            int GetGridSum((int x, int y) c) => neigbours.Sum(n => grid.TryGetValue((c.x + n.dx, c.y + n.dy), out var num) ? num : 0);

            int s = 2;
            int curS = 1;
            var coord = (x: 0, y: 0);
            var dir = Direction.Right;

            (int x, int y) Move()
            {
                curS++;
                switch (dir)
                {
                    case Direction.Right:
                        return (coord.x + 1, coord.y);
                    case Direction.Up:
                        return (coord.x, coord.y + 1);
                    case Direction.Left:
                        return (coord.x - 1, coord.y);
                    case Direction.Down:
                        return (coord.x, coord.y - 1);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dir));
                }
            }

            int result;
            while (true)
            {
                coord = Move();

                grid[coord] = GetGridSum(coord);
                if (grid[coord] > number)
                {
                    result = grid[coord];
                    break;
                }

                if (curS == s)
                {
                    curS = 1;
                    switch (dir)
                    {
                        case Direction.Right:
                            dir = Direction.Up;
                            break;

                        case Direction.Up:
                            dir = Direction.Left;
                            s++;
                            break;

                        case Direction.Left:
                            dir = Direction.Down;
                            break;

                        case Direction.Down:
                            dir = Direction.Right;
                            s++;
                            break;
                    }
                }
            }

            Console.WriteLine("Result: " + result);
        }
    }
}
