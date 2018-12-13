using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days
{
    /*

     */
    internal class Day13 : IDay
    {
        public object Part1(string input)
        {
            var (map, carts) = Parse(input);

            while (true)
            {
                var cartTmp = carts.Values
                                   .OrderBy(c => c.Y)
                                   .ThenBy(c => c.X)
                                   .ToList();

                foreach (var cart in cartTmp)
                {
                    int newX = cart.X, newY = cart.Y;
                    switch (cart.Dir)
                    {
                        case Dir.U:
                            newY--;
                            break;
                        case Dir.R:
                            newX++;
                            break;
                        case Dir.D:
                            newY++;
                            break;
                        case Dir.L:
                            newX--;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    var newDir = cart.Dir;
                    var c = map[newY][newX];
                    switch (c)
                    {
                        case '-':
                        case '|':
                            break;

                        case '\\' when newDir == Dir.R:
                            newDir = Dir.D;
                            break;

                        case '\\' when newDir == Dir.U:
                            newDir = Dir.L;
                            break;

                        case '\\' when newDir == Dir.D:
                            newDir = Dir.R;
                            break;

                        case '\\' when newDir == Dir.L:
                            newDir = Dir.U;
                            break;

                        case '/' when newDir == Dir.R:
                            newDir = Dir.U;
                            break;

                        case '/' when newDir == Dir.U:
                            newDir = Dir.R;
                            break;

                        case '/' when newDir == Dir.D:
                            newDir = Dir.L;
                            break;

                        case '/' when newDir == Dir.L:
                            newDir = Dir.D;
                            break;

                        case '+' when newDir == Dir.L && cart.Turn == Turn.L:
                            newDir = Dir.D;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.L && cart.Turn == Turn.R:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            newDir = Dir.U;
                            break;

                        case '+' when newDir == Dir.L && cart.Turn == Turn.S:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.R && cart.Turn == Turn.L:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            newDir = Dir.U;
                            break;

                        case '+' when newDir == Dir.R && cart.Turn == Turn.R:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            newDir = Dir.D;
                            break;

                        case '+' when newDir == Dir.R && cart.Turn == Turn.S:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.U && cart.Turn == Turn.L:
                            newDir = Dir.L;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.U && cart.Turn == Turn.R:
                            newDir = Dir.R;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.U && cart.Turn == Turn.S:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.D && cart.Turn == Turn.L:
                            newDir = Dir.R;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.D && cart.Turn == Turn.R:
                            newDir = Dir.L;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.D && cart.Turn == Turn.S:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;
                    }

                    carts.Remove((cart.X, cart.Y));
                    cart.X = newX;
                    cart.Y = newY;
                    cart.Dir = newDir;

                    if (carts.ContainsKey((cart.X, cart.Y)))
                    {
                        return (cart.X, cart.Y);
                    }

                    carts.Add((cart.X, cart.Y), cart);
                }
            }

            return null;
        }

        public object Part2(string input)
        {
            var (map, carts) = Parse(input);

            while (true)
            {
                var cartTmp = carts.Values
                                   .OrderBy(c => c.Y)
                                   .ThenBy(c => c.X)
                                   .ToList();

                foreach (var cart in cartTmp)
                {
                    if (!carts.ContainsKey((cart.X, cart.Y)))
                        continue;

                    int newX = cart.X, newY = cart.Y;
                    switch (cart.Dir)
                    {
                        case Dir.U:
                            newY--;
                            break;
                        case Dir.R:
                            newX++;
                            break;
                        case Dir.D:
                            newY++;
                            break;
                        case Dir.L:
                            newX--;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    var newDir = cart.Dir;
                    var c = map[newY][newX];
                    switch (c)
                    {
                        case '-':
                        case '|':
                            break;

                        case '\\' when newDir == Dir.R:
                            newDir = Dir.D;
                            break;

                        case '\\' when newDir == Dir.U:
                            newDir = Dir.L;
                            break;

                        case '\\' when newDir == Dir.D:
                            newDir = Dir.R;
                            break;

                        case '\\' when newDir == Dir.L:
                            newDir = Dir.U;
                            break;

                        case '/' when newDir == Dir.R:
                            newDir = Dir.U;
                            break;

                        case '/' when newDir == Dir.U:
                            newDir = Dir.R;
                            break;

                        case '/' when newDir == Dir.D:
                            newDir = Dir.L;
                            break;

                        case '/' when newDir == Dir.L:
                            newDir = Dir.D;
                            break;

                        case '+' when newDir == Dir.L && cart.Turn == Turn.L:
                            newDir = Dir.D;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.L && cart.Turn == Turn.R:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            newDir = Dir.U;
                            break;

                        case '+' when newDir == Dir.L && cart.Turn == Turn.S:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.R && cart.Turn == Turn.L:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            newDir = Dir.U;
                            break;

                        case '+' when newDir == Dir.R && cart.Turn == Turn.R:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            newDir = Dir.D;
                            break;

                        case '+' when newDir == Dir.R && cart.Turn == Turn.S:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.U && cart.Turn == Turn.L:
                            newDir = Dir.L;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.U && cart.Turn == Turn.R:
                            newDir = Dir.R;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.U && cart.Turn == Turn.S:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.D && cart.Turn == Turn.L:
                            newDir = Dir.R;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.D && cart.Turn == Turn.R:
                            newDir = Dir.L;
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;

                        case '+' when newDir == Dir.D && cart.Turn == Turn.S:
                            cart.Turn = (Turn)((((int) cart.Turn)+1)%3);
                            break;
                    }

                    carts.Remove((cart.X, cart.Y));
                    cart.X = newX;
                    cart.Y = newY;
                    cart.Dir = newDir;

                    if (carts.ContainsKey((cart.X, cart.Y)))
                    {
                        carts.Remove((cart.X, cart.Y));
                        Console.Write("Left: " + carts.Count + "\n");
                    }
                    else
                    {
                        carts.Add((cart.X, cart.Y), cart);
                    }
                }

                if (carts.Count == 1)
                {
                    var c = carts.Values.First();
                    return (c.X, c.Y);
                }
            }

            return null;
        }

        private static (char[][] map, IDictionary<(int x, int y), Cart> carts) Parse(string input)
        {
            var map = input.Split("\n")
                           .Select(l => l.ToCharArray())
                           .ToArray();

            var carts = new Dictionary<(int x, int y), Cart>();
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    var c = map[y][x];
                    switch (c)
                    {
                        case '>':
                            map[y][x] = '-';
                            carts.Add((x, y), new Cart() { X = x, Y = y, Dir = Dir.R });
                            break;

                        case '<':
                            map[y][x] = '-';
                            carts.Add((x, y), new Cart() { X = x, Y = y, Dir = Dir.L });
                            break;

                        case 'v':
                            map[y][x] = '|';
                            carts.Add((x, y), new Cart() { X = x, Y = y, Dir = Dir.D });
                            break;

                        case '^':
                            map[y][x] = '|';
                            carts.Add((x, y), new Cart() { X = x, Y = y, Dir = Dir.U });
                            break;
                    }
                }
            }

            return (map, carts);
        }

        enum Turn {L, S, R}
        enum Dir {U, R, D, L}

        class Cart
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Dir Dir { get; set; }
            public Turn Turn { get; set; }

            protected bool Equals(Cart other)
            {
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Cart) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (X * 397) ^ Y;
                }
            }

            public static bool operator ==(Cart left, Cart right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Cart left, Cart right)
            {
                return !Equals(left, right);
            }
        }
    }
}
