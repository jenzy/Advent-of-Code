using System;
using System.IO;
using AdventOfCode.Y2017.Days;

namespace AdventOfCode.Y2017
{
    internal static class Program
    {
        private static void Main()
        {
            var day = DateTime.Now.Day;

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

        internal static string SolveDay(int day)
        {
            var inst = (IDay)Activator.CreateInstance(Type.GetType($"{typeof(Day1).Namespace}.Day{day}"));
            var input = (string)typeof(Input).GetField($"Day{day}").GetValue(null);

            var output = Console.Out;
            using (var tw = new StringWriter())
            {
                Console.SetOut(tw);
                try
                {
                    inst.Part1(input);
                    inst.Part2(input);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                Console.SetOut(output);

                return tw.GetStringBuilder().Replace("\n", "").Replace("\r", "").ToString();
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