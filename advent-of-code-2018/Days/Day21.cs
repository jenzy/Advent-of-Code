using System;
using System.Linq;

namespace AdventOfCode2018.Days
{
    /*

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
                    return reg[4]; // 10961197

                reg = Day19.ApplyInstruction(reg, program[reg[ipReg]]);
                reg[ipReg]++;
            }

            return reg[0];
        }

        public override object Part2()
        {
            var program = Day19.Parse(out int ipReg, Input);
            var reg = new int[6];

            while (reg[ipReg] < program.Count)
            {
                if (reg[ipReg] == 8)
                {
                    Console.WriteLine(reg[1]);
                    reg[5] = reg[1];
                    reg[ipReg]++;
                    continue;
                }

                if (reg[ipReg] == 28)
                    reg = reg;
                //    Console.WriteLine(reg[1]);

                reg = Day19.ApplyInstruction(reg, program[reg[ipReg]]);
                reg[ipReg]++;
            }

            return reg[0];
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
