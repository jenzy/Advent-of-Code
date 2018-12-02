using System;
using System.IO;
using AdventOfCode2018.Days;

namespace AdventOfCode2018
{
    internal static class Program
    {
        private static void Main()
        {
            var day = 1; //DateTime.Now.Day;

            try
            {
                var inst = (IDay)Activator.CreateInstance(Type.GetType($"{typeof(DayX).Namespace}.Day{day:0#}"));
                var input = File.ReadAllText($"Inputs/Day{day:0#}.txt").TrimEnd().Replace("\r", "");

                RunDay(inst, input);

                Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        private static void RunDay(IDay day, string input)
        {
            Console.WriteLine($"{day.GetType().Name}, {nameof(IDay.Part1)}");
            var r1 = day.Part1(input);
            Console.WriteLine("Result: {0}\n", r1);

            Console.WriteLine($"{day.GetType().Name}, {nameof(IDay.Part2)}");
            var r2 = day.Part2(input);
            Console.WriteLine("Result: {0}\n", r2);
        }
    }
}
