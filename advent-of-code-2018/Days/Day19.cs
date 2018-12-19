using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days
{
    /*

     */
    internal class Day19 : DayBase
    {
        public override object Part1()
        {
            var program = Parse(out int ipReg);

            int ip = 0;
            var reg = new int[6];
            reg[0] = 1;

            while (ip < program.Count && ip >= 0)
            {
                reg[ipReg] = ip;
                var instr = program[ip];

                reg = ApplyInstruction(reg, instr);

                ip = reg[ipReg] + 1;

                Console.WriteLine(string.Join(", ", reg));
            }

            return reg[0];
        }

        public override object Part2()
        {
            Console.WriteLine("TODO");
            return null;
        }

        private List<Instruction> Parse(out int ip)
        {
            ip = -1;
            var lines = Input.Split("\n");
            var program = new List<Instruction>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                if (lines[i].StartsWith("#ip"))
                {
                    ip = int.Parse(lines[i].Split()[1]);
                }
                else
                {
                    program.Add(new Instruction(lines[i]));
                }
            }

            return program;
        }

        private static int[] ApplyInstruction(int[] reg, Instruction instr)
        {
            switch (instr.OpCode)
            {
                case "addr":
                    reg[instr.C] = reg[instr.A] + reg[instr.B];
                    break;

                case "addi":
                    reg[instr.C] = reg[instr.A] + instr.B;
                    break;

                case "mulr":
                    reg[instr.C] = reg[instr.A] * reg[instr.B];
                    break;

                case "muli":
                    reg[instr.C] = reg[instr.A] * instr.B;
                    break;

                case "banr":
                    reg[instr.C] = reg[instr.A] & reg[instr.B];
                    break;

                case "bani":
                    reg[instr.C] = reg[instr.A] & instr.B;
                    break;

                case "borr":
                    reg[instr.C] = reg[instr.A] | reg[instr.B];
                    break;

                case "bori":
                    reg[instr.C] = reg[instr.A] | instr.B;
                    break;

                case "setr":
                    reg[instr.C] = reg[instr.A];
                    break;

                case "seti":
                    reg[instr.C] = instr.A;
                    break;

                case "gtir":
                    reg[instr.C] = instr.A > reg[instr.B] ? 1 : 0;
                    break;

                case "gtri":
                    reg[instr.C] = reg[instr.A] > instr.B ? 1 : 0;
                    break;

                case "gtrr":
                    reg[instr.C] = reg[instr.A] > reg[instr.B] ? 1 : 0;
                    break;

                case "eqir":
                    reg[instr.C] = instr.A == reg[instr.B] ? 1 : 0;
                    break;

                case "eqri":
                    reg[instr.C] = reg[instr.A] == instr.B ? 1 : 0;
                    break;

                case "eqrr":
                    reg[instr.C] = reg[instr.A] == reg[instr.B] ? 1 : 0;
                    break;
            }

            return reg;
        }


        private class Instruction
        {
            public Instruction(string instruction)
            {
                var spl = instruction.Split();
                OpCode = spl[0].Trim();
                A = int.Parse(spl[1]);
                B = int.Parse(spl[2]);
                C = int.Parse(spl[3]);
            }

            public string OpCode { get; }

            public int A { get; }

            public int B { get; }

            public int C { get; }
        }
    }
}
