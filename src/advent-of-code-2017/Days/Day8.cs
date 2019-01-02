using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day8 : IDay
    {
        public void Part1(string input)
        {
            var instructions = Parse(input);

            var regs = new Dictionary<string, int>();
            foreach (var instruction in instructions)
            {
                if (instruction.Condition(regs))
                    instruction.Apply(regs);
            }

            var result = regs.Values.Max();
            Console.WriteLine("Result: " + result);
        }

        public void Part2(string input)
        {
            var instructions = Parse(input);

            var regs = new Dictionary<string, int>();
            int max = 0;
            foreach (var instruction in instructions)
            {
                if (instruction.Condition(regs))
                    instruction.Apply(regs);

                max = Math.Max(max, regs.Values.Max());
            }

            Console.WriteLine("Result: " + max);
        }

        private List<Instruction> Parse(string input) => input.Split('\n').Select(x => new Instruction(x.Trim())).ToList();

        private class Instruction
        {
            public Instruction(string input)
            {
                var ar = input.Split(' ').Select(x => x.Trim()).ToList();
                Register = ar[0];
                InstrType = ar[1];
                Amount = int.Parse(ar[2]);
                CondRegister = ar[4];
                CondType = ar[5];
                CondAmount = int.Parse(ar[6]);
            }

            private string Register { get; }

            private string InstrType { get; }

            private int Amount { get; }

            private string CondRegister { get; }

            private string CondType { get; }

            private int CondAmount { get; }

            public bool Condition(Dictionary<string, int> regs)
            {
                regs.TryGetValue(CondRegister, out int val);

                switch (CondType)
                {
                    case ">":
                        return val > CondAmount;

                    case "<":
                        return val < CondAmount;

                    case ">=":
                        return val >= CondAmount;

                    case "==":
                        return val == CondAmount;

                    case "!=":
                        return val != CondAmount;

                    case "<=":
                        return val <= CondAmount;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public void Apply(Dictionary<string, int> regs)
            {
                regs.TryGetValue(Register, out int val);

                switch (InstrType)
                {
                    case "inc":
                        val += Amount;
                        break;

                    case "dec":
                        val -= Amount;
                        break;
                }

                regs[Register] = val;
            }
        }
    }
}

