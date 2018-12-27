using System.Collections.Generic;

namespace AdventOfCode2018.Days
{
    /*
--- Day 21: Chronal Conversion ---

You should have been watching where you were going, because as you wander the new North Pole base, you trip and fall into a very deep hole!

Just kidding. You're falling through time again.

If you keep up your current pace, you should have resolved all of the temporal anomalies by the next time the device activates.
Since you have very little interest in browsing history in 500-year increments for the rest of your life,
you need to find a way to get back to your present time.

After a little research, you discover two important facts about the behavior of the device:

First, you discover that the device is hard-wired to always send you back in time in 500-year increments.
Changing this is probably not feasible.

Second, you discover the activation system (your puzzle input) for the time travel module.
Currently, it appears to run forever without halting.

If you can cause the activation system to halt at a specific moment
maybe you can make the device send you so far back in time that you cause an integer underflow in time itself and wrap around back to your current time!

The device executes the program as specified in manual section one and manual section two.

Your goal is to figure out how the program works and cause it to halt.
You can only control register 0; every other register begins at 0 as usual.

Because time travel is a dangerous activity,
the activation system begins with a few instructions which verify that bitwise AND (via bani) does a numeric operation and not an operation
as if the inputs were interpreted as strings. If the test fails, it enters an infinite loop re-running the test instead of allowing the
program to execute normally. If the test passes, the program continues, and assumes that all other bitwise operations (banr, bori, and borr)
also interpret their inputs as numbers. (Clearly, the Elves who wrote this system were worried that someone might introduce a
bug while trying to emulate this system with a scripting language.)

What is the lowest non-negative integer value for register 0 that causes the program to halt after executing the fewest instructions?
(Executing the same instruction multiple times counts as multiple instructions executed.)

--- Part Two ---

In order to determine the timing window for your underflow exploit, you also need an upper bound:

What is the lowest non-negative integer value for register 0 that causes the program to halt after executing the most instructions?
(The program must actually halt; running forever does not count as halting.)
     */
    internal class Day21 : DayBase
    {
        public override object Part1()
        {
            var program = Day19.Parse(out int ipReg, Input);
            var reg = new int[6];

            while (reg[ipReg] < program.Count)
            {
                if (reg[ipReg] == 28)
                    return reg[4];

                reg = Day19.ApplyInstruction(reg, program[reg[ipReg]]);
                reg[ipReg]++;
            }

            return reg[0];
        }

        public override object Part2()
        {
            int r4 = 0;
            var set = new HashSet<long>();
            int last = -1;

            while (true)
            {
                int r1 = r4 | 65536;
                r4 = 678134;
                while (true)
                {
                    r4 = (((r4 + (r1 & 255)) & 16777215) * 65899) & 16777215;
                    if (256 > r1)
                    {
                        if (set.Contains(r4))
                            return last;
                        last = r4;
                        set.Add(last);
                        break;
                    }

                    r1 = r1 / 256;
                }
            }

            /*
            var program = Day19.Parse(out int ipReg, Input);
            var reg = new int[6];

            while (reg[ipReg] < program.Count)
            {
                if (reg[ipReg] == 28)
                {
                    if (set.Contains(reg[4]))
                        return last;
                    last = reg[4];
                    set.Add(last);
                }

                reg = Day19.ApplyInstruction(reg, program[reg[ipReg]]);
                reg[ipReg]++;
            }
            */
        }
    }
}
/*
#ip 3

 0  seti 123 0 4			
 1  bani 4 456 4
 2  eqri 4 72 4
 3  addr 4 3 3
 4  seti 0 0 3			goto 1
 
 5  seti 0 6 4			R4 = 0
 6  bori 4 65536 1		R1 = R4 | 65536
 7  seti 678134 1 4		R4 = 678134
 
 8  bani 1 255 5		R5 = R1 & 255
 9  addr 4 5 4			R4 += R5
10  bani 4 16777215 4	R4 = R4 & 16777215
11  muli 4 65899 4		R4 = R4 * 65899
12  bani 4 16777215 4	R4 = R4 & 16777215
13  gtir 256 1 5		R5 = 256 > R1		| if 256 > R1		| 
14  addr 5 3 3			IP += R5 // 1/0		|    goto 16		|
15  addi 3 1 3			IP += 1				| else goto 17		|  if 256 > R1  
16  seti 27 8 3			IP = 27				|					|	 goto 28

17  seti 0 1 5			R5 = 0
18  addi 5 1 2			R2 = R5 + 1
19  muli 2 256 2		R2 = R2 * 256
20  gtrr 2 1 2			R2 = R2 > R1		| if R2 > R1		|
21  addr 2 3 3			IP += R2			|   goto 23			| 
22  addi 3 1 3			IP += 1				|  else goto 24		| if R2 > R1
23  seti 25 7 3			R3 = 25				|					|    goto 26

24  addi 5 1 5			R5 += 1
25  seti 17 1 3			goto 18			

26  setr 5 3 1			R1 = R5
27  seti 7 8 3			goto 8

28  eqrr 4 0 5			R5 = R4 == R0		| if R4 == R0
29  addr 5 3 3			IP += R5			|   goto 31 END
30  seti 5 4 3								| else goto 6

 */
