using AdventOfCode.Y2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*
 --- Day 25: Cryostasis ---

As you approach Santa's ship, your sensors report two important details:

First, that you might be too late: the internal temperature is -40 degrees.

Second, that one faint life signature is somewhere on the ship.

The airlock door is locked with a code; your best option is to send in a small droid to investigate the situation.
You attach your ship to Santa's, break a small hole in the hull, and let the droid run in before you seal it up again.
Before your ship starts freezing, you detach your ship and set it to automatically stay within range of Santa's ship.

This droid can follow basic instructions and report on its surroundings;
you can communicate with it through an Intcode program (your puzzle input) running on an ASCII-capable computer.

As the droid moves through its environment, it will describe what it encounters.
When it says Command?, you can give it a single instruction terminated with a newline (ASCII code 10).
Possible instructions are:

    Movement via north, south, east, or west.
    To take an item the droid sees in the environment, use the command take <name of item>.
        For example, if the droid reports seeing a red ball, you can pick it up with take red ball.
    To drop an item the droid is carrying, use the command drop <name of item>.
        For example, if the droid is carrying a green ball, you can drop it with drop green ball.
    To get a list of all of the items the droid is currently carrying, use the command inv (for "inventory").

Extra spaces or other characters aren't allowed - instructions must be provided precisely.

Santa's ship is a Reindeer-class starship; these ships use pressure-sensitive floors to determine the identity of droids and crew members.
The standard configuration for these starships is for all droids to weigh exactly the same amount to make them easier to detect.
If you need to get past such a sensor, you might be able to reach the correct weight by carrying items from the environment.

Look around the ship and see if you can find the password for the main airlock.

--- Part Two ---

As you move through the main airlock, the air inside the ship is already heating up to reasonable levels.
Santa explains that he didn't notice you coming because he was just taking a quick nap.
The ship wasn't frozen; he just had the thermostat set to "North Pole".

You make your way over to the navigation console. It beeps. "Status: Stranded. Please supply measurements from 49 stars to recalibrate."

"49 stars? But the Elves told me you needed fifty--"

Santa just smiles and nods his head toward the window. There, in the distance, you can see the center of the Solar System: the Sun!

The navigation console beeps again.

     */

    public class Day25 : DayBase
    {
        private readonly Random random = new Random();
        private readonly bool console = false;

        public override object Part1()
        {
            var blacklist = new [] { "molten lava", "giant electromagnet", "infinite loop", "escape pod", "photons" };
            var inventory = new List<string>();

            var ic = new Intcode(Input.Split(',').Select(long.Parse));
            var output = RunStep(ic, null, inventory);

            while (!output.raw.Contains("== Pressure-Sensitive Floor ==") || inventory.Count != 8)
            {
                output = RunStep(ic, output.doors[random.Next(output.doors.Count)], inventory);
                foreach (var item in output.items.Except(blacklist))
                {
                    inventory.Add(item);
                    RunStep(ic, "take " + item, inventory);
                }
            }

            var droppedItems = new List<string>();
            while (true)
            {
                output = RunStep(ic, output.doors.First(), inventory);
                if (output.raw.Contains("heavier"))
                {
                    var item = droppedItems[random.Next(droppedItems.Count)];
                    droppedItems.Remove(item);
                    inventory.Add(item);
                    RunStep(ic, "take " + item, inventory);
                }
                else if (output.raw.Contains("lighter"))
                {
                    var item = inventory[random.Next(inventory.Count)];
                    droppedItems.Add(item);
                    inventory.Remove(item);
                    RunStep(ic, "drop " + item, inventory);
                }
                else
                {
                    var m = Regex.Match(output.raw, @"(\d+)");
                    if (m.Success)
                        return int.Parse(m.Captures.Single().Value);

                    throw new Exception("Error parsing password");
                }
            }
        }

        public override object Part2() => null;

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(25);
            Assert.Equal(8462464, day.Part1());
        }

        private (string raw, List<string> doors, List<string> items) RunStep(Intcode ic, string input, List<string> inventory)
        {
            if (input != null)
            {
                foreach (var c in input)
                    ic.Input.Enqueue(c);
                ic.Input.Enqueue('\n');
            }

            ic.Run();

            var output = string.Join("", ic.Output.Select(x => (char)x));
            ic.Output.Clear();

            if (console)
            {
                Console.WriteLine("INPUT: " + input);
                Console.WriteLine("OUTPUT: " + output);
                Console.WriteLine("INVENTORY: " + string.Join(", ", inventory));
                Console.WriteLine(new string('-', 80) + '\n');
            }

            var spl = output.Split('\n');
            var doors = new List<string>();
            var items = new List<string>();

            for (int i = 0; i < spl.Length;)
            {
                if (spl[i].StartsWith("Items here:"))
                {
                    while (spl[++i].StartsWith("- "))
                        items.Add(spl[i].Substring(2));
                }
                else if (spl[i].StartsWith("Doors here lead:"))
                {
                    while (spl[++i].StartsWith("- "))
                        doors.Add(spl[i].Substring(2));
                }
                else if (spl[i].Contains("you are ejected back to the checkpoint"))
                {
                    doors.Clear();
                    items.Clear();
                    i++;
                }
                else
                {
                    i++;
                }
            }

            return (raw: output, doors, items);
        }
    }
}