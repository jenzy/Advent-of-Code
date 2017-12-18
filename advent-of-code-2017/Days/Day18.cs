using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Days
{
    internal class Day18 : IDay
    {
        public void Part1(string input)
        {
            var commands = input.Split('\n').Select(x => x.Trim()).ToList();

            var p0 = new Prog(commands, null);
            p0.Run();

            Console.WriteLine("Result: " + p0.result);
        }

        public void Part2(string input)
        {
            var commands = input.Split('\n').Select(x => x.Trim()).ToList();

            Queue<long> q0 = new Queue<long>(), q1 = new Queue<long>();
            var p0 = new Prog(commands, 0, q0, q1);
            var p1 = new Prog(commands, 1, q1, q0);

            int blckCount = 0;
            while (true)
            {
                p0.Run();

                p1.Run();


                if (p0.Blocked && p1.Blocked)
                {
                    blckCount++;
                    if (p0.BlcCount > 10 && p1.BlcCount > 10)
                        break;
                }
                else
                {
                    blckCount = 0;
                }

                if (p0.Terminated && p1.Blocked)
                    break;

                if (p1.Terminated && p0.Blocked)
                    break;

                if (p0.Terminated && p1.Terminated)
                    break;
            }

            Console.WriteLine("Result: " + p1.sndCount);
        }


        class Prog
        {
            private readonly List<string> commands;
            private readonly int? id;
            private readonly Dictionary<string, long> reg;
            private readonly Queue<long> myQueue;
            private readonly Queue<long> otherQueue;
            private long lastFreq;
            public long? result = null;
            private int i = 0;
            public int sndCount = 0;

            public Prog(List<string> commands, int? id, Queue<long> myQueue = null, Queue<long> otherQueue = null)
            {
                this.commands = commands;
                this.id = id;
                this.myQueue = myQueue;
                this.otherQueue = otherQueue;
                this.reg = new Dictionary<string, long>();
                if (id > 0)
                    reg["p"] = id.Value;
            }

            public bool Blocked { get; set; }

            public int BlcCount { get; set; }

            public bool Terminated { get; set; }

            public void Run()
            {
                if (Terminated)
                    return;

                while (true)
                {
                    if (i < 0 && i >= commands.Count)
                    {
                        Terminated = true;
                        break;
                    }

                    i += Process(commands[i]);
                    if (id == null && result != null)
                        break;

                    if (Blocked)
                        break;
                }
            }


            private int Process(string command)
            {
                if (command.StartsWith("snd"))
                {
                    var val = GetValue(command.Substring(4));
                    sndCount++;
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
                    reg[arg[0]] = GetReg(arg[0]) + GetValue(arg[1]);
                }
                else if (command.StartsWith("mul"))
                {
                    var arg = command.Substring(4).Split(' ');
                    reg[arg[0]] = GetReg(arg[0]) * GetValue(arg[1]);
                }
                else if (command.StartsWith("mod"))
                {
                    var arg = command.Substring(4).Split(' ');
                    reg[arg[0]] = GetReg(arg[0]) % GetValue(arg[1]);
                }
                else if (command.StartsWith("rcv"))
                {
                    var arg = command.Substring(4);
                    if (myQueue != null)
                    {
                        Blocked = false;
                        if (!myQueue.TryDequeue(out var val))
                        {
                            Blocked = true;
                            BlcCount++;
                            return 0;
                        }
                        BlcCount = 0;
                        reg[arg] = val;
                    }
                    else if (GetValue(arg) != 0)
                        result = lastFreq;
                }
                else if (command.StartsWith("jgz"))
                {
                    var arg = command.Substring(4).Split(' ');
                    if (GetValue(arg[0]) > 0)
                        return (int)GetValue(arg[1]);
                }

                return 1;
            }

            private long GetValue(string str)
            {
                return long.TryParse(str, out long val) ? val : GetReg(str);
            }


            private long GetReg(string str)
            {
                if (!reg.TryGetValue(str, out long val))
                    reg[str] = val = 0;

                return val;
            }
        }
    }
}
