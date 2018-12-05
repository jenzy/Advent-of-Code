using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Days
{
    /*


     */
    internal class Day05 : IDay
    {
        public string Part1(string input)
        {
            return Reduce(input.ToCharArray()).ToString();
        }

        public string Part2(string input)
        {
            var letters = input.Select(char.ToUpper).Distinct();

            var min = int.MaxValue;

            foreach (var letter in letters)
            {
                var tmp = Reduce(input.Where(c => char.ToUpper(c) != letter));
                min = Math.Min(tmp, min);
            }

            return min.ToString();
        }

        private static int Reduce(IEnumerable<char> data)
        {
            var stack = new Stack<char>();

            foreach (var newChar in data)
            {
                if (stack.TryPeek(out char prevChar)
                    && char.ToUpperInvariant(prevChar) == char.ToUpperInvariant(newChar)
                    && char.IsUpper(prevChar) != char.IsUpper(newChar))
                {
                    stack.Pop();
                }
                else
                {
                    stack.Push(newChar);
                }
            }

            return stack.Count;
        }
    }
}
