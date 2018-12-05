using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days
{
    /*
--- Day 3: No Matter How You Slice It ---

The Elves managed to locate the chimney-squeeze prototype fabric for Santa's suit
(thanks to someone who helpfully wrote its box IDs on the wall of the warehouse in the middle of the night).
Unfortunately, anomalies are still affecting them - nobody can even agree on how to cut the fabric.

The whole piece of fabric they're working on is a very large square - at least 1000 inches on each side.

Each Elf has made a claim about which area of fabric would be ideal for Santa's suit.
All claims have an ID and consist of a single rectangle with edges parallel to the edges of the fabric.
Each claim's rectangle is defined as follows:

    The number of inches between the left edge of the fabric and the left edge of the rectangle.
    The number of inches between the top edge of the fabric and the top edge of the rectangle.
    The width of the rectangle in inches.
    The height of the rectangle in inches.

A claim like #123 @ 3,2: 5x4 means that claim ID 123 specifies a rectangle 3 inches from the left edge,
2 inches from the top edge, 5 inches wide, and 4 inches tall.
Visually, it claims the square inches of fabric
represented by # (and ignores the square inches of fabric represented by .) in the diagram below:

...........
...........
...#####...
...#####...
...#####...
...#####...
...........
...........
...........

The problem is that many of the claims overlap,
causing two or more claims to cover part of the same areas.
For example, consider the following claims:

#1 @ 1,3: 4x4
#2 @ 3,1: 4x4
#3 @ 5,5: 2x2

Visually, these claim the following areas:

........
...2222.
...2222.
.11XX22.
.11XX22.
.111133.
.111133.
........

The four square inches marked with X are claimed by both 1 and 2.
(Claim 3, while adjacent to the others, does not overlap either of them.)

If the Elves all proceed with their own plans, none of them will have enough fabric.
How many square inches of fabric are within two or more claims?

--- Part Two ---

Amidst the chaos, you notice that exactly one claim doesn't overlap by even a single square inch of fabric with any other claim.
If you can somehow draw attention to it, maybe the Elves will be able to make Santa's suit after all!

For example, in the claims above, only claim 3 is intact after all claims are made.

What is the ID of the only claim that doesn't overlap?

     */

    internal class Day03 : IDay
    {
        public object Part1(string input) => GetOvelaps(Parse(input)).Values.Count(c => c.Count > 1);

        public object Part2(string input)
        {
            var claims = Parse(input);
            GetOvelaps(claims);
            return claims.Single(c => !c.Overlaps).Id;
        }

        private static List<Claim> Parse(string input) => input.Split("\n").Select(Claim.ParseLine).ToList();

        private static Dictionary<(int x, int y), List<Claim>> GetOvelaps(List<Claim> claims)
        {
            var overlaps = new Dictionary<(int x, int y), List<Claim>>();

            foreach (var ct in claims.SelectMany(c => c.Tiles, (claim, tile) => (claim, tile)))
            {
                if (overlaps.TryGetValue(ct.tile, out var overlapList))
                {
                    overlapList.Add(ct.claim);
                    foreach (var c in overlapList)
                        c.Overlaps = true;
                }
                else
                {
                    overlaps[ct.tile] = new List<Claim> { ct.claim };
                }
            }

            return overlaps;
        }

        private class Claim
        {
            public bool Overlaps { get; set; }

            public string Id { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public IEnumerable<(int x, int y)> Tiles => from x in Enumerable.Range(X, Width)
                                                        from y in Enumerable.Range(Y, Height)
                                                        select (x, y);

            public static Claim ParseLine(string line)
            {
                // #1 @ 808,550: 12x22
                var spl1 = line.Split('@');
                var spl2 = spl1[1].Split(':');
                var splCoord = spl2[0].Split(',');
                var splSize = spl2[1].Split('x');

                return new Claim
                {
                    Id = spl1[0].TrimStart('#'),
                    X = int.Parse(splCoord[0].Trim()),
                    Y = int.Parse(splCoord[1].Trim()),
                    Width = int.Parse(splSize[0].Trim()),
                    Height = int.Parse(splSize[1].Trim())
                };
            }
        }
    }
}
