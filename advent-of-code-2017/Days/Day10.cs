using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2017.Days
{
    internal class Day10 : IDay
    {
        public void Part1(string input)
        {
            var list = new CircularList();
            var lengths = input.Split(',').Select(x => int.Parse(x.Trim())).ToList();

            int iCur = 0, skip = 0;
            foreach (int length in lengths)
            {
                var tmpList = list.GetRange(iCur, length).Reverse().ToList();
                for (int l = 0; l < length; l++)
                    list[iCur + l] = tmpList[l];

                iCur += length + skip;
                skip++;
            }

            Console.WriteLine("Result: " + list[0]*list[1]);
        }

        public void Part2(string input)
        {
            var list = new CircularList();
            var lengths = Encoding.ASCII.GetBytes(input).Concat(new byte[]{ 17, 31, 73, 47, 23 }).ToList();

            int iCur = 0, skip = 0;
            for (int round = 0; round < 64; round++)
            {
                foreach (int length in lengths)
                {
                    var tmpList = list.GetRange(iCur, length).Reverse().ToList();
                    for (int l = 0; l < length; l++)
                        list[iCur + l] = tmpList[l];

                    iCur += length + skip;
                    skip++;
                }
            }

            var hash = "";
            for (int i = 0; i < 16; i++)
                hash += list.GetRange(i * 16, 16).Aggregate(0, (a, b) => a ^ b).ToString("x2");

            Console.WriteLine("Result: " + hash);
        }

        private class CircularList
        {
            private List<int> List { get; } = Enumerable.Range(0, 256).ToList();

            public int this[int i]
            {
                get => List[i % List.Count];
                set => List[i % List.Count] = value;
            }

            public IEnumerable<int> GetRange(int iStart, int length)
            {
                for (int i = 0; i < length; i++)
                    yield return this[iStart + i];
            }
        }
    }
}
