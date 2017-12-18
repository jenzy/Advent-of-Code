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
            Console.WriteLine("Result: " + Common(input, input.Length/2));
        }



        private static long Common(string input, int offset) =>
            input.Where((cur, i) => cur == input[(i + offset) % input.Length])
                 .Sum(t => (long) char.GetNumericValue(t));

        class Prog
        {
            private readonly List<string> commands;
            private readonly int? id;
            private readonly Dictionary<string, long> reg;
            private long lastFreq;
            public long? result = null;
            private int i = 0;

            public Prog(List<string> commands, int? id)
            {
                this.commands = commands;
                this.id = id;
                this.reg = new Dictionary<string, long>();
                if (id > 0)
                    reg["p"] = id.Value;
            }

            public void Run()
            {
                while (i >= 0 && i < commands.Count)
                {
                    i += Process(commands[i]);
                    if (id == null && result != null)
                        break;
                }
            }


            private int Process(string command)
            {
                if (command.StartsWith("snd"))
                {
                    lastFreq = GetValue(command.Substring(4));
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
                    var arg = GetValue(command.Substring(4));
                    if (arg != 0)
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
