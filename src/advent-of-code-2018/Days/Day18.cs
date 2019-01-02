using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode2018.Days
{
    /*
--- Day 18: Settlers of The North Pole ---

On the outskirts of the North Pole base construction project, many Elves are collecting lumber.

The lumber collection area is 50 acres by 50 acres; each acre can be either open ground (.), trees (|), or a lumberyard (#).
You take a scan of the area (your puzzle input).

Strange magic is at work here: each minute, the landscape looks entirely different.
In exactly one minute, an open acre can fill with trees, a wooded acre can be converted to a lumberyard,
or a lumberyard can be cleared to open ground (the lumber having been sent to other projects).

The change to each acre is based entirely on the contents of that acre as well as the number of open, wooded,
or lumberyard acres adjacent to it at the start of each minute. Here, "adjacent" means any of the eight acres surrounding that acre.
(Acres on the edges of the lumber collection area might have fewer than eight adjacent acres; the missing acres aren't counted.)

In particular:

    An open acre will become filled with trees if three or more adjacent acres contained trees. Otherwise, nothing happens.
    An acre filled with trees will become a lumberyard if three or more adjacent acres were lumberyards. Otherwise, nothing happens.
    An acre containing a lumberyard will remain a lumberyard if it was adjacent to at least one other lumberyard and
        at least one acre containing trees. Otherwise, it becomes open.

These changes happen across all acres simultaneously,
each of them using the state of all acres at the beginning of the minute and changing to their new form by the end of that same minute.
Changes that happen during the minute don't affect each other.

For example, suppose the lumber collection area is instead only 10 by 10 acres with this initial configuration:

Initial state:
.#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.

After 1 minute:
.......##.
......|###
.|..|...#.
..|#||...#
..##||.|#|
...#||||..
||...|||..
|||||.||.|
||||||||||
....||..|.

After 2 minutes:
.......#..
......|#..
.|.|||....
..##|||..#
..###|||#|
...#|||||.
|||||||||.
||||||||||
||||||||||
.|||||||||

After 3 minutes:
.......#..
....|||#..
.|.||||...
..###|||.#
...##|||#|
.||##|||||
||||||||||
||||||||||
||||||||||
||||||||||

After 4 minutes:
.....|.#..
...||||#..
.|.#||||..
..###||||#
...###||#|
|||##|||||
||||||||||
||||||||||
||||||||||
||||||||||

After 5 minutes:
....|||#..
...||||#..
.|.##||||.
..####|||#
.|.###||#|
|||###||||
||||||||||
||||||||||
||||||||||
||||||||||

After 6 minutes:
...||||#..
...||||#..
.|.###|||.
..#.##|||#
|||#.##|#|
|||###||||
||||#|||||
||||||||||
||||||||||
||||||||||

After 7 minutes:
...||||#..
..||#|##..
.|.####||.
||#..##||#
||##.##|#|
|||####|||
|||###||||
||||||||||
||||||||||
||||||||||

After 8 minutes:
..||||##..
..|#####..
|||#####|.
||#...##|#
||##..###|
||##.###||
|||####|||
||||#|||||
||||||||||
||||||||||

After 9 minutes:
..||###...
.||#####..
||##...##.
||#....###
|##....##|
||##..###|
||######||
|||###||||
||||||||||
||||||||||

After 10 minutes:
.||##.....
||###.....
||##......
|##.....##
|##.....##
|##....##|
||##.####|
||#####|||
||||#|||||
||||||||||

After 10 minutes, there are 37 wooded acres and 31 lumberyards.
Multiplying the number of wooded acres by the number of lumberyards gives the total resource value after ten minutes: 37 * 31 = 1147.

What will the total resource value of the lumber collection area be after 10 minutes?

--- Part Two ---

This important natural resource will need to last for at least thousands of years. Are the Elves collecting this lumber sustainably?

What will the total resource value of the lumber collection area be after 1000000000 minutes?

     */
    internal class Day18 : DayBase
    {
        private static readonly (int x, int y)[] NeighboursDiff = { (-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1) };

        public override object Part1() => Solve(10);

        public override object Part2() => Solve(1000000000);

        private int Solve(int limit)
        {
            var map = Parse();
            var repeats = new Dictionary<string, int>();

            for (int i = 0; i < limit; i++)
            {
                var newMap = new Dictionary<(int x, int y), char>();
                foreach (var kvp in map)
                {
                    char newVal = kvp.Value;
                    if (kvp.Value == '.')
                    {
                        if (Neighbours(map, kvp.Key).Where(x => x == '|').HasAtLeast(3))
                            newVal = '|';
                    }
                    else if (kvp.Value == '|')
                    {
                        if (Neighbours(map, kvp.Key).Where(x => x == '#').HasAtLeast(3))
                            newVal = '#';
                    }
                    else if (kvp.Value == '#')
                    {
                        var grps = Neighbours(map, kvp.Key).GroupBy(x => x).Select(x => x.Key).ToImmutableHashSet();
                        if (!grps.Contains('#') || !grps.Contains('|'))
                            newVal = '.';
                    }
                    newMap[kvp.Key] = newVal;
                }

                map = newMap;

                var mapStr = string.Join("", map.OrderBy(x => x.Key.y).ThenBy(x => x.Key.x).Select(x => x.Value));
                if (repeats.TryGetValue(mapStr, out int iRepeat))
                {
                    int repeatOf = ((limit - iRepeat - 1) % (i - iRepeat)) + iRepeat;
                    var mm = repeats.First(x => x.Value == repeatOf).Key;
                    return mm.Count(x => x == '|') * mm.Count(x => x == '#');
                }
                repeats[mapStr] = i;
            }

            return map.Values.Count(x => x == '|') * map.Values.Count(x => x == '#');
        }

        private static IEnumerable<char> Neighbours(IDictionary<(int x, int y), char> map, (int x, int y) coord)
        {
            return NeighboursDiff.Select(d => (coord.x + d.x, coord.y + d.y))
                                 .Select(c => map.TryGetValue(c, out char val) ? val : ' ');
        }

        private Dictionary<(int x, int y), char> Parse()
        {
            var data = Input.Split("\n");
            var map = new Dictionary<(int x, int y), char>();

            for (int y = 0; y < data.Length; y++)
            {
                for (int x = 0; x < data[y].Length; x++)
                    map[(x, y)] = data[y][x];
            }

            return map;
        }
    }
}
