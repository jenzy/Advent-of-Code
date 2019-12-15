using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Common
{
    public class Intcode
    {
        public enum IntcodeState { Ready, PendingInput, Done }

        private enum ArgMode { Position = 0, Immediate = 1, Relative = 2 }

        private enum OpCode
        {
            OpAdd = 1,
            OpMul = 2,
            OpInput = 3,
            OpOutput = 4,
            OpJumpIfTrue = 5,
            OpJumpIfFalse = 6,
            OpLessThan = 7,
            OpEquals = 8,
            OpRel = 9,
            Exit = 99
        }

        private int pc = 0;
        private long rb = 0;
        private readonly IDictionary<int, long> data;

        public Intcode(IEnumerable<long> memory, IEnumerable<long> input = null)
        {
            this.data = memory.Select((x, ix) => (x, ix)).ToDictionary(x => x.ix, x => x.x);
            this.Input = input as Queue<long> ?? new Queue<long>(input ?? Enumerable.Empty<long>());
            this.Output = new Queue<long>();
        }

        private Intcode(Intcode other)
        {
            this.data = other.data.ToDictionary(x => x.Key, x => x.Value);
            this.pc = other.pc;
            this.rb = other.rb;
            this.State = other.State;
            this.Input = new Queue<long>();
            this.Output = new Queue<long>();
        }

        public IntcodeState State { get; private set; } = IntcodeState.Ready;

        public long SimpleOutput => data[0];

        public Queue<long> Input { get; private set; }

        public Queue<long> Output { get; }

        private OpCode CurrentOpcodeFull => (OpCode)data[pc];

        private OpCode CurrentOpcode => (OpCode)((int)CurrentOpcodeFull % 100);

        public void SetInput(Queue<long> customInput) => this.Input = customInput;

        public Intcode Run()
        {
            while (true)
            {
                switch (CurrentOpcode)
                {
                    case OpCode.Exit:
                        State = IntcodeState.Done;
                        return this;

                    case OpCode.OpAdd:
                        data[GetArgPosition(3)] = GetArgValue(1) + GetArgValue(2);
                        pc += 4;
                        break;

                    case OpCode.OpMul:
                        data[GetArgPosition(3)] = GetArgValue(1) * GetArgValue(2);
                        pc += 4;
                        break;

                    case OpCode.OpInput:
                        if (!Input.TryDequeue(out long inValue))
                        {
                            State = IntcodeState.PendingInput;
                            return this;
                        }

                        data[GetArgPosition(1)] = inValue;
                        pc += 2;
                        break;

                    case OpCode.OpOutput:
                        Output.Enqueue(GetArgValue(1));
                        pc += 2;
                        break;

                    case OpCode.OpJumpIfTrue:
                        pc = GetArgValue(1) != 0 ? (int)GetArgValue(2) : (pc + 3);
                        break;

                    case OpCode.OpJumpIfFalse:
                        pc = GetArgValue(1) == 0 ? (int)GetArgValue(2) : (pc + 3);
                        break;

                    case OpCode.OpLessThan:
                        data[GetArgPosition(3)] = GetArgValue(1) < GetArgValue(2) ? 1 : 0;
                        pc += 4;
                        break;

                    case OpCode.OpEquals:
                        data[GetArgPosition(3)] = GetArgValue(1) == GetArgValue(2) ? 1 : 0;
                        pc += 4;
                        break;

                    case OpCode.OpRel:
                        rb += GetArgValue(1);
                        pc += 2;
                        break;

                    default:
                        throw new InvalidOperationException("Unknown opcode " + CurrentOpcode);
                }
            }
        }

        public Intcode Clone() => new Intcode(this);

        public Intcode WithInput(long input)
        {
            this.Input.Enqueue(input);
            return this;
        }

         private int GetArgPosition(int argNum)
        {
            var mode = (ArgMode)(((int)CurrentOpcodeFull % (int)Math.Pow(10, argNum + 2)) / (int)Math.Pow(10, argNum + 1));
            switch (mode)
            {
                case ArgMode.Position:
                    return (int)data[pc + argNum];

                case ArgMode.Immediate:
                    return pc + argNum;

                case ArgMode.Relative:
                    return (int)rb + (int)data[pc + argNum];

                default:
                    throw new InvalidOperationException("Unknown parameter mode " + (int)mode);
            }
        }

        private long GetArgValue(int argNum) => data.TryGetValue(GetArgPosition(argNum), out long val) ? val : 0;
    }
}
