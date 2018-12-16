using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days
{
    /*
--- Day 16: Chronal Classification ---

As you see the Elves defend their hot chocolate successfully, you go back to falling through time.
This is going to become a problem.

If you're ever going to return to your own time, you need to understand how this device on your wrist works.
You have a little while before you reach your next destination, and with a bit of trial and error,
you manage to pull up a programming manual on the device's tiny screen.

According to the manual, the device has four registers (numbered 0 through 3)
that can be manipulated by instructions containing one of 16 opcodes. The registers start with the value 0.

Every instruction consists of four values: an opcode, two inputs (named A and B), and an output (named C), in that order.
The opcode specifies the behavior of the instruction and how the inputs are interpreted.
The output, C, is always treated as a register.

In the opcode descriptions below, if something says "value A", it means to take the number given as A literally.
(This is also called an "immediate" value.) If something says "register A", it means to use the number given as A to read from (or write to) the register with that number. So, if the opcode addi adds register A and value B, storing the result in register C, and the instruction addi 0 7 3 is encountered, it would add 7 to the value contained by register 0 and store the sum in register 3, never modifying registers 0, 1, or 2 in the process.

Many opcodes are similar except for how they interpret their arguments. The opcodes fall into seven general categories:

Addition:

    addr (add register) stores into register C the result of adding register A and register B.
    addi (add immediate) stores into register C the result of adding register A and value B.

Multiplication:

    mulr (multiply register) stores into register C the result of multiplying register A and register B.
    muli (multiply immediate) stores into register C the result of multiplying register A and value B.

Bitwise AND:

    banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
    bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.

Bitwise OR:

    borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
    bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.

Assignment:

    setr (set register) copies the contents of register A into register C. (Input B is ignored.)
    seti (set immediate) stores value A into register C. (Input B is ignored.)

Greater-than testing:

    gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B.
        Otherwise, register C is set to 0.
    gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B.
        Otherwise, register C is set to 0.
    gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B.
        Otherwise, register C is set to 0.

Equality testing:

    eqir (equal immediate/register) sets register C to 1 if value A is equal to register B.
        Otherwise, register C is set to 0.
    eqri (equal register/immediate) sets register C to 1 if register A is equal to value B.
        Otherwise, register C is set to 0.
    eqrr (equal register/register) sets register C to 1 if register A is equal to register B.
        Otherwise, register C is set to 0.

Unfortunately, while the manual gives the name of each opcode, it doesn't seem to indicate the number.
However, you can monitor the CPU to see the contents of the registers
before and after instructions are executed to try to work them out.
Each opcode has a number from 0 through 15, but the manual doesn't say which is which.
For example, suppose you capture the following sample:

Before: [3, 2, 1, 1]
9 2 1 2
After:  [3, 2, 2, 1]

This sample shows the effect of the instruction 9 2 1 2 on the registers.
Before the instruction is executed, register 0 has value 3, register 1 has value 2, and registers 2 and 3 have value 1.
After the instruction is executed, register 2's value becomes 2.

The instruction itself, 9 2 1 2, means that opcode 9 was executed with A=2, B=1, and C=2.
Opcode 9 could be any of the 16 opcodes listed above, but only three of them behave in a way that would cause the result shown in the sample:

    Opcode 9 could be mulr: register 2 (which has a value of 1) times register 1 (which has a value of 2) produces 2,
        which matches the value stored in the output register, register 2.
    Opcode 9 could be addi: register 2 (which has a value of 1) plus value 1 produces 2,
        which matches the value stored in the output register, register 2.
    Opcode 9 could be seti: value 2 matches the value stored in the output register, register 2;
        the number given for B is irrelevant.

None of the other opcodes produce the result captured in the sample.
Because of this, the sample above behaves like three opcodes.

You collect many of these samples (the first section of your puzzle input).
The manual also includes a small test program (the second section of your puzzle input) - you can ignore it for now.

Ignoring the opcode numbers, how many samples in your puzzle input behave like three or more opcodes?

--- Part Two ---

Using the samples you collected, work out the number of each opcode and execute the test program
(the second section of your puzzle input).

What value is contained in register 0 after executing the test program?
     */
    internal class Day16 : DayBase
    {
        private static readonly OpCode[] OpCodes = Enum.GetValues(typeof(OpCode)).Cast<OpCode>().ToArray();

        public override object Part1() => Parse().samples.Count(CouldBeThreeOrMoreOpCodes);

        public override object Part2()
        {
            var (samples, program) = Parse();

            var unknownOps = OpCodes.ToHashSet();
            var opcodeMap = new Dictionary<int, OpCode>();

            while (unknownOps.Any())
            {
                foreach (var sample in samples.Where(s => !opcodeMap.ContainsKey(s.Instruction.OpCode)))
                {
                    var candidate = GetOpCodeCandidate(sample, unknownOps);
                    if (candidate != null)
                    {
                        opcodeMap[sample.Instruction.OpCode] = candidate.Value;
                        unknownOps.Remove(candidate.Value);
                    }
                }
            }

            var reg = new int[4];
            foreach (var instr in program)
                ApplyInstruction(reg, instr, opcodeMap[instr.OpCode]);
            return reg[0];
        }

        private (List<Sample> samples, List<Instruction> program) Parse()
        {
            var lines = Input.Split("\n");
            var samples = new List<Sample>();
            var program = new List<Instruction>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                if (lines[i].StartsWith("Before"))
                {
                    samples.Add(new Sample
                    {
                        RegBefore = lines[i].Substring(lines[i].IndexOf('[') + 1).TrimEnd(']').Split(',').Select(int.Parse).ToArray(),
                        Instruction = new Instruction(lines[i + 1]),
                        RegAfter = lines[i + 2].Substring(lines[i + 2].IndexOf('[') + 1).TrimEnd(']').Split(',').Select(int.Parse).ToArray()
                    });
                    i += 2;
                }
                else
                {
                    program.Add(new Instruction(lines[i]));
                }
            }

            return (samples, program);
        }

        private static int[] ApplyInstruction(int[] reg, Instruction instr, OpCode op)
        {
            switch (op)
            {
                case OpCode.AddR:
                    reg[instr.C] = reg[instr.A] + reg[instr.B];
                    break;

                case OpCode.AddI:
                    reg[instr.C] = reg[instr.A] + instr.B;
                    break;

                case OpCode.MulR:
                    reg[instr.C] = reg[instr.A] * reg[instr.B];
                    break;

                case OpCode.MulI:
                    reg[instr.C] = reg[instr.A] * instr.B;
                    break;

                case OpCode.BanR:
                    reg[instr.C] = reg[instr.A] & reg[instr.B];
                    break;

                case OpCode.BanI:
                    reg[instr.C] = reg[instr.A] & instr.B;
                    break;

                case OpCode.BorR:
                    reg[instr.C] = reg[instr.A] | reg[instr.B];
                    break;

                case OpCode.BorI:
                    reg[instr.C] = reg[instr.A] | instr.B;
                    break;

                case OpCode.SetR:
                    reg[instr.C] = reg[instr.A];
                    break;

                case OpCode.SetI:
                    reg[instr.C] = instr.A;
                    break;

                case OpCode.GtIR:
                    reg[instr.C] = instr.A > reg[instr.B] ? 1 : 0;
                    break;

                case OpCode.GtRI:
                    reg[instr.C] = reg[instr.A] > instr.B ? 1 : 0;
                    break;

                case OpCode.GtRR:
                    reg[instr.C] = reg[instr.A] > reg[instr.B] ? 1 : 0;
                    break;

                case OpCode.EqIR:
                    reg[instr.C] = instr.A == reg[instr.B] ? 1 : 0;
                    break;

                case OpCode.EqRI:
                    reg[instr.C] = reg[instr.A] == instr.B ? 1 : 0;
                    break;

                case OpCode.EqRR:
                    reg[instr.C] = reg[instr.A] == reg[instr.B] ? 1 : 0;
                    break;
            }

            return reg;
        }

        private static bool CouldBeThreeOrMoreOpCodes(Sample sample)
        {
            int count = 0;
            foreach (OpCode op in OpCodes)
            {
                var reg = ApplyInstruction(sample.RegBefore.ToArray(), sample.Instruction, op);
                if (reg.SequenceEqual(sample.RegAfter) && ++count >= 3)
                    return true;
            }
            return false;
        }

        private static OpCode? GetOpCodeCandidate(Sample sample, IEnumerable<OpCode> unknownOps)
        {
            OpCode? possibleOp = null;
            foreach (OpCode op in unknownOps)
            {
                var reg = ApplyInstruction(sample.RegBefore.ToArray(), sample.Instruction, op);
                if (reg.SequenceEqual(sample.RegAfter))
                {
                    if (possibleOp != null)
                        return null;
                    possibleOp = op;
                }
            }

            return possibleOp;
        }

        private enum OpCode { AddR, AddI, MulR, MulI, BanR, BanI, BorR, BorI, SetR, SetI, GtIR, GtRI, GtRR, EqIR, EqRI, EqRR }

        private class Instruction
        {
            public Instruction(string instruction)
            {
                var spl = instruction.Split();
                OpCode = int.Parse(spl[0]);
                A = int.Parse(spl[1]);
                B = int.Parse(spl[2]);
                C = int.Parse(spl[3]);
            }

            public int OpCode { get; }

            public int A { get; }

            public int B { get; }

            public int C { get; }
        }

        private class Sample
        {
            public int[] RegBefore { get; set; }

            public int[] RegAfter { get; set; }

            public Instruction Instruction { get; set; }
        }
    }
}
