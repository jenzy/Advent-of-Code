using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Days
{
    /*

     */
    internal class Day17 : DayBase
    {
        public override object Part1()
        {
//            Input = @"x=495, y=2..7
//y=7, x=495..501
//x=501, y=3..7
//x=498, y=2..4
//x=506, y=1..2
//x=498, y=10..13
//x=504, y=10..13
//y=13, x=498..504";
            var map = Parse();
            int minx = map.Keys.Min(x => x.x);
            int maxx = map.Keys.Max(x => x.x);
            int miny = map.Keys.Min(x => x.y);
            int maxy = map.Keys.Max(x => x.y);

            var stack = new Stack<(int x, int y)>();
            stack.Push((500, 0));

            Print(map);

            while (stack.Any())
            {
                var coord = stack.Pop();
                if (coord.y > maxy)
                    continue;

                var c = Get(map, coord);
                if (c == '#' || c == '~')
                    continue;

                //if (c == '|')
                //{
                //    Print(map);
                //}

                var coordBelow = (coord.x, y: coord.y + 1);
                var below = Get(map, coordBelow);

                if (below == '#' || below == '~')
                {
                    map[coord] = '|';
                    var left = (coord.x - 1, coord.y);
                    var right = (coord.x + 1, coord.y);

                    bool filled = false;
                    if (Get(map, left) == '#')
                    {
                        filled = Fill(map, coord, left: false);
                        if (!filled)
                        {
                            stack.Push(right);
                        }
                    }
                    else if (Get(map, right) == '#')
                    {
                        filled = Fill(map, coord, left: true);
                        if (!filled)
                        {
                            stack.Push(left);
                        }
                    }
                    else
                    {
                        if (Get(map, right) == '.')
                            stack.Push(right);
                        if (Get(map, left) == '.')
                            stack.Push(left);
                    }
                }
                else if (below == '|')
                {
                    map[coord] = '|';
                    continue;
                }
                else
                {
                    Debug.Assert(below == '.');
                    map[coord] = '|';
                    if (coordBelow.y <= maxy)
                    {
                        stack.Push(coord);
                        stack.Push(coordBelow);
                    }
                }

                //Print(map);
                //Console.Write(">");
                //Console.ReadKey();
            }

            //Print(map);

            //var r = map.Count(x => (x.Value == '|' || x.Value == '~')
            //                       && x.Key.y >= miny
            //                       && x.Key.y <= maxy);

            var r = map.Count(x => (x.Value == '~')
                                   && x.Key.y >= miny
                                   && x.Key.y <= maxy);

            return r;
        }

        public override object Part2()
        {
            Console.WriteLine("TODO");
            return null;
        }

        private static void Print(Dictionary<(int x, int y), char> map)
        {
            int minx = map.Keys.Min(x => x.x);
            int maxx = map.Keys.Max(x => x.x);
            int miny = map.Keys.Min(x => x.y);
            int maxy = map.Keys.Max(x => x.y);

            var sb = new StringBuilder("\n");

            for (int y = miny; y <= maxy; y++)
            {
                for (int x = minx; x <= maxx; x++)
                {
                    if (map.TryGetValue((x, y), out char c))
                        sb.Append(c);
                    else
                        sb.Append('.');
                }
                sb.Append('\n');
            }
                sb.Append('\n');
                sb.Append('\n');
                //Console.WriteLine(sb.ToString());

            File.WriteAllText("out.txt", sb.ToString());
        }

        private static char Get(Dictionary<(int x, int y), char> map, (int x, int y) coord)
        {
            if (map.TryGetValue(coord, out char c))
                return c;

            return '.';
        }

        private static bool Fill(Dictionary<(int x, int y), char> map, (int x, int y) coord, bool left)
        {
            int? from = null;
            int? to = null;
            if (left)
            {
                to = coord.x;
                var cur = coord;
                while (true)
                {
                    var c = Get(map, cur);
                    if (c == '#')
                        break;

                    if (c != '|')
                        return false;

                    var coordBelow = (cur.x, cur.y + 1);
                    var below = Get(map, coordBelow);
                    if (below != '#' && below != '~')
                        return false;

                    cur = (cur.x - 1, cur.y);
                }
                from = cur.x + 1;
            }
            else
            {
                from = coord.x;
                var cur = coord;
                while (true)
                {
                    var c = Get(map, cur);
                    if (c == '#')
                        break;

                    if (c != '|')
                        return false;

                    var coordBelow = (cur.x, cur.y + 1);
                    var below = Get(map, coordBelow);
                    if (below != '#' && below != '~')
                        return false;

                    cur = (cur.x + 1, cur.y);
                }
                to = cur.x - 1;
            }

            if (from != null && to != null)
            {
                for (int x = from.Value; x <= to.Value; x++)
                {
                    map[(x, coord.y)] = '~';
                }
            }

            return true;
        }

        private Dictionary<(int x, int y), char> Parse()
        {
            var regex1 = new Regex(@"x=(?<X>\d+), y=(?<Y1>\d+)..(?<Y2>\d+)");
            var regex2 = new Regex(@"y=(?<Y>\d+), x=(?<X1>\d+)..(?<X2>\d+)");

            var input = Input.Split("\n");
            var clay1 = input.Select(x => regex1.Match(x))
                            .Where(x => x.Success)
                            .Select(x => new
                            {
                                X = int.Parse(x.Groups["X"].Value),
                                Y1 = int.Parse(x.Groups["Y1"].Value),
                                Y2 = int.Parse(x.Groups["Y2"].Value),
                            });

            var clay2 = input.Select(x => regex2.Match(x))
                             .Where(x => x.Success)
                             .Select(x => new
                             {
                                 Y = int.Parse(x.Groups["Y"].Value),
                                 X1 = int.Parse(x.Groups["X1"].Value),
                                 X2 = int.Parse(x.Groups["X2"].Value),
                             });

            var map = new Dictionary<(int x, int y), char>();

            foreach (var c in clay1)
            {
                for (int y = c.Y1; y <= c.Y2; y++)
                    map[(c.X, y)] = '#';
            }

            foreach (var c in clay2)
            {
                for (int x = c.X1; x <= c.X2; x++)
                    map[(x, c.Y)] = '#';
            }

            int minx = map.Keys.Min(x => x.x);
            int maxx = map.Keys.Max(x => x.x);
            int miny = map.Keys.Min(x => x.y);
            int maxy = map.Keys.Max(x => x.y);

            //for (int y = miny; y <= maxy; y++)
            //{
            //    for (int x = minx; x <= maxx; x++)
            //    {
            //        if (!map.ContainsKey((x, y)))
            //            map[(x, y)] = '.';
            //    }
            //}

            return map;
        }
    }
}
