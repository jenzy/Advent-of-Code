using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2017.Days
{
    internal class Day17 : IDay
    {
        public void Part1(string input)
        {
            int steps = int.Parse(input);
            var list = new LinkedList<int>();
            list.AddFirst(0);
            var current = list.First;

            void Insert(int value)
            {
                for (int i = 0; i < steps; i++)
                    current = current.Next ?? list.First;

                current = list.AddAfter(current, value);
            }

            for (int i = 1; i <= 2017; i++)
                Insert(i);

            Console.WriteLine("Result: " + current.Next.Value);
        }

        public void Part2(string input)
        {
            int current = 0, result = 0, steps = int.Parse(input);

            for (int i = 1; i <= 50_000_000; i++)
            {
                current = (current + steps) % i + 1;
                if (current == 1)
                    result = i;
            }

            Console.WriteLine("Result: " + result);
        }
    }
}
