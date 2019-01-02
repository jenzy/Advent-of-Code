using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day14 : IDay
    {
        public void Part1(string input)
        {
            int result = 0;
            for (int row = 0; row < 128; row++)
            {
                string hash = Day10.KnotHash($"{input}-{row}");
                for (int i = 0; i < hash.Length; i += 2)
                    result += CountBits(int.Parse(hash.Substring(i, 2), NumberStyles.HexNumber));
            }

            Console.WriteLine("Result: " + result);
        }

        private static int CountBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        public void Part2(string input)
        {
            var grid = new List<List<bool>>();
            for (int row = 0; row < 128; row++)
            {
                var hash = Day10.KnotHash($"{input}-{row}");
                grid.Add(new List<bool>());
                for (int i = 0; i < hash.Length; i += 2)
                {
                    var b = byte.Parse(hash.Substring(i, 2), NumberStyles.HexNumber);
                    grid[row].AddRange(new BitArray(new []{b}).Cast<bool>().Reverse());
                }
            }

            int nGroups = 0;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (!grid[i][j])
                        continue;
                    nGroups++;
                    Dfs(i, j);
                }
            }

            void Dfs(int i, int j)
            {
                if (!grid[i][j])
                    return;

                grid[i][j] = false;

                if (i > 0)
                    Dfs(i - 1, j);
                if (j > 0)
                    Dfs(i, j - 1);
                if (i < grid.Count - 1)
                    Dfs(i + 1, j);
                if (j < grid[i].Count - 1)
                    Dfs(i, j + 1);
            }

            Console.WriteLine("Result: " + (nGroups));
        }
    }
}
