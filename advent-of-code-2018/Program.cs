using System;
using System.IO;
using AdventOfCode2018.Days;

namespace AdventOfCode2018
{
    static class Program
    {
        static void Main()
        {
            var day = DateTime.Now.Day;

            try
            {
                var inst = (IDay)Activator.CreateInstance(Type.GetType($"{typeof(DayX).Namespace}.Day{day}"));
                var input = File.ReadAllText($"Inputs/Day{day}.txt");

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
