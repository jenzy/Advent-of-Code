using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Y2018.Days
{
    /*
--- Day 17: Reservoir Research ---

You arrive in the year 18. If it weren't for the coat you got in 1018,
you would be very cold: the North Pole base hasn't even been constructed.

Rather, it hasn't been constructed yet. The Elves are making a little progress
but there's not a lot of liquid water in this climate, so they're getting very dehydrated.
Maybe there's more underground?

You scan a two-dimensional vertical slice of the ground nearby and discover that it is mostly sand with veins of clay.
The scan only provides data with a granularity of square meters,
but it should be good enough to determine how much water is trapped there. In the scan, x represents the distance to the right,
and y represents the distance down. There is also a spring of water near the surface at x=500, y=0.
The scan identifies which square meters are clay (your puzzle input).

For example, suppose your scan shows the following veins of clay:

x=495, y=2..7
y=7, x=495..501
x=501, y=3..7
x=498, y=2..4
x=506, y=1..2
x=498, y=10..13
x=504, y=10..13
y=13, x=498..504

Rendering clay as #, sand as ., and the water spring as +,
and with x increasing to the right and y increasing downward, this becomes:

   44444455555555
   99999900000000
   45678901234567
 0 ......+.......
 1 ............#.
 2 .#..#.......#.
 3 .#..#..#......
 4 .#..#..#......
 5 .#.....#......
 6 .#.....#......
 7 .#######......
 8 ..............
 9 ..............
10 ....#.....#...
11 ....#.....#...
12 ....#.....#...
13 ....#######...

The spring of water will produce water forever. Water can move through sand, but is blocked by clay.
Water always moves down when possible, and spreads to the left and right otherwise,
filling space that has clay on both sides and falling out otherwise.

For example, if five squares of water are created, they will flow downward until they reach the clay and settle there.
Water that has come to rest is shown here as ~, while sand through which water has passed (but which is now dry again) is shown as |:

......+.......
......|.....#.
.#..#.|.....#.
.#..#.|#......
.#..#.|#......
.#....|#......
.#~~~~~#......
.#######......
..............
..............
....#.....#...
....#.....#...
....#.....#...
....#######...

Two squares of water can't occupy the same location. If another five squares of water are created,
they will settle on the first five, filling the clay reservoir a little more:

......+.......
......|.....#.
.#..#.|.....#.
.#..#.|#......
.#..#.|#......
.#~~~~~#......
.#~~~~~#......
.#######......
..............
..............
....#.....#...
....#.....#...
....#.....#...
....#######...

Water pressure does not apply in this scenario. If another four squares of water are created,
they will stay on the right side of the barrier, and no water will reach the left side:

......+.......
......|.....#.
.#..#.|.....#.
.#..#~~#......
.#..#~~#......
.#~~~~~#......
.#~~~~~#......
.#######......
..............
..............
....#.....#...
....#.....#...
....#.....#...
....#######...

At this point, the top reservoir overflows. While water can reach the tiles above the surface of the water,
it cannot settle there, and so the next five squares of water settle like this:

......+.......
......|.....#.
.#..#||||...#.
.#..#~~#|.....
.#..#~~#|.....
.#~~~~~#|.....
.#~~~~~#|.....
.#######|.....
........|.....
........|.....
....#...|.#...
....#...|.#...
....#~~~~~#...
....#######...

Note especially the leftmost |: the new squares of water can reach this tile, but cannot stop there.
Instead, eventually, they all fall to the right and settle in the reservoir below.

After 10 more squares of water, the bottom reservoir is also full:

......+.......
......|.....#.
.#..#||||...#.
.#..#~~#|.....
.#..#~~#|.....
.#~~~~~#|.....
.#~~~~~#|.....
.#######|.....
........|.....
........|.....
....#~~~~~#...
....#~~~~~#...
....#~~~~~#...
....#######...

Finally, while there is nowhere left for the water to settle,
it can reach a few more tiles before overflowing beyond the bottom of the scanned data:

......+.......    (line not counted: above minimum y value)
......|.....#.
.#..#||||...#.
.#..#~~#|.....
.#..#~~#|.....
.#~~~~~#|.....
.#~~~~~#|.....
.#######|.....
........|.....
...|||||||||..
...|#~~~~~#|..
...|#~~~~~#|..
...|#~~~~~#|..
...|#######|..
...|.......|..    (line not counted: below maximum y value)
...|.......|..    (line not counted: below maximum y value)
...|.......|..    (line not counted: below maximum y value)

How many tiles can be reached by the water? To prevent counting forever,
ignore tiles with a y coordinate smaller than the smallest y coordinate in your scan data or larger than the largest one.
Any x coordinate is valid. In this example, the lowest y coordinate given is 1, and the highest is 13,
causing the water spring (in row 0) and the water falling off the bottom of the render (in rows 14 through infinity) to be ignored.

So, in the example above, counting both water at rest (~) and other sand tiles the water can hypothetically reach (|),
the total number of tiles the water can reach is 57.

How many tiles can the water reach within the range of y values in your scan?

--- Part Two ---

After a very long time, the water spring will run dry. How much water will be retained?

In the example above, water that won't eventually drain out is shown as ~, a total of 29 tiles.

How many water tiles are left after the water spring stops producing water and all remaining water not at rest has drained?

     */
    internal class Day17 : DayBase
    {
        public override object Part1() => Solve().CountPart1();

        public override object Part2() => Solve().CountPart2();

        private Map Solve()
        {
            var map = Parse();

            var stack = new Stack<(int x, int y)>();
            stack.Push((500, 0));

            while (stack.Any())
            {
                var coord = stack.Pop();
                if (coord.y > map.MaxY)
                    continue;

                var cur = map[coord];
                if (cur == '#' || cur == '~')
                    continue;

                map[coord] = '|';
                var coordBelow = OneDown(coord);
                var below = map[coordBelow];

                if (below == '#' || below == '~')
                {
                    var left = OneLeft(coord);
                    var right = OneRight(coord);

                    if (map[left] == '#')
                    {
                        if (!map.Fill(coord, left: false))
                            stack.Push(right);
                    }
                    else if (map[right] == '#')
                    {
                        if (!map.Fill(coord, left: true))
                            stack.Push(left);
                    }
                    else
                    {
                        if (map[right] == '.')
                            stack.Push(right);
                        if (map[left] == '.')
                            stack.Push(left);
                    }
                }
                else if (below == '.' && coordBelow.y <= map.MaxY)
                {
                    stack.Push(coord);
                    stack.Push(coordBelow);
                }
            }

            return map;
        }

        private Map Parse()
        {
            var map = new Dictionary<(int x, int y), char>();
            var regex1 = new Regex(@"x=(?<X>\d+), y=(?<Y1>\d+)..(?<Y2>\d+)");
            var regex2 = new Regex(@"y=(?<Y>\d+), x=(?<X1>\d+)..(?<X2>\d+)");

            foreach (string line in Input.Split("\n"))
            {
                if (regex1.TryMatch(line, out var m1))
                {
                    int x = int.Parse(m1.Get("X")), y1 = int.Parse(m1.Get("Y1")), y2 = int.Parse(m1.Get("Y2"));
                    for (int y = y1; y <= y2; y++)
                        map[(x, y)] = '#';
                }
                else if (regex2.TryMatch(line, out var m2))
                {
                    int y = int.Parse(m2.Get("Y")), x1 = int.Parse(m2.Get("X1")), x2 = int.Parse(m2.Get("X2"));
                    for (int x = x1; x <= x2; x++)
                        map[(x, y)] = '#';
                }
            }
           
            return new Map(map);
        }

        private static (int x, int y) OneLeft((int x, int y) coord) => (coord.x - 1, coord.y);

        private static (int x, int y) OneRight((int x, int y) coord) => (coord.x + 1, coord.y);

        private static (int x, int y) OneDown((int x, int y) coord) => (coord.x, coord.y + 1);

        private class Map
        {
            private readonly Dictionary<(int x, int y), char> map;

            public Map(Dictionary<(int x, int y), char> map)
            {
                this.map = map;
                MinY = map.Keys.Min(x => x.y);
                MaxY = map.Keys.Max(x => x.y);
            }

            public int MinY { get; }

            public int MaxY { get; }

            public char this[(int x, int y) coord]
            {
                get => this.map.TryGetValue(coord, out char c) ? c : '.';
                set => this.map[coord] = value;
            }

            public int CountPart1() => this.map.Count(x => (x.Value == '|' || x.Value == '~') && x.Key.y >= MinY && x.Key.y <= MaxY);

            public int CountPart2() => this.map.Count(x => x.Value == '~' && x.Key.y >= MinY && x.Key.y <= MaxY);

            public bool Fill((int x, int y) coord, bool left)
            {
                int? from = left ? (int?)null : coord.x;
                int? to = left ? coord.x : (int?)null;
                int dx = left ? -1 : 1;

                var cur = coord;
                while (true)
                {
                    var c = this[cur];
                    if (c == '#')
                        break;

                    var below = this[OneDown(cur)];
                    if (c != '|' && below != '#' && below != '~')
                        return false;

                    cur = (cur.x + dx, cur.y);
                }

                from = left ? cur.x - dx : from;
                to = left ? to : cur.x - dx;

                if (from == null || to == null) 
                    return false;

                for (int x = from.Value; x <= to.Value; x++)
                    this.map[(x, coord.y)] = '~';

                return true;
            }

            // ReSharper disable once UnusedMember.Local
            public void Print()
            {
                int minx = map.Keys.Min(x => x.x);
                int maxx = map.Keys.Max(x => x.x);

                var sb = new StringBuilder();
                for (int y = MinY; y <= MaxY; y++)
                {
                    for (int x = minx; x <= maxx; x++)
                        sb.Append(map.TryGetValue((x, y), out char c) ? c : '.');
                    sb.Append('\n');
                }
                sb.Append('\n');

                Console.WriteLine(sb.ToString());
                //File.WriteAllText("out.txt", sb.ToString());
            }
        }
    }
}
