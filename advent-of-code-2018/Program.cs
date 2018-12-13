using System;
using System.Diagnostics;
using System.IO;
using AdventOfCode2018.Days;

namespace AdventOfCode2018
{
    internal static class Program
    {
        private static void Main()
        {
            var day = DateTime.Now.Day;

            try
            {
                var inst = (IDay)Activator.CreateInstance(Type.GetType($"{typeof(DayX).Namespace}.Day{day:0#}"));
                inst.InputRaw = File.ReadAllText($"Inputs/Day{day:0#}.txt");
                inst.Input = inst.InputRaw.TrimEnd().Replace("\r", "");

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
