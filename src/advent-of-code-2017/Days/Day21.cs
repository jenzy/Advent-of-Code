using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
    internal class Day21 : IDay
    {
        public void Part1(string input)
        {
            Console.WriteLine("Result: " + Run(5, input));
        }

        public void Part2(string input)
        {
            Console.WriteLine("Result: " + Run(18, input));
        }

        private static int Run(int numSteps, string input)
        {
            string GetPattern(char[,] source, int row, int col, int size)
            {
                var result = "";
                for (int i = 0; i < size; i++)
                {
                    result += "/";
                    for (int j = 0; j < size; j++)
                        result += source[row + i, col + j];
                }
                return result.Substring(1);
            }

            var patterns = Parse(input);
            var grid = new[,]
            {
                { '.', '#', '.' },
                { '.', '.', '#' },
                { '#', '#', '#' }
            };

            for (int iteration = 0; iteration < numSteps; iteration++)
            {
                int breakSize = grid.GetLength(0) % 2 == 0 ? 2 : 3;
                int newSize = breakSize + 1;
                int newGridSize = grid.GetLength(0) / breakSize * newSize;
                var newGrid = new char[newGridSize, newGridSize];

                for (int row = 0, nrow = 0; row < grid.GetLength(0); row += breakSize, nrow += newSize)
                {
                    for (int col = 0, ncol = 0; col < grid.GetLength(0); col += breakSize, ncol += newSize)
                    {
                        var replacement2 = patterns[GetPattern(grid, row, col, breakSize)].Split('/');

                        for (int i = 0; i < newSize; i++)
                            for (int j = 0; j < newSize; j++)
                                newGrid[nrow + i, ncol + j] = replacement2[i][j];
                    }
                }

                grid = newGrid;
            }

            return grid.Cast<char>().Count(c => c == '#');
        }

        private static Dictionary<string, string> Parse(string input)
        {
            string JoinPattern(IEnumerable<IEnumerable<char>> pat) => string.Join("/", pat.Select(p => string.Join("", p)));

            List<string> Rotate90(IEnumerable<IEnumerable<char>> pat)
            {
                var gr = pat.Select(p => p.ToList()).ToList();
                var result = new List<string>();

                for (int i = 0; i < gr.Count; i++)
                {
                    result.Add("");
                    for (int j = 0; j < gr.Count; j++)
                        result[i] += gr[gr.Count - j - 1][i];
                }

                return result;
            }

            var patterns = new Dictionary<string, string>();

            foreach (string line in input.Split('\n').Select(x => x.TrimEnd()))
            {
                var spl = line.Split(" => ");
                var pat = spl[0].Split('/').ToList();

                for (int i = 0; i < 4; i++)
                {
                    patterns[JoinPattern(pat)] = spl[1];
                    patterns[JoinPattern(pat.Select(s => s.Reverse()))] = spl[1];
                    patterns[JoinPattern(pat.AsEnumerable().Reverse())] = spl[1];
                    pat = Rotate90(pat);
                }
            }

            return patterns;
        }
    }
}
