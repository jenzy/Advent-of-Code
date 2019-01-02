using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day6 : IDay
    {
        public void Part1(string input)
        {
            var banks = new Mem(input);
            var set = new HashSet<string> { banks.ToString() };
            int result = 0;

            while (true)
            {
                var mx = banks.GetMax();
                banks.Data[mx.ix] = 0;
                int each = mx.max / banks.N;
                if (each > 0)
                    banks.AddToEach(each);
                int rem = mx.max % banks.N;
                if (rem > 0)
                    banks.Distribute(mx.ix+1, rem);

                result++;
                var str = banks.ToString();
                if (set.Contains(str))
                    break;
                set.Add(str);
            }

            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            var banks = new Mem(input);
            var set = new Dictionary<string, int> { { banks.ToString(), 0 } };
            int count = 0, result;

            while (true)
            {
                var mx = banks.GetMax();
                banks.Data[mx.ix] = 0;
                int each = mx.max / banks.N;
                if (each > 0)
                    banks.AddToEach(each);
                int rem = mx.max % banks.N;
                if (rem > 0)
                    banks.Distribute(mx.ix + 1, rem);

                count++;

                
                var str = banks.ToString();
                if (set.TryGetValue(str, out int prev))
                {
                    result = count - prev;
                    break;
                }

                set.Add(str, count);
            }

            Console.WriteLine("Result: " + result);
        }

        class Mem
        {
            public readonly int[] Data;

            public Mem(string input)
            {
                Data = input.Split('\t').Select(x => int.Parse(x.Trim())).ToArray();
            }

            public int N => Data.Length;

            public (int max, int ix) GetMax()
            {
                var max = Data.Max();
                var ix = Array.IndexOf(Data, max);
                return(max, ix);
            }

            public void AddToEach(int val)
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    Data[i] += val;
                }
            }

            public void Distribute(int iStart, int val)
            {
                for (int c = 0; c < val; c++)
                {
                    Data[(iStart + c) % N]++;
                }
            }

            public override string ToString()
            {
                return string.Join(',', Data);
            }
        }
    }
}
