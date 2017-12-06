using System;
using AdventOfCode2017.Days;

namespace AdventOfCode2017
{
    internal static class Program
    {
        private static void Main()
        {
            var day = 6;

            try
            {
                var inst = (IDay)Activator.CreateInstance(Type.GetType($"{typeof(Day1).Namespace}.Day{day}"));
                var input = (string)typeof(Input).GetField($"Day{day}").GetValue(null);

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
            day.Part1(input);
            Console.WriteLine();

            Console.WriteLine($"{day.GetType().Name}, {nameof(IDay.Part2)}");
            day.Part2(input);
            Console.WriteLine();
        }
    }
}