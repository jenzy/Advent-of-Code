using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Days
{
    /*
--- Day 6: Chronal Coordinates ---

The device on your wrist beeps several times, and once again you feel like you're falling.

"Situation critical," the device announces.
"Destination indeterminate. Chronal interference detected. Please specify new target coordinates."

The device then produces a list of coordinates (your puzzle input).
Are they places it thinks are safe or dangerous? It recommends you check manual page 729. The Elves did not give you a manual.

If they're dangerous, maybe you can minimize the danger by finding the coordinate that gives the largest distance from the other points.

Using only the Manhattan distance, determine the area around each coordinate by counting
the number of integer X,Y locations that are closest to that coordinate (and aren't tied in distance to any other coordinate).

Your goal is to find the size of the largest area that isn't infinite.
For example, consider the following list of coordinates:

1, 1
1, 6
8, 3
3, 4
5, 5
8, 9

If we name these coordinates A through F, we can draw them on a grid, putting 0,0 at the top left:

..........
.A........
..........
........C.
...D......
.....E....
.B........
..........
..........
........F.

This view is partial - the actual grid extends infinitely in all directions.
Using the Manhattan distance, each location's closest coordinate can be determined, shown here in lowercase:

aaaaa.cccc
aAaaa.cccc
aaaddecccc
aadddeccCc
..dDdeeccc
bb.deEeecc
bBb.eeee..
bbb.eeefff
bbb.eeffff
bbb.ffffFf

Locations shown as . are equally far from two or more coordinates, and so they don't count as being closest to any.

In this example, the areas of coordinates A, B, C, and F are infinite - while not shown here,
their areas extend forever outside the visible grid. However, the areas of coordinates D and E are finite: D is closest to 9 locations,
and E is closest to 17 (both including the coordinate's location itself). Therefore, in this example, the size of the largest area is 17.

What is the size of the largest area that isn't infinite?

Your puzzle answer was 4215.
--- Part Two ---

On the other hand, if the coordinates are safe, maybe the best you can do is try to find a region near as many coordinates as possible.

For example, suppose you want the sum of the Manhattan distance to all of the coordinates to be less than 32.
For each location, add up the distances to all of the given coordinates;
if the total of those distances is less than 32, that location is within the desired region.
Using the same coordinates as above, the resulting region looks like this:

..........
.A........
..........
...###..C.
..#D###...
..###E#...
.B.###....
..........
..........
........F.

In particular, consider the highlighted location 4,3 located at the top middle of the region.
Its calculation is as follows, where abs() is the absolute value function:

    Distance to coordinate A: abs(4-1) + abs(3-1) =  5
    Distance to coordinate B: abs(4-1) + abs(3-6) =  6
    Distance to coordinate C: abs(4-8) + abs(3-3) =  4
    Distance to coordinate D: abs(4-3) + abs(3-4) =  2
    Distance to coordinate E: abs(4-5) + abs(3-5) =  3
    Distance to coordinate F: abs(4-8) + abs(3-9) = 10
    Total distance: 5 + 6 + 4 + 2 + 3 + 10 = 30

Because the total distance to all coordinates (30) is less than 32, the location is within the region.

This region, which also includes coordinates D and E, has a total size of 16.

Your actual region will need to be much larger than this example, though,
instead including all locations with a total distance of less than 10000.

What is the size of the region containing all locations which have a total distance to all given coordinates of less than 10000?

     */
    internal class Day06 : IDay
    {
        public object Part1(string input)
        {
            var points = Parse(input);
            var bounds = GetBounds(points);

            var counts = new Dictionary<int, int>();
            var closestPoints = new Dictionary<(int x, int y), int>();

            foreach (var point in AllPointsInBounds(bounds))
            {
                var iMin = GetClosest(points, point);
                if (iMin != null)
                {
                    closestPoints[point] = iMin.Value;
                    if (counts.TryGetValue(iMin.Value, out int total))
                        counts[iMin.Value] = 1 + total;
                    else
                        counts[iMin.Value] = 1;
                }
            }

            void RemoveInfinitePoint((int x, int y) p)
            {
                if (closestPoints.TryGetValue(p, out int iPoint))
                    counts.Remove(iPoint);
            }

            for (int x = bounds.minX; x <= bounds.maxX; x++)
            {
                RemoveInfinitePoint((x, 0));
                RemoveInfinitePoint((x, bounds.maxY));
            }

            for (int y = 0; y <= bounds.maxY; y++)
            {
                RemoveInfinitePoint((0, y));
                RemoveInfinitePoint((bounds.maxX, y));
            }

            return counts.Values.Max();
        }

        public object Part2(string input)
        {
            var points = Parse(input);
            return AllPointsInBounds(GetBounds(points)).Count(point => points.Sum(t => Dist(point, t)) < 10000);
        }

        private static List<(int x, int y)> Parse(string input)
        {
            return input.Split("\n")
                        .Select(x => Regex.Match(x, @"(?<X>\d+),\s*(?<Y>\d+)"))
                        .Where(m => m.Success)
                        .Select(m => (x: int.Parse(m.Groups["X"].Value), y: int.Parse(m.Groups["Y"].Value)))
                        .ToList();
        }

        private static (int minX, int maxX, int minY, int maxY) GetBounds(List<(int x, int y)> points)
        {
            int minX = points.Min(p => p.x);
            int minY = points.Min(p => p.y);
            int maxX = points.Max(p => p.x);
            int maxY = points.Max(p => p.y);
            return (minX, maxX, minY, maxY);
        }

        private static IEnumerable<(int x, int y)> AllPointsInBounds((int minX, int maxX, int minY, int maxY) bounds)
        {
            return from x in Enumerable.Range(bounds.minX, bounds.maxX - bounds.minX + 1)
                   from y in Enumerable.Range(bounds.minY, bounds.maxY - bounds.minY + 1)
                   select (x, y);
        }

        private static int Dist((int x, int y) p1, (int x, int y) p2) => Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);

        private static int? GetClosest(List<(int x, int y)> points, (int x, int y) p)
        {
            int min1 = int.MaxValue;
            int? iMin = null;

            for (int i = 0; i < points.Count; i++)
            {
                var dist = Dist(points[i], p);
                if (dist < min1)
                {
                    min1 = dist;
                    iMin = i;
                }
                else if (dist == min1)
                {
                    iMin = null;
                }
            }

            return iMin;
        }
    }
}
