using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Days
{
    internal class Day18 : IDay
    {
        public void Part1(string input)
        {
            var p0 = new Prog(Parse(input).ToList(), new Dictionary<string, long>());
            p0.Run();

            Console.WriteLine("Result: " + p0.ResultPart1);
        }

        public void Part2(string input)
        {
            var commands = Parse(input).ToList();
            Queue<long> q0 = new Queue<long>(), q1 = new Queue<long>();
            var p0 = new Prog(commands, new Dictionary<string, long> { ["p"] = 0 }, q0, q1);
            var p1 = new Prog(commands, new Dictionary<string, long> { ["p"] = 1 }, q1, q0);

            while (true)
            {
                p0.Run();
                p1.Run();

                if ((p0.BlockedCount > 1 && p1.BlockedCount > 1)
                    || (p0.Terminated && (p1.Terminated || p1.BlockedCount > 0))
                    || (p1.Terminated && (p0.Terminated || p0.BlockedCount > 0)))
                    break;
            }

            Console.WriteLine("Result: " + p1.CountSnd);
        }

        public static IEnumerable<string> Parse(string input) => input.Split('\n').Select(x => x.Trim());

        public class Prog
        {
            private readonly List<string> commands;
            private readonly Dictionary<string, long> reg;
            private readonly Queue<long> myQueue;
            private readonly Queue<long> otherQueue;
            private long lastFreq;
            private int i;

            public Prog(List<string> commands, Dictionary<string, long> reg, Queue<long> myQueue = null, Queue<long> otherQueue = null)
            {
                this.commands = commands;
                this.myQueue = myQueue;
                this.otherQueue = otherQueue;
                this.reg = reg;
            }

            public long? ResultPart1 { get; private set; }

            public int CountSnd { get; private set; }

            public int CountMul { get; private set; }

            public int BlockedCount { get; private set; }

            public bool Terminated { get; private set; }

            public void Run()
            {
                while (!Terminated)
                {
                    i += Process(commands[i]);

                    if (BlockedCount > 0)
                        break;

                    if (i < 0 || i >= commands.Count || (myQueue == null && ResultPart1 != null))
                        Terminated = true;
                }
            }

            private int Process(string command)
            {
                if (command.StartsWith("snd"))
                {
                    CountSnd++;
                    long val = GetValue(command.Substring(4));
                    if (otherQueue != null)
                        otherQueue.Enqueue(val);
                    else
                        lastFreq = val;
                }
                else if (command.StartsWith("set"))
                {
                    var arg = command.Substring(4).Split(' ');
                    reg[arg[0]] = GetValue(arg[1]);
                }
                else if (command.StartsWith("add"))
                {
                    var arg = command.Substring(4).Split(' ');
                    reg[arg[0]] = GetRegister(arg[0]) + GetValue(arg[1]);
                }
                else if (command.StartsWith("sub"))
                {
                    var arg = command.Substring(4).Split(' ');
                    reg[arg[0]] = GetRegister(arg[0]) - GetValue(arg[1]);
                }
                else if (command.StartsWith("mul"))
                {
                    CountMul++;
                    var arg = command.Substring(4).Split(' ');
                    reg[arg[0]] = GetRegister(arg[0]) * GetValue(arg[1]);
                }
                else if (command.StartsWith("mod"))
                {
                    var arg = command.Substring(4).Split(' ');
                    reg[arg[0]] = GetRegister(arg[0]) % GetValue(arg[1]);
                }
                else if (command.StartsWith("rcv"))
                {
                    var arg = command.Substring(4);
                    if (myQueue == null)
                    {
                        if (GetValue(arg) != 0)
                            ResultPart1 = lastFreq;
                    }
                    else
                    {
                        if (!myQueue.TryDequeue(out long val))
                        {
                            BlockedCount++;
                            return 0;
                        }
                        BlockedCount = 0;
                        reg[arg] = val;
                    }
                }
                else if (command.StartsWith("jgz"))
                {
                    var arg = command.Substring(4).Split(' ');
                    if (GetValue(arg[0]) > 0)
                        return (int)GetValue(arg[1]);
                }
                else if (command.StartsWith("jnz"))
                {
                    var arg = command.Substring(4).Split(' ');
                    if (GetValue(arg[0]) != 0)
                        return (int)GetValue(arg[1]);
                }

                return 1;
            }

            private long GetValue(string str) => long.TryParse(str, out long val) ? val : GetRegister(str);

            public long GetRegister(string str) => reg.TryGetValue(str, out long val) ? val : (reg[str] = 0);
        }
    }
}
