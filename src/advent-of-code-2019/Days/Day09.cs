using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using AdventOfCode.Common;

namespace AdventOfCode.Y2019.Days
{
    /*

     */

    public class Day09 : DayBase
    {
        public override object Part1() => string.Join(", ", new Intcode(Parse(Input), Enumerable.Repeat(1L, 1)).Run().Output);

        public override object Part2() => string.Join(", ", new Intcode(Parse(Input), Enumerable.Repeat(2L, 1)).Run().Output);

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(9);
            Assert.Equal(3454977209, day.Part1());
            Assert.Equal(50102, day.Part2());
        }


        public enum IntcodeState { Ready, PendingInput, Done }

        public class Intcode
        {
            private const int OpAdd = 1;
            private const int OpMul = 2;
            private const int OpInput = 3;
            private const int OpOutput = 4;
            private const int OpJumpIfTrue = 5;
            private const int OpJumpIfFalse = 6;
            private const int OpLessThan = 7;
            private const int OpEquals = 8;
            private const int OpRel = 9;
            private const int OpExit = 99;

            private int pc = 0;
            private long rb = 0;
            private readonly IDictionary<int, long> data;
            private Queue<long> input;

            public Intcode(IEnumerable<long> memory, IEnumerable<long> input = null)
            {
                this.data = memory.Select((x, ix) => (x, ix)).ToDictionary(x => x.ix, x => x.x);
                this.input = input as Queue<long> ?? new Queue<long>(input ?? Enumerable.Empty<long>());
                this.Output = new Queue<long>();
            }

            public bool PrintOutput { get; } = false;

            public IntcodeState State { get; private set; } = IntcodeState.Ready;

            public long SimpleOutput => data[0];

            public Queue<long> Output { get; }

            private int CurrentOpcodeFull => (int)data[pc];

            private int CurrentOpcode => CurrentOpcodeFull % 100;

            public void SetInput(Queue<long> customInput)
            {
                this.input = customInput;
            }

            public Intcode Run()
            {
                while (true)
                {
                    int opcode = CurrentOpcode;
                    if (opcode == OpExit)
                    {
                        State = IntcodeState.Done;
                        break;
                    }

                    switch (opcode)
                    {
                        case OpAdd:
                            GetArg(3, out int pos);
                            data[pos] = GetArg(1, out var _) + GetArg(2, out var _);
                            pc += 4;
                            break;

                        case OpMul:
                            GetArg(3, out int pos1);
                            data[pos1] = GetArg(1, out var _) * GetArg(2, out var _);
                            pc += 4;
                            break;

                        case OpInput:
                            if (!input.TryDequeue(out long inValue))
                            {
                                State = IntcodeState.PendingInput;
                                return this;
                            }

                            GetArg(1, out int poss);
                            data[poss] = inValue;
                            pc += 2;
                            break;

                        case OpOutput:
                            var a = GetArg(1, out var _);
                            Output.Enqueue(a);
                            if (PrintOutput)
                                Console.WriteLine(a);
                            pc += 2;
                            break;

                        case OpJumpIfTrue:
                            if (GetArg(1, out var _) != 0)
                                pc = (int)GetArg(2, out var _);
                            else
                                pc += 3;
                            break;

                        case OpJumpIfFalse:
                            if (GetArg(1, out var _) == 0)
                                pc = (int)GetArg(2, out var _);
                            else
                                pc += 3;
                            break;

                        case OpLessThan:
                            GetArg(3, out int pos2);
                            data[pos2] = GetArg(1, out var _) < GetArg(2, out var _) ? 1 : 0;
                            pc += 4;
                            break;

                        case OpEquals:
                            GetArg(3, out int pos3);
                            data[pos3] = GetArg(1, out var _) == GetArg(2, out var _) ? 1 : 0;
                            pc += 4;
                            break;

                        case OpRel:
                            rb += GetArg(1, out var _);
                            pc += 2;
                            break;

                        default:
                            throw new InvalidOperationException("Unknown opcode " + opcode);
                    }
                }

                return this;
            }

            private long GetArg(int argNum, out int position)
            {
                int mode = (CurrentOpcodeFull % (int)Math.Pow(10, argNum + 2)) / (int)Math.Pow(10, argNum + 1);

                if (mode == 1)
                {
                    position = pc + argNum;
                    data.TryGetValue(position, out var val1);
                    return val1;
                }

                if (mode == 0)
                {
                    position = (int)data[pc + argNum];
                    data.TryGetValue(position, out var val2);
                    return val2;
                }

                position = (int)rb + (int)data[pc + argNum];
                data.TryGetValue(position, out var val);
                return val;
            }
        }
    }
}

