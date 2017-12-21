using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day21 : IDay
    {
        public void Part1(string input)
        {
//            input = @"../.# => ##./#../...
//.#./..#/### => #..#/..../..../#..#";

            var patterns = Parse(input);

            var grid = new[]
            {
                ".#.".ToCharArray(),
                "..#".ToCharArray(),
                "###".ToCharArray(),
            };

            for (int iteration = 0; iteration < 18; iteration++)
            {
                int breakSize = grid.Length % 2 == 0 ? 2 : 3;
                int newSize = grid.Length % 2 == 0 ? 3 : 4;
                int newGridSize = grid.Length / breakSize * newSize;

                var newGrid = new char[newGridSize][];
                for (int i = 0; i < newGridSize; i++)
                    newGrid[i] = new char[newGridSize];

                for (int row = 0, nrow = 0; row < grid.Length; row += breakSize, nrow += newSize)
                {
                    for (int col = 0, ncol = 0; col < grid.Length; col += breakSize, ncol += newSize)
                    {
                        string pattern = GetPattern(grid, row, col, breakSize);
                        var replacement = patterns[pattern].Split('/');

                        for (int i = 0; i < newSize; i++)
                        {
                            for (int j = 0; j < newSize; j++)
                            {
                                newGrid[nrow + i][ncol + j] = replacement[i][j];
                            }
                        }
                    }
                }

                grid = newGrid;
            }

            var result = grid.SelectMany(g => g).Count(g => g == '#');

            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            //int sum = ParseInput(input)
            //    .Select(row => (from a in row
            //                    from b in row
            //                    where a > b && a % b == 0
            //                    select a / b).First())
            //    .Sum();

            //Console.WriteLine("Result: " + sum);
        }

        private static Dictionary<string, string> Parse(string input)
        {
            var patterns = new Dictionary<string, string>();

            foreach (string line in input.Split('\n').Select(x => x.TrimEnd()))
            {
                var spl = line.Split(" => ");
                var pat = spl[0].Split('/');

                patterns[JoinPattern(pat)] = spl[1];

                var patFlipH = pat.Select(s => s.Reverse());
                patterns[JoinPattern(patFlipH)] = spl[1];

                var p1 = Rotate90(patFlipH);
                patterns[JoinPattern(p1)] = spl[1];

                var p2 = Rotate90(p1);
                patterns[JoinPattern(p2)] = spl[1];

                var p3 = Rotate90(p2);
                patterns[JoinPattern(p3)] = spl[1];

                var patFlipV = pat.Reverse();
                patterns[JoinPattern(patFlipV)] = spl[1];

                var patFlipHV = pat.Reverse().Select(s => s.Reverse());
                patterns[JoinPattern(patFlipHV)] = spl[1];

                var patRot90 = Rotate90(pat);
                patterns[JoinPattern(patRot90)] = spl[1];

                var patRot180 = Rotate90(patRot90);
                patterns[JoinPattern(patRot180)] = spl[1];

                var patRot270 = Rotate90(patRot180);
                patterns[JoinPattern(patRot270)] = spl[1];
            }

            return patterns;
        }

        private static List<string> Rotate90(IEnumerable<IEnumerable<char>> pat)
        {
            var gr = pat.Select(p => p.ToList()).ToList();
            var result = new List<string>();

            for (int i = 0; i < gr.Count; i++)
            {
                result.Add("");
                for (int j = 0; j < gr.Count; j++)
                {
                    result[i] += gr[gr.Count - j - 1][i];
                }
            }

            return result;
        }

        private static string JoinPattern(IEnumerable<IEnumerable<char>> pat) => string.Join("/", pat.Select(p => string.Join("", p)));

        private static string GetPattern(char[][] grid, int row, int col, int size)
        {
            var result = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result += grid[row + i][col + j];
                }
                if (i != size-1)
                    result += "/";
            }

            return result;
        }

        private static List<List<int>> ParseInput(string input) =>
            input.Split('\n')
                 .Select(l => l.Split('\t')
                               .Select(int.Parse)
                               .ToList())
                 .ToList();
    }
}
