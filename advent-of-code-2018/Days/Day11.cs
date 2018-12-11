using System;

namespace AdventOfCode2018.Days
{
    /*

     */
    internal class Day11 : IDay
    {
        private int serial;

        public object Part1(string input)
        {
            serial = int.Parse(input);

            var grid = new int[301, 301];
            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                    grid[x, y] = PowerLevel(x, y);
            }

            var maxes = new int[301, 301];
            int max = int.MinValue;
            var maxCord = (x: 0, y: 0);
            for (int x = 1; x <= 300-2; x++)
            {
                for (int y = 1; y <= 300-2; y++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            maxes[x, y] += grid[x + i, y + j];
                        }
                    }

                    if (maxes[x, y] > max)
                    {
                        max = maxes[x, y];
                        maxCord = (x, y);
                    }
                }
            }
            return maxCord;
        }

        public object Part2(string input)
        {
            serial = int.Parse(input);

            var grid = new int[301, 301];
            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                    grid[x, y] = PowerLevel(x, y);
            }

            var maxes = new int[301, 301];
            int max = int.MinValue;
            int maxSize = 0;
            var maxCord = (x: 0, y: 0);
            for (int x = 1; x <= 300-2; x++)
            {
                for (int y = 1; y <= 300-2; y++)
                {
                    for (int size = 3; size <= 300 - Math.Max(x, y); size++)
                    {
                        int mm = 0;
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                mm += grid[x + i, y + j];
                            }
                        }

                        if (mm > max)
                        {
                            max = mm;
                            maxCord = (x, y);
                            maxSize = size;
                        }
                    }
                }
            }

            return (maxCord.x, maxCord.y, maxSize);
        }

        private int PowerLevel(int x, int y)
        {
            int rackId = x + 10;
            var pl = (rackId*y+serial)*rackId;
            return (pl % 1000 - pl % 100) / 100 - 5;
        }
    }
}
