using AdventOfCode.Common;
using AdventOfCode.Y2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*


     */

    public class Day17 : DayBase
    {
        public override object Part1()
        {
            var output = string.Join("", new Intcode(Parse(Input)).Run().Output.Select(x => (char)x));

            Console.WriteLine("\n" + output);
            
            var grid = output.Split('\n')
                             .SelectMany((yy, y) => yy
                             .Select((xx, x) => (x, y, xx)))
                             .Where(x => x.xx == '#')
                             .ToDictionary(x => (x.x, x.y), x => x.xx);



            int count = 0;
            foreach(var kvp in grid.Keys)
            {
                if (grid.ContainsKey((kvp.x - 1, kvp.y))
                    && grid.ContainsKey((kvp.x + 1, kvp.y))
                    && grid.ContainsKey((kvp.x, kvp.y - 1))
                    && grid.ContainsKey((kvp.x, kvp.y - 1))
                    )
                    count += kvp.x * kvp.y;
            }

            return count;
        }

        public override object Part2()
        {
            var output = string.Join("", new Intcode(Parse(Input)).Run().Output.Select(x => (char)x));

            Console.WriteLine("\n" + output);

            var grid = output.Split('\n').SelectMany((yy, y) => yy.Select((xx, x) => (x, y, xx)).ToDictionary(x => (x.x, x.y), x => x.xx));
            var robotTmp = grid.Single(kvp => new[] { '^', 'v', '<', '>' }.Contains(kvp.Value));
            var robotPos = (x: robotTmp.Key.x, y: robotTmp.Key.y);
            var robotDir = robotTmp.Value;
            var scaffold = grid.Where(kvp => kvp.Value == '#').Select(x => x.Key).ToHashSet();

            int currentMove = 0;
            var path = new List<string>();

            while (true)
            {
                var newPos = NewPosition(robotPos, robotDir);
                if (scaffold.Contains(newPos))
                {
                    robotPos = newPos;
                    currentMove++;
                }
                else
                {
                    if (currentMove > 0)
                        path.Add(currentMove.ToString());
                    currentMove = 0;

                    var dirs = GetDirections(robotDir).Select((x, i) => (d: x, i));
                    var dir = dirs.FirstOrDefault(d => scaffold.Contains(NewPosition(robotPos, d.d)));
                    if (dir.d == default)
                        break;

                    robotDir = dir.d;
                    var rot = dir.i == 0 ? "L" : "R";
                    path.Add(rot);
                }
            }



            return string.Join(",", path);
        }

        // R,6,L,10,R,8,R,8,R,12,L,8,L,10,R,6,L,10,R,8,R,8,R,12,L,10,R,6,L,10,R,12,L,8,L,10,R,12,L,10,R,6,L,10,R,6,L,10,R,8,R,8,R,12,L,8,L,10,R,6,L,10,R,8,R,8,R,12,L,10,R,6,L,10

        private static (int x, int y) NewPosition((int x, int y) pos, char direction) => direction switch
        {
            '^' => (pos.x, pos.y - 1),
            'v' => (pos.x, pos.y + 1),
            '<' => (pos.x - 1, pos.y),
            '>' => (pos.x + 1, pos.y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

        private static char[] GetDirections(char direction) => direction switch
        {
            '^' => new[] { '<', '>' },
            'v' => new[] { '>', '<' },
            '<' => new[] { 'v', '^' },
            '>' => new[] { '^', 'v' },
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        private static IEnumerable<int> GetPattern(int round, int len)
        {
            return Generate().RepeatForever().Skip(1).Take(len);

            IEnumerable<int> Generate()
            {
                foreach (var num in new[] { 0, 1, 0, -1 })
                {
                    for (int i = round; i > 0; --i)
                        yield return num;
                }
            }
        }

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(17);
            Assert.Equal("19944447", day.Part1());
            Assert.Equal("81207421", day.Part2());
        }
    }
}

/*
 R,6,L,10,R,8,R,8,R,12,L,8,L,10,R,6,L,10,R,8,R,8,R,12,L,10,R,6,L,10,R,12,L,8,L,10,R,12,L,10,R,6,L,10,R,6,L,10,R,8,R,8,R,12,L,8,L,10,R,6,L,10,R,8,R,8,R,12,L,10,R,6,L,10

R,6,L,2,R,8,L,8,L,8,L8,R4,
L,8,L,10,R,6,L,2,R,8,L,8,L,8,R4
L,10,R,6,L,10,R,12,L,8,L,10,R,12,L,10,R,6,L,10,R,6,
L2,R,8,L,8,L,8,L,8,R,4
L,8,L,10,R,6,
L,10,R,8,R,8,R,12,L,10,R,6,L,10



........................................#########...........#########........
........................................#.......#...........#.......#........
........................................#.......#...........#.......#........
........................................#.......#...........#.......#........
........................................#.......#...........#.......#........
........................................#.......#...........#.......#........
........................................#.......#...........#.......#........
........................................#.......#...........#.......#........
........................................#.......#.......#############........
........................................#.......#.......#...#................
#########...................#############.......#.....^######................
#.......#...................#...................#.......#....................
#.......#...................#...................###########..................
#.......#...................#...........................#.#..................
#.......#...................#...........................#.#..................
#.......#...................#...........................#.#..................
#.......#...................#...........................###########..........
#.......#...................#.............................#.......#..........
##########x.................#.............................###########........
........#.#.................#.....................................#.#........
........#.#...........#######.....................................#.#........
........#.#...........#...........................................#.#........
........###########...#...........................................###########
..........#.......#...#.............................................#.......#
..........###########.#.............................................#.......#
..................#.#.#.............................................#.......#
..................#.#.#.............................................#.......#
..................#.#.#.............................................#.......#
..................###########.......................................#.......#
....................#.#.............................................#.......#
................x######.............................................#########
................#...#........................................................
........#############........................................................
........#.......#............................................................
........#.......#............................................................
........#.......#............................................................
........#.......#............................................................
........#.......#............................................................
........#.......#............................................................
........#.......#............................................................
........#########............................................................
 */
