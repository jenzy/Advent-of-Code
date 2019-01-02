using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2017.Days
{
    internal class Day9 : IDay
    {
        public void Part1(string input) => Console.WriteLine("Result: " + Parse(input).totalScore);

        public void Part2(string input) => Console.WriteLine("Result: " + Parse(input).garbage);

        private static (int totalScore, int garbage) Parse(string input)
        {
            bool inGarbage = false;
            int totalScore = 0, garbage = 0;
            var groups = new Stack<int>();
            groups.Push(0);

            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '{' when !inGarbage:
                        groups.Push(groups.Peek() + 1);
                        break;

                    case '}' when !inGarbage:
                        totalScore += groups.Pop();
                        break;

                    case '<' when !inGarbage:
                        inGarbage = true;
                        break;

                    case '>' when inGarbage:
                        inGarbage = false;
                        break;

                    case '!' when inGarbage:
                        i++;
                        break;

                    case char _ when inGarbage:
                        garbage++;
                        break;
                }
            }

            return (totalScore, garbage);
        }
    }
}
