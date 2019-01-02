using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2018.Days
{
    /*
--- Day 19: Go With The Flow ---

With the Elves well on their way constructing the North Pole base,
you turn your attention back to understanding the inner workings of programming the device.

You can't help but notice that the device's opcodes don't contain any flow control like jump instructions.
The device's manual goes on to explain:

"In programs where flow control is required, the instruction pointer can be bound to a register so that it can be manipulated directly.
This way, setr/seti can function as absolute jumps, addr/addi can function as relative jumps,
and other opcodes can cause truly fascinating effects."

This mechanism is achieved through a declaration like #ip 1,
which would modify register 1 so that accesses to it let the program indirectly access the instruction pointer itself.
To compensate for this kind of binding, there are now six registers (numbered 0 through 5);
the five not bound to the instruction pointer behave as normal.
Otherwise, the same rules apply as the last time you worked with this device.

When the instruction pointer is bound to a register, its value is written to that register just before each instruction is executed,
and the value of that register is written back to the instruction pointer immediately after each instruction finishes execution.
Afterward, move to the next instruction by adding one to the instruction pointer,
even if the value in the instruction pointer was just updated by an instruction.
(Because of this, instructions must effectively set the instruction pointer to the instruction before the one they want executed next.)

The instruction pointer is 0 during the first instruction, 1 during the second, and so on.
If the instruction pointer ever causes the device to attempt to load an instruction outside the instructions defined in the program,
the program instead immediately halts. The instruction pointer starts at 0.

It turns out that this new information is already proving useful: the CPU in the device is not very powerful,
and a background process is occupying most of its time.
You dump the background process' declarations and instructions to a file (your puzzle input),
making sure to use the names of the opcodes rather than the numbers.

For example, suppose you have the following program:

#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5

When executed, the following instructions are executed.
Each line contains the value of the instruction pointer at the time the instruction started,
the values of the six registers before executing the instructions (in square brackets), the instruction itself,
and the values of the six registers after executing the instruction (also in square brackets).

ip=0 [0, 0, 0, 0, 0, 0] seti 5 0 1 [0, 5, 0, 0, 0, 0]
ip=1 [1, 5, 0, 0, 0, 0] seti 6 0 2 [1, 5, 6, 0, 0, 0]
ip=2 [2, 5, 6, 0, 0, 0] addi 0 1 0 [3, 5, 6, 0, 0, 0]
ip=4 [4, 5, 6, 0, 0, 0] setr 1 0 0 [5, 5, 6, 0, 0, 0]
ip=6 [6, 5, 6, 0, 0, 0] seti 9 0 5 [6, 5, 6, 0, 0, 9]

In detail, when running this program, the following events occur:

    The first line (#ip 0) indicates that the instruction pointer should be bound to register 0 in this program.
        This is not an instruction, and so the value of the instruction pointer does not change during the processing of this line.
    The instruction pointer contains 0, and so the first instruction is executed (seti 5 0 1).
        It updates register 0 to the current instruction pointer value (0), sets register 1 to 5,
        sets the instruction pointer to the value of register 0 (which has no effect, as the instruction did not modify register 0),
        and then adds one to the instruction pointer.
    The instruction pointer contains 1, and so the second instruction, seti 6 0 2, is executed.
        This is very similar to the instruction before it: 6 is stored in register 2, and the instruction pointer is left with the value 2.
    The instruction pointer is 2, which points at the instruction addi 0 1 0.
        This is like a relative jump: the value of the instruction pointer, 2, is loaded into register 0.
        Then, addi finds the result of adding the value in register 0 and the value 1, storing the result, 3, back in register 0.
        Register 0 is then copied back to the instruction pointer,
        which will cause it to end up 1 larger than it would have otherwise and skip the next instruction (addr 1 2 3) entirely.
        Finally, 1 is added to the instruction pointer.
    The instruction pointer is 4, so the instruction setr 1 0 0 is run.
        This is like an absolute jump: it copies the value contained in register 1, 5,
        into register 0, which causes it to end up in the instruction pointer.
        The instruction pointer is then incremented, leaving it at 6.
    The instruction pointer is 6, so the instruction seti 9 0 5 stores 9 into register 5.
        The instruction pointer is incremented, causing it to point outside the program, and so the program ends.

What value is left in register 0 when the background process halts?

--- Part Two ---

A new background process immediately spins up in its place. It appears identical, but on closer inspection, you notice that this time, register 0 started with the value 1.

What value is left in register 0 when this new background process halts?

     */
    internal class Day19 : DayBase
    {
        public override object Part1()
        {
            var program = Parse(out int ipReg, Input);
            var reg = new int[6];

            while (reg[ipReg] < program.Count)
            {
                reg = ApplyInstruction(reg, program[reg[ipReg]]);
                reg[ipReg]++;
            }

            return reg[0];
        }

        public override object Part2()
        {
            var program = Parse(out int ipReg, Input);

            var reg = new int[6];
            reg[0] = 1;

            while (reg[ipReg] != 1)
            {
                reg = ApplyInstruction(reg, program[reg[ipReg]]);
                reg[ipReg]++;
            }

            int num = reg.Max();
            return Enumerable.Range(1, num).Where(x => num % x == 0).Sum();

            /*
0    addi 2 16  2		IP += 16			goto 17

1    seti 1  1  1		R1 = 1				R1 = 1
2    seti 1  8  5		R5 = 1				R5 = 1

3    mulr 1  5  4		R4 = R1 * R5		R4 = R1 * R5	|
4    eqrr 4  3  4		R4 = R4 == R3		| if R3 == R4	|
5    addr 4  2  2		IP += R4			|     goto 7	|
6    addi 2  1  2		IP += 1				| else goto 8	| if R1 * R5 == R3
7    addr 1  0  0		R0 += R1							|	R0 += R1

8    addi 5  1  5		R5 += 1				R5 += 1
9    gtrr 5  3  4		R4 = R5 > R3		| if R5 > R3	|
10   addr 2  4  2		IP += R4			|     goto 12	| if R5 <= R3
11   seti 2  0  2		IP = 2				goto 3			|	goto 3

12   addi 1  1  1		R1 += 1				R1 += 1
13   gtrr 1  3  4		R4 = R1 > R3		| if R1 > R3	|
14   addr 4  2  2		IP += R4			|     goto 16	| if R1 <= R3
15   seti 1  1  2		IP = 1				goto 2			|	goto 2
16   mulr 2  2  2		IP = IP * IP		IP = 16 * 16  	| else END


17   addi 3  2  3		R3 += 2				R3 = 2
18   mulr 3  3  3		R3 = R3 * R3		R3 = (2 * 2)
19   mulr 2  3  3		R3 = IP * R3		R3 = (2 * 2 * 19)
20   muli 3 11  3		R3 = R3 * 11		R3 = (2 * 2 * 19 * 11) = 836
21   addi 4  7  4		R4 += 7				R4 = 7
22   mulr 4  2  4		R4 = IP * R4		R4 = (7 * 22)
23   addi 4  6  4		R4 += 6				R4 = (7 * 22) + 6 = 160
24   addr 3  4  3		R3 += R4			R3 = 836 + 160 = 996

25   addr 2  0  2		IP += R0			| if part 1
26   seti 0  3  2		IP = 0				|     goto 1

27   setr 2  0  4		R4 = IP				R4 = 27
28   mulr 4  2  4		R4 = R4 * IP		R4 = 27 * 28
29   addr 2  4  4		R4 += IP			R4 = 27 * 28 + 29
30   mulr 2  4  4		R4 = IP * R4		R4 = (27 * 28 + 29) * 30
31   muli 4 14  4		R4 = R4 * 14		R4 = (27 * 28 + 29) * 30 * 14
32   mulr 4  2  4		R4 = R4 * IP		R4 = (27 * 28 + 29) * 30 * 14 * 32 = 10550400
33   addr 3  4  3		R3 += R4			R3 = 996 + 10550400 = 10551396

34   seti 0  4  0		R0 = 0				R0 = 0
35   seti 0  4  2		IP = 0				goto 1


if part 1
	R3 = 996
else
	R3 = 10551396
	R0 = 0
	
R1 = 1
while R1 <= R3
	R5 = 1
	while R5 <= R3
		R5 += 1
		if R1 * R5 == R3
			R0 += R1
	R1 += 1
             */
        }

        public static List<Instruction> Parse(out int ip, string input)
        {
            ip = -1;
            var lines = input.Split("\n");
            var program = new List<Instruction>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("#ip"))
                    ip = int.Parse(line.Split()[1]);
                else
                    program.Add(new Instruction(line));
            }

            return program;
        }

        public static int[] ApplyInstruction(int[] reg, Instruction instr)
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


        public class Instruction
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
