using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days
{
    /*

     */
    internal class Day03 : IDay
    {
        public string Part1(string input)
        {
            var list = Parse(input);

            var claimOverlaps = new Dictionary<(int x, int y), List<int>>();

            foreach (var claim in list)
            {
                for (int x = claim.X; x < claim.RightEdge; ++x)
                {
                    for (int y = claim.Y; y < claim.BottomEdge; ++y)
                    {
                        if (!claimOverlaps.TryGetValue((x, y), out var co))
                            claimOverlaps[(x, y)] = co = new List<int>();

                        co.Add(claim.Id);
                    }
                }
            }

            return claimOverlaps.Count(c => c.Value.Count > 1).ToString();
        }

        public string Part2(string input)
        {
            Console.WriteLine("TODO");
            return null;
        }

        private static List<Rectangle> Parse(string input)
        {
            return input.Split("\n")
                        .Select(Rectangle.ParseLine)
                        .ToList();
        }

        private class Rectangle
        {
            public int Id { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public int RightEdge => X + Width;

            public int BottomEdge => Y + Height;

            public static Rectangle ParseLine(string line)
            {
                // #1 @ 808,550: 12x22
                var spl1 = line.Split('@');
                var spl2 = spl1[1].Split(':');
                var splCoord = spl2[0].Split(',');
                var splSize = spl2[1].Split('x');

                return new Rectangle
                {
                    Id = int.Parse(spl1[0].TrimStart('#')),
                    X = int.Parse(splCoord[0].Trim()),
                    Y = int.Parse(splCoord[1].Trim()),
                    Width = int.Parse(splSize[0].Trim()),
                    Height = int.Parse(splSize[0].Trim())
                };
            }
        }
    }
}
