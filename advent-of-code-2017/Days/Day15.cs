using System;

namespace AdventOfCode2017.Days
{
    internal class Day15 : IDay
    {
        public void Part1(string input)
        {
            var spl = input.Split(' ');
            long genA = int.Parse(spl[0]), genB = int.Parse(spl[1]);

            long result = 0;
            for (int i = 0; i < 40_000_000; i++)
            {
                genA = GeneratorStep(genA, true);
                genB = GeneratorStep(genB, false);

                if ((genA & 0xFFFF) == (genB & 0xFFFF))
                    result++;
            }

            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            var spl = input.Split(' ');
            long genA = int.Parse(spl[0]), genB = int.Parse(spl[1]);

            long result = 0;
            for (int i = 0; i < 5_000_000; i++)
            {
                do genA = GeneratorStep(genA, true);
                while (genA % 4 != 0);

                do genB = GeneratorStep(genB, false);
                while (genB % 8 != 0);

                if ((genA & 0xFFFF) == (genB & 0xFFFF))
                    result++;
            }

            Console.WriteLine("Result: " + result);
        }

        long GeneratorStep(long val, bool isA)
        {
            long factor = isA ? 16807 : 48271;
            return val * factor % 2147483647;
        }
    }
}
