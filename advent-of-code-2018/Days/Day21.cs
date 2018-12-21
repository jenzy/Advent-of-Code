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
