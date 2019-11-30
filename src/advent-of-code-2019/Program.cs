using System;
using System.Diagnostics;
using System.IO;
using AdventOfCode.Y2018.Days;

namespace AdventOfCode.Y2018
{
    internal static class Program
    {
        private static void Main()
        {
            var day = 1;// DateTime.Now.Day;

            try
            {
                var inst = CreateInstance(day);
                RunDay(inst);

                Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        internal static IDay CreateInstance(int day)
        {
            var inst = (IDay)Activator.CreateInstance(Type.GetType($"{typeof(Day01).Namespace}.Day{day:0#}"));
            inst.InputRaw = File.ReadAllText($"Inputs/Day{day:0#}.txt");
            inst.Input = inst.InputRaw.TrimEnd().Replace("\r", "");
            return inst;
        }

        private static void RunDay(IDay day)
        {
            var sw = new Stopwatch();

            Console.WriteLine($"{day.GetType().Name}, {nameof(IDay.Part1)}");
            sw.Start();
            var r1 = day.Part1();
            sw.Stop();
            Console.WriteLine("Result: {0}\n", r1);
            Console.WriteLine("Time: {0}\n", sw.Elapsed);

            Console.WriteLine($"{day.GetType().Name}, {nameof(IDay.Part2)}");
            sw.Restart();
            var r2 = day.Part2();
            sw.Stop();
            Console.WriteLine("Result: {0}\n", r2);
            Console.WriteLine("Time: {0}\n", sw.Elapsed);
        }
    }
}
