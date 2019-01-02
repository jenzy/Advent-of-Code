using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day20 : IDay
    {
        public void Part1(string input)
        {
            var particles = Parse(input);

            (int i, int count) result = (0, 0);
            while (result.count < 200)
            {
                (int i, long d) min = (0, long.MaxValue);

                for (int i = 0; i < particles.Count; i++)
                {
                    var d = particles[i].Step();
                    if (d < min.d)
                        min = (i, d);
                }

                if (result.i == min.i)
                    result.count++;
                else
                    result = (min.i, 1);
            }

            Console.WriteLine("Result: " + result.i);
        }

        public void Part2(string input)
        {
            var particles = Parse(input);

            (int res, int count) result = (0, 0);
            while (result.count < 200)
            {
                var dict = new Dictionary<(long x, long y, long z), int>();

                for (int i = 0; i < particles.Count; i++)
                {
                    if (!particles[i].Alive)
                        continue;

                    particles[i].Step();

                    if (dict.TryGetValue(particles[i].X, out int iOther))
                    {
                        particles[i].Alive = false;
                        particles[iOther].Alive = false;
                    }
                    else
                    {
                        dict[particles[i].X] = i;
                    }
                }

                var aliveCount = particles.Count(p => p.Alive);
                if (result.res == aliveCount)
                    result.count++;
                else
                    result = (aliveCount, 1);
            }

            Console.WriteLine("Result: " + result.res);
        }

        private List<Particle> Parse(string input) => input.Split('\n').Select(x => x.TrimEnd()).Select(line => new Particle(line)).ToList();

        private class Particle
        {
            private readonly long[] x;
            private readonly long[] v;
            private readonly long[] a;

            public Particle(string input)
            {
                var spl = input.Replace("p=", "").Replace("v=", "").Replace("a=", "").Split(", ");
                x = Parse(spl[0]);
                v = Parse(spl[1]);
                a = Parse(spl[2]);

                long[] Parse(string str)
                {
                    var spl1 = str.TrimStart('<').TrimEnd('>').Split(',');
                    return new[] { long.Parse(spl1[0]), long.Parse(spl1[1]), long.Parse(spl1[2]) };
                }
            }

            public bool Alive { get; set; } = true;

            public (long x, long y, long z) X => (x[0], x[1], x[2]);

            public long Step()
            {
                for (int j = 0; j < 3; j++)
                {
                    v[j] += a[j];
                    x[j] += v[j];
                }

                return x.Select(Math.Abs).Sum();
            }
        }
    }
}
