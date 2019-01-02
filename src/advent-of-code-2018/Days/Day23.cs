using AdventOfCode.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Math;

namespace AdventOfCode.Y2018.Days
{
    /*
--- Day 23: Experimental Emergency Teleportation ---

Using your torch to search the darkness of the rocky cavern, you finally locate the man's friend: a small reindeer.

You're not sure how it got so far in this cave. It looks sick - too sick to walk - and too heavy for you to carry all the way back.
Sleighs won't be invented for another 1500 years, of course.

The only option is experimental emergency teleportation.

You hit the "experimental emergency teleportation" button on the device and push I accept the risk on no fewer than 18 different warning messages.
Immediately, the device deploys hundreds of tiny nanobots which fly around the cavern, apparently assembling themselves into a very specific formation.
The device lists the X,Y,Z position (pos) for each nanobot as well as its signal radius (r) on its tiny screen (your puzzle input).

Each nanobot can transmit signals to any integer coordinate which is a distance away from it less than or equal to its signal radius
(as measured by Manhattan distance). Coordinates a distance away of less than or equal to a nanobot's signal radius are said to be in range of that nanobot.

Before you start the teleportation process, you should determine which nanobot is the strongest (that is, which has the largest signal radius) and then,
for that nanobot, the total number of nanobots that are in range of it, including itself.

For example, given the following nanobots:

pos=<0,0,0>, r=4
pos=<1,0,0>, r=1
pos=<4,0,0>, r=3
pos=<0,2,0>, r=1
pos=<0,5,0>, r=3
pos=<0,0,3>, r=1
pos=<1,1,1>, r=1
pos=<1,1,2>, r=1
pos=<1,3,1>, r=1

The strongest nanobot is the first one (position 0,0,0) because its signal radius, 4 is the largest.
Using that nanobot's location and signal radius, the following nanobots are in or out of range:

    The nanobot at 0,0,0 is distance 0 away, and so it is in range.
    The nanobot at 1,0,0 is distance 1 away, and so it is in range.
    The nanobot at 4,0,0 is distance 4 away, and so it is in range.
    The nanobot at 0,2,0 is distance 2 away, and so it is in range.
    The nanobot at 0,5,0 is distance 5 away, and so it is not in range.
    The nanobot at 0,0,3 is distance 3 away, and so it is in range.
    The nanobot at 1,1,1 is distance 3 away, and so it is in range.
    The nanobot at 1,1,2 is distance 4 away, and so it is in range.
    The nanobot at 1,3,1 is distance 5 away, and so it is not in range.

In this example, in total, 7 nanobots are in range of the nanobot with the largest signal radius.

Find the nanobot with the largest signal radius. How many nanobots are in range of its signals?

--- Part Two ---

Now, you just need to figure out where to position yourself so that you're actually teleported when the nanobots activate.

To increase the probability of success, you need to find the coordinate which puts you in range of the largest number of nanobots.
If there are multiple, choose one closest to your position (0,0,0, measured by manhattan distance).

For example, given the following nanobot formation:

pos=<10,12,12>, r=2
pos=<12,14,12>, r=2
pos=<16,12,12>, r=4
pos=<14,14,14>, r=6
pos=<50,50,50>, r=200
pos=<10,10,10>, r=5

Many coor
     */
    internal class Day23 : DayBase
    {
        public override object Part1()
        {
            var bots = Parse();
            var best = bots.MaxBy(x => x.R);
            return bots.Count(b => Dist(best, (b.X, b.Y, b.Z)) <= best.R);
        }

        public override object Part2()
        {
            var bots = Parse();
            var xx = bots.MinMax(b => b.X);
            var yy = bots.MinMax(b => b.Y);
            var zz = bots.MinMax(b => b.Z);

            long range = Min(xx.max - xx.min, Min(yy.max - yy.min, zz.max - zz.min));
            var zero = new Bot((0, 0, 0), 0);

            while (true)
            {
                long maxBotsInRange = 0;
                Bot bestPosition = null;

                var scan = from x in Utils.Range(xx.min, xx.max - xx.min, range)
                           from y in Utils.Range(yy.min, yy.max - yy.min, range)
                           from z in Utils.Range(zz.min, zz.max - zz.min, range)
                           select (x, y, z);

                foreach (var p in scan)
                {
                    long nBotsInRange = bots.Count(b => Dist(b, p) <= b.R);
                    if (nBotsInRange >= maxBotsInRange)
                    {
                        var d = Dist(zero, p);
                        if (nBotsInRange > maxBotsInRange || bestPosition == null || d < bestPosition.R)
                            bestPosition = new Bot(p, d);
                        maxBotsInRange = nBotsInRange;
                    }
                }

                if (range > 1)
                {
                    xx = (bestPosition.X - range, bestPosition.X + range);
                    yy = (bestPosition.Y - range, bestPosition.Y + range);
                    zz = (bestPosition.Z - range, bestPosition.Z + range);
                    range = (long)Ceiling(range / 2.0);
                }
                else
                {
                    return bestPosition.R;
                }
            }
        }

        private List<Bot> Parse()
        {
            var regex = new Regex(@"pos=<(?<X>-?\d+),(?<Y>-?\d+),(?<Z>-?\d+)>, r=(?<R>\d+)");
            return Input.Split("\n")
                        .Select(x => regex.Match(x))
                        .Where(x => x.Success)
                        .Select(x => new Bot((long.Parse(x.Get("X")), long.Parse(x.Get("Y")), long.Parse(x.Get("Z"))), long.Parse(x.Get("R"))))
                        .ToList();
        }

        private static long Dist(Bot b1, (long X, long Y, long Z) b2) => Abs(b1.X - b2.X) + Abs(b1.Y - b2.Y) + Abs(b1.Z - b2.Z);

        private class Bot
        {
            public Bot((long x, long y, long z) p, long r) => (X, Y, Z, R) = (p.x, p.y, p.z, r);

            public long X { get; }

            public long Y { get; }

            public long Z { get; }

            public long R { get; }
        }
    }
}
