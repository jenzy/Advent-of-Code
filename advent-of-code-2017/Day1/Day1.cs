using System;

namespace AdventOfCode2017
{
    public class Day1
    {
        public void Part1(string input)
        {
            int sum = 0;

            for(int i = 0; i<input.Length; i++)
            {
                if (input[i] == input[(i + 1) % input.Length])
                    sum += (int)char.GetNumericValue(input[i]);
            }

            Console.WriteLine("Result: " + sum);
        }

        public void Part2(string input)
        {
            int sum = 0;
            int offset = input.Length / 2;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == input[(i + offset) % input.Length])
                    sum += (int)char.GetNumericValue(input[i]);
            }

            Console.WriteLine("Result: " + sum);
        }
    }
}
