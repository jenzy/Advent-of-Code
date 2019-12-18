using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day18 : DayBase
    {
        public override object Part1() => CollectKeys(Parse(Input, false));

        public override object Part2() => CollectKeys(Parse(Input, true));

        private static Dictionary<(int x, int y), char> Parse(string input, bool part2)
        {
            var lines = input.Split('\n').Select(l => l.ToCharArray()).ToList();

            if (part2)
            {
                var iMid = lines.Count / 2;
                Debug.Assert(lines[iMid][iMid] == '@');

                var replace = new char[,] { { '@', '#', '@' }, { '#', '#', '#' }, { '@', '#', '@' } };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                        lines[iMid - 1 + i][iMid - 1 + j] = replace[i, j];
                }
            }

            return lines.SelectMany((row, y) => row.Select((c, x) => (pos: (x, y), val: c)))
                        .ToDictionary(x => x.pos, x => x.val);
        }

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(18);
            Assert.Equal(3832, day.Part1());
            Assert.Equal(1724, day.Part2());
        }

        private int CollectKeys(Dictionary<(int x, int y), char> grid)
        {
            var locations = grid.Where(kvp => char.IsLetter(kvp.Value) || kvp.Value == '@')
                                .ToLookup(kvp => kvp.Value, kvp => kvp.Key);
            var keys = locations.Select(g => g.Key).Where(char.IsLower).ToList();

            // Cache accessible keys for each relevant location
            var keysInRange = new Dictionary<(int x, int y), List<Key>>();
            foreach (var pos in locations['@'].Concat(keys .Select(k => locations[k].Single())))
                keysInRange[pos] = GetKeysInRange(grid, pos).ToList();

            var queue = new Queue<State>();
            queue.Enqueue(new State(robots: locations['@']));

            var visited = new Dictionary<State, int>();
            var min = int.MaxValue;
            int allKeys = (int)Math.Pow(2, keys.Count) - 1;

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();

                if (visited.TryGetValue(state, out var steps) && steps <= state.Steps)
                    continue;
                visited[state] = state.Steps;

                if (state.Keys == allKeys)
                {
                    min = Math.Min(min, state.Steps);
                    continue;
                }

                for (int iRobot = 0; iRobot < state.Robots.Length; iRobot++)
                {
                    foreach (var key in keysInRange[state.Robots[iRobot]])
                    {
                        if ((state.Keys & key.Bit) > 0 || (key.Obstacles & ~state.Keys) > 0)
                            continue;
                        queue.Enqueue(state.MoveToKey(iRobot, locations[key.Id].Single(), key));
                    }
                }
            }

            return min;
        }

        private static IEnumerable<(int x, int y)> Neighbours((int x, int y) pos)
        {
            var (x, y) = pos;
            yield return (x, y - 1);
            yield return (x - 1, y);
            yield return (x + 1, y);
            yield return (x, y + 1);
        }

        private IEnumerable<Key> GetKeysInRange(Dictionary<(int x, int y), char> grid, (int x, int y) from)
        {
            var visited = new HashSet<(int x, int y)>();
            var queue = new Queue<((int x, int y) pos, int dist, int obst)>();
            queue.Enqueue((from, 0, 0));

            while (queue.Count > 0)
            {
                var (pos, dist, obst) = queue.Dequeue();
                if (visited.Contains(pos))
                    continue;
                visited.Add(pos);

                if (grid.TryGetValue(pos, out char c) && c != '#')
                {
                    if (char.IsLower(c))
                        yield return new Key(c, dist, obst);

                    if (char.IsUpper(c))
                        obst |= (int)Math.Pow(2, c - 'A');

                    foreach (var p in Neighbours(pos))
                        queue.Enqueue((p, dist + 1, obst));
                }
            }
        }

        private class State
        {
            public State(IEnumerable<(int x, int y)> robots = null) => Robots = robots?.ToArray();

            public (int x, int y)[] Robots { get; }

            public int Keys { get; private set; }

            public int Steps { get; private set; }

            public State MoveToKey(int iRobot, (int x, int y) keyPosition, Key key)
            {
                var state = new State(Robots)
                {
                    Steps = this.Steps + key.Distance,
                    Keys = this.Keys | key.Bit
                };
                state.Robots[iRobot] = keyPosition;
                return state;
            }

            public override bool Equals(object obj) => obj is State state && Keys == state.Keys && Robots.SequenceEqual(state.Robots);

            public override int GetHashCode()
            {
                int hc = Robots.Length;
                foreach (var robot in Robots)
                    hc = unchecked((hc * 314159) + robot.GetHashCode());
                return HashCode.Combine(hc, Keys);
            }
        }

        private class Key
        {
            public Key(char id, int distance, int obstacles)
            {
                Id = id;
                Distance = distance;
                Obstacles = obstacles;
            }

            public char Id { get; }

            public int Distance { get; }

            public int Obstacles { get; }

            public int Bit => (int)Math.Pow(2, Id - 'a');
        }
    }
}
