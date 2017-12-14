using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AdventOfCode2017.Days
{
    internal class Day14 : IDay
    {
        public void Part1(string input)
        {
            int result = 0;
            for (int row = 0; row < 128; row++)
            {
                var hash = KnotHash($"{input}-{row}");
                for (int i = 0; i < hash.Length; i += 2)
                {
                    var aa = int.Parse(hash.Substring(i, 2), NumberStyles.HexNumber);
                    result += NumberOfSetBits(aa);
                }
            }

            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            input = "flqrgnkx";
            var grid = new List<List<bool>>();
            for (int row = 0; row < 128; row++)
            {
                var hash = KnotHash($"{input}-{row}");
                var ll = new List<bool>();
                for (int i = 0; i < hash.Length; i += 2)
                {
                    var aa = byte.Parse(hash.Substring(i, 2), NumberStyles.HexNumber);
                    var b = new BitArray(new []{aa}).Cast<bool>().Select(b1 => !b1).ToList();
                    if (b.Count != 8)
                        throw new Exception();

                    ll.AddRange(b);
                }
                grid.Add(ll);
            }

            int result = 0;
            var seen = new HashSet<(int, int)>();
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    if (seen.Contains((i, j)))
                        continue;
                    if (!grid[i][j])
                        continue;

                    result++;
                    dfs(i, j);
                }
            }

            void dfs(int i, int j)
            {
                if (seen.Contains((i,j)))
                    return;
                if (!grid[i][j])
                    return;

                seen.Add((i, j));

                if (i > 0)
                    dfs(i - 1, j);
                if (j > 0)
                    dfs(i, j - 1);
                if (i < 127)
                    dfs(i + 1, j);
                if (j < 127)
                    dfs(i, j + 1);
            }

            Console.WriteLine("Result: " + (result));
        }

        int NumberOfSetBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        public string KnotHash(string input)
        {
            var list = new CircularList();
            var lengths = Encoding.ASCII.GetBytes(input).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToList();

            int iCur = 0, skip = 0;
            for (int round = 0; round < 64; round++)
            {
                foreach (int length in lengths)
                {
                    var tmpList = list.GetRange(iCur, length).Reverse().ToList();
                    for (int l = 0; l < length; l++)
                        list[iCur + l] = tmpList[l];

                    iCur += length + skip;
                    skip++;
                }
            }

            var hash = "";
            for (int i = 0; i < 16; i++)
                hash += list.GetRange(i * 16, 16).Aggregate(0, (a, b) => a ^ b).ToString("x2");

            return hash;
        }

        private class CircularList
        {
            private List<int> List { get; } = Enumerable.Range(0, 256).ToList();

            public int this[int i]
            {
                get => List[i % List.Count];
                set => List[i % List.Count] = value;
            }

            public IEnumerable<int> GetRange(int iStart, int length)
            {
                for (int i = 0; i < length; i++)
                    yield return this[iStart + i];
            }
        }
    }
}
