using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day11 : DayBase
    {
        public override object Part1()
        {
            var asteroids = Parse(Input);
            return asteroids.Max(a => CountDistinctAngles(asteroids, a));
        }

        public override object Part2()
        {
            var asteroids = Parse(Input);
            var laser = asteroids.MaxBy(a => CountDistinctAngles(asteroids, a));

            var directions = new Dictionary<double, LinkedList<(int dist2, int x, int y)>>();
            foreach (var other in asteroids.Where(x => x != laser))
            {
                int dx = other.x - laser.x, dy = -other.y + laser.y;
                var angle = GetNormalizedAngle(dx, dy);
                if (!directions.TryGetValue(angle, out var list))
                    directions[angle] = list = new LinkedList<(int, int, int)>();
                list.AddFirst((dx * dx + dy * dy, other.x, other.y));
            }

            var sortedDirections = directions.OrderBy(x => x.Key)
                                             .Select(x => x.Value.OrderBy(d => d.dist2).ToLinkedList());

            int i = 0;
            foreach (var candidates in sortedDirections.RepeatForever().Where(x => x.Count > 0))
            {
                var removed = candidates.First();
                candidates.RemoveFirst();
                if (++i == 200)
                    return removed.x * 100 + removed.y;
            }

            return -1;
        }

        private static double GetNormalizedAngle(int dx, int dy) => (Math.Atan2(dx, dy) + (2 * Math.PI)) % (2 * Math.PI);

        private static int CountDistinctAngles(IEnumerable<(int x, int y)> asteroids, (int x, int y) asteroid)
        {
            return asteroids.Where(other => other != asteroid)
                            .Select(other => GetNormalizedAngle(other.x - asteroid.x, -other.y + asteroid.y))
                            .Distinct()
                            .Count();
        }

        private static HashSet<(int x, int y)> Parse(string input)
        {
            return input.Split('\n')
                        .Select((row, y) => row.Select((c, x) => (x, y, c))
                                               .Where(x => x.c == '#')
                                               .Select(x => (x.x, x.y)))
                        .SelectMany(x => x)
                        .ToHashSet();
        }

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(11);
            Assert.Equal(340, day.Part1());
            Assert.Equal(2628, day.Part2());
        }
    }
}

