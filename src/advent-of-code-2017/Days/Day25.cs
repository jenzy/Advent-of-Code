using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day25 : IDay
    {
        public void Part1(string input)
        {
            var (state, nSteps, rules) = Parse(input);
            var tape = new Dictionary<int, int>();
            int position = 0;

            for (int i = 0; i < nSteps; i++)
            {
                tape.TryGetValue(position, out int val);
                var rule = rules[state][val];
                tape[position] = rule.newValue;
                position += rule.move;
                state = rule.newState;
            }

            Console.WriteLine("Result: " + tape.Values.Count(x => x == 1));
        }

        public void Part2(string input)
        {
        }

        private static (char startState, int nSteps, RuleDictionary rules) Parse(string input)
        {
            var lines = input.Split('\n').Select(x => x.TrimEnd()).ToList();
            var rules = new RuleDictionary();

            void ParseState(int start)
            {
                var dict = new Dictionary<int, (int newValue, int move, char newState)>();
                foreach (int i in new[] { 1, 5 })
                {
                    var value = lines[start + i][lines[start + i].Length - 2] - '0';
                    var newValue = lines[start + i + 1][lines[start + i + 1].Length - 2] - '0';
                    var move = lines[start + i + 2].EndsWith("left.") ? -1 : 1;
                    var newState = lines[start + i + 3][lines[start + i + 3].Length - 2];
                    dict[value] = (newValue, move, newState);
                }
                rules[lines[start][9]] = dict;
            }

            for (int i = 3; i < lines.Count; i++)
                if (lines[i].StartsWith("In state "))
                    ParseState(i);

            char startState = lines[0][lines[0].IndexOf('.') - 1];
            int iAfter = lines[1].IndexOf("after", StringComparison.Ordinal);
            int iSteps = lines[1].IndexOf(" steps", StringComparison.Ordinal);
            int nSteps = int.Parse(lines[1].Substring(iAfter + 6, iSteps - iAfter - 6));

            return (startState, nSteps, rules);
        }

        private class RuleDictionary : Dictionary<char, Dictionary<int, (int newValue, int move, char newState)>>
        {
        }
    }
}
