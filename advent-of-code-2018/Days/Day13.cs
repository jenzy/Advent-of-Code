using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days
{
    /*
--- Day 13: Mine Cart Madness ---

A crop of this size requires significant logistics to transport produce, soil, fertilizer, and so on.
The Elves are very busy pushing things around in carts on some kind of rudimentary system of tracks they've come up with.

Seeing as how cart-and-track systems don't appear in recorded history for another 1000 years,
the Elves seem to be making this up as they go along. They haven't even figured out how to avoid collisions yet.

You map out the tracks (your puzzle input) and see where you can help.

Tracks consist of straight paths (| and -), curves (/ and \), and intersections (+).
Curves connect exactly two perpendicular pieces of track; for example, this is a closed loop:

/----\
|    |
|    |
\----/

Intersections occur when two perpendicular paths cross. At an intersection, a cart is capable of turning left,
turning right, or continuing straight. Here are two loops connected by two intersections:

/-----\
|     |
|  /--+--\
|  |  |  |
\--+--/  |
   |     |
   \-----/

Several carts are also on the tracks. Carts always face either up (^), down (v), left (<), or right (>).
(On your initial map, the track under each cart is a straight path matching the direction the cart is facing.)

Each time a cart has the option to turn (by arriving at any intersection), it turns left the first time,
goes straight the second time, turns right the third time, and then repeats those directions starting again with
left the fourth time, straight the fifth time, and so on. This process is independent of the particular intersection
at which the cart has arrived - that is, the cart has no per-intersection memory.

Carts all move at the same speed; they take turns moving a single step at a time.
They do this based on their current location: carts on the top row move first (acting from left to right),
then carts on the second row move (again from left to right), then carts on the third row, and so on.
Once each cart has moved one step, the process repeats; each of these loops is called a tick.

For example, suppose there are two carts on a straight track:

|  |  |  |  |
v  |  |  |  |
|  v  v  |  |
|  |  |  v  X
|  |  ^  ^  |
^  ^  |  |  |
|  |  |  |  |

First, the top cart moves. It is facing down (v), so it moves down one square.
Second, the bottom cart moves. It is facing up (^), so it moves up one square.
Because all carts have moved, the first tick ends. Then, the process repeats, starting with the first cart.
The first cart moves down, then the second cart moves up - right into the first cart, colliding with it!
(The location of the crash is marked with an X.) This ends the second and last tick.

Here is a longer example:

/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   

/-->\        
|   |  /----\
| /-+--+-\  |
| | |  | |  |
\-+-/  \->--/
  \------/   

/---v        
|   |  /----\
| /-+--+-\  |
| | |  | |  |
\-+-/  \-+>-/
  \------/   

/---\        
|   v  /----\
| /-+--+-\  |
| | |  | |  |
\-+-/  \-+->/
  \------/   

/---\        
|   |  /----\
| /->--+-\  |
| | |  | |  |
\-+-/  \-+--^
  \------/   

/---\        
|   |  /----\
| /-+>-+-\  |
| | |  | |  ^
\-+-/  \-+--/
  \------/   

/---\        
|   |  /----\
| /-+->+-\  ^
| | |  | |  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /----<
| /-+-->-\  |
| | |  | |  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /---<\
| /-+--+>\  |
| | |  | |  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /--<-\
| /-+--+-v  |
| | |  | |  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /-<--\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   

/---\        
|   |  /<---\
| /-+--+-\  |
| | |  | |  |
\-+-/  \-<--/
  \------/   

/---\        
|   |  v----\
| /-+--+-\  |
| | |  | |  |
\-+-/  \<+--/
  \------/   

/---\        
|   |  /----\
| /-+--v-\  |
| | |  | |  |
\-+-/  ^-+--/
  \------/   

/---\        
|   |  /----\
| /-+--+-\  |
| | |  X |  |
\-+-/  \-+--/
  \------/   

After following their respective paths for a while, the carts eventually crash.
To help prevent crashes, you'd like to know the location of the first crash.
Locations are given in X,Y coordinates, where the furthest left column is X=0 and the furthest top row is Y=0:

           111
 0123456789012
0/---\        
1|   |  /----\
2| /-+--+-\  |
3| | |  X |  |
4\-+-/  \-+--/
5  \------/   

In this example, the location of the first crash is 7,3.

--- Part Two ---

There isn't much you can do to prevent crashes in this ridiculous system.
However, by predicting the crashes, the Elves know where to be in advance and
instantly remove the two crashing carts the moment any crash occurs.

They can proceed like this for a while, but eventually, they're going to run out of carts.
It could be useful to figure out where the last cart that hasn't crashed will end up.

For example:

/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/

/---\  
|   |  
| v-+-\
| | | |
\-+-/ |
  |   |
  ^---^

/---\  
|   |  
| /-+-\
| v | |
\-+-/ |
  ^   ^
  \---/

/---\  
|   |  
| /-+-\
| | | |
\-+-/ ^
  |   |
  \---/

After four very expensive crashes, a tick ends with only one cart remaining; its final location is 6,4.

What is the location of the last cart at the end of the first tick where it is the only cart left?

     */
    internal class Day13 : IDay
    {
        public object Part1(string input)
        {
            var (map, carts) = Parse(input);

            while (true)
            {
                foreach (var cart in carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList())
                {
                    var (newX, newY) = cart.GetNewLocation();
                    cart.MakeTurn(cart.GetTurn(map[newY][newX]));

                    carts.Remove(cart);
                    cart.Move(newX, newY);

                    if (carts.Contains(cart))
                        return cart;

                    carts.Add(cart);
                }
            }
        }

        public object Part2(string input)
        {
            var (map, carts) = Parse(input);

            while (carts.Count > 1)
            {
                foreach (var cart in carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList())
                {
                    if (!carts.Contains(cart))
                        continue;

                    var (newX, newY) = cart.GetNewLocation();
                    cart.MakeTurn(cart.GetTurn(map[newY][newX]));

                    carts.Remove(cart);
                    cart.Move(newX, newY);

                    if (carts.Contains(cart))
                        carts.Remove(cart);
                    else
                        carts.Add(cart);
                }
            }

            return carts.Single();
        }

        private static (char[][] map, ISet<Cart> carts) Parse(string input)
        {
            var map = input.Split("\n").Select(l => l.ToCharArray()).ToArray();
            var mapping = new Dictionary<char, (char track, Direction dir)>
            {
                ['>'] = ('-', Direction.East),
                ['<'] = ('-', Direction.West),
                ['v'] = ('|', Direction.South),
                ['^'] = ('|', Direction.North)
            };

            var carts = new HashSet<Cart>();
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (mapping.TryGetValue(map[y][x], out var m))
                    {
                        map[y][x] = m.track;
                        carts.Add(new Cart(x, y, m.dir));
                    }
                }
            }

            return (map, carts);
        }

        private enum Turn { Left, Straight, Right }

        private enum Direction { North, East, South, West }

        private class Cart
        {
            public Cart(int x, int y, Direction direction) => (X, Y, Direction) = (x, y, direction);

            public int X { get; private set; }

            public int Y { get; private set; }

            private Direction Direction { get; set; }

            private Turn Turn { get; set; }

            public (int x, int y) GetNewLocation()
            {
                switch (Direction)
                {
                    case Direction.North: return (X, Y - 1);
                    case Direction.East: return (X + 1, Y);
                    case Direction.South: return (X, Y + 1);
                    case Direction.West: return (X - 1, Y);
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            public void Move(int x, int y) => (this.X, this.Y) = (x, y);

            public Turn GetTurn(char c)
            {
                switch (c)
                {
                    case '\\':
                        return Direction == Direction.East || Direction == Direction.West ? Turn.Right : Turn.Left;

                    case '/':
                        return Direction == Direction.East || Direction == Direction.West ? Turn.Left : Turn.Right;

                    case '+':
                        var turn = this.Turn;
                        this.Turn = (Turn) (((int) this.Turn + 1) % 3);
                        return turn;

                    default:
                        return Turn.Straight;
                }
            }

            public void MakeTurn(Turn turn)
            {
                if (turn == Turn.Right)
                    this.Direction = (Direction) (((int) Direction + 1) % 4);
                else if (turn == Turn.Left)
                    this.Direction = (Direction) (((int) Direction + 3) % 4);
            }

            private bool Equals(Cart other) => X == other.X && Y == other.Y;

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == this.GetType() && Equals((Cart) obj);
            }

            // ReSharper disable NonReadonlyMemberInGetHashCode
            public override int GetHashCode() => (X * 397) ^ Y;

            public override string ToString() => $"{X},{Y}";
        }
    }
}
