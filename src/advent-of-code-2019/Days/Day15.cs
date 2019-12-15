using AdventOfCode.Common;
using AdventOfCode.Y2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*
--- Day 15: Oxygen System ---

Out here in deep space, many things can go wrong.
Fortunately, many of those things have indicator lights.
Unfortunately, one of those lights is lit: the oxygen system for part of the ship has failed!

According to the readouts, the oxygen system must have failed days ago after a rupture in oxygen tank two;
that section of the ship was automatically sealed once oxygen levels went dangerously low.
A single remotely-operated repair droid is your only option for fixing the oxygen system.

The Elves' care package included an Intcode program (your puzzle input) that you can use to remotely control the repair droid.
By running that program, you can direct the repair droid to the oxygen system and fix the problem.

The remote control program executes the following steps in a loop forever:

    Accept a movement command via an input instruction.
    Send the movement command to the repair droid.
    Wait for the repair droid to finish the movement operation.
    Report on the status of the repair droid via an output instruction.

Only four movement commands are understood: north (1), south (2), west (3), and east (4). Any other command is invalid.
The movements differ in direction, but not in distance: in a long enough east-west hallway,
a series of commands like 4,4,4,4,3,3,3,3 would leave the repair droid back where it started.

The repair droid can reply with any of the following status codes:

    0: The repair droid hit a wall. Its position has not changed.
    1: The repair droid has moved one step in the requested direction.
    2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.

You don't know anything about the area around the repair droid, but you can figure it out by watching the status codes.

For example, we can draw the area using D for the droid, # for walls, . for locations the droid can traverse,
and empty space for unexplored locations. Then, the initial state looks like this:

      
      
   D  
      
      

To make the droid go north, send it 1. If it replies with 0, you know that location is a wall and that the droid didn't move:

      
   #  
   D  
      
      

To move east, send 4; a reply of 1 means the movement was successful:

      
   #  
   .D 
      
      

Then, perhaps attempts to move north (1), south (2), and east (4) are all met with replies of 0:

      
   ## 
   .D#
    # 
      

Now, you know the repair droid is in a dead end.
Backtrack with 3 (which you already know will get a reply of 1 because you already know that location is open):

      
   ## 
   D.#
    # 
      

Then, perhaps west (3) gets a reply of 0, south (2) gets a reply of 1,
south again (2) gets a reply of 0, and then west (3) gets a reply of 2:

      
   ## 
  #..#
  D.# 
   #  

Now, because of the reply of 2, you know you've found the oxygen system! In this example,
it was only 2 moves away from the repair droid's starting position.

What is the fewest number of movement commands required to move the repair droid
from its starting position to the location of the oxygen system?

--- Part Two ---

You quickly repair the oxygen system; oxygen gradually fills the area.

Oxygen starts in the location containing the repaired oxygen system.
It takes one minute for oxygen to spread to all open locations that are adjacent to a location that already contains oxygen.
Diagonal locations are not adjacent.

In the example above, suppose you've used the droid to explore the area fully and have the following map
(where locations that currently contain oxygen are marked O):

 ##   
#..## 
#.#..#
#.O.# 
 ###  

Initially, the only location which contains oxygen is the location of the repaired oxygen system.
However, after one minute, the oxygen spreads to all open (.) locations that are adjacent to a location containing oxygen:

 ##   
#..## 
#.#..#
#OOO# 
 ###  

After a total of two minutes, the map looks like this:

 ##   
#..## 
#O#O.#
#OOO# 
 ###  

After a total of three minutes:

 ##   
#O.## 
#O#OO#
#OOO# 
 ###  

And finally, the whole region is full of oxygen after a total of four minutes:

 ##   
#OO## 
#O#OO#
#OOO# 
 ###  

So, in this example, all locations contain oxygen after 4 minutes.

Use the repair droid to get a complete map of the area. How many minutes will it take to fill with oxygen?

     */

    public class Day15 : DayBase
    {
        public override object Part1()
        {
            var map = new Map();
            var queue = new Queue<Robot>();
            queue.Enqueue(new Robot(new Intcode(Parse(Input))));

            while (true)
            {
                var robot = queue.Dequeue();
                if (robot.Oxygen)
                    return robot.Distance;
                queue.EnqueueRange(robot.Explore(map));
            }
        }

        public override object Part2()
        {
            var map = new Map();
            var queue = new Queue<Robot>();
            queue.Enqueue(new Robot(new Intcode(Parse(Input))));

            (int x, int y) oxygen = (0, 0);
            while (queue.Count > 0)
            {
                var robot = queue.Dequeue();
                if (robot.Oxygen)
                    oxygen = robot.Position;
                queue.EnqueueRange(robot.Explore(map));
            }

            foreach (var kvp in map)
            {
                foreach (var other in kvp.Value)
                    map[other].Add(kvp.Key);
            }

            var seen = new HashSet<(int x, int y)>();
            return Recurse(oxygen, 0);

            int Recurse((int x, int y) position, int count)
            {
                if (seen.Contains(position))
                    return count - 1;

                seen.Add(position);
                if (map.TryGetValue(position, out var neigbours) && neigbours.Count > 0)
                    return neigbours.Max(x => Recurse(x, count + 1));
                return count;
            }
        }

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(15);
            Assert.Equal(248, day.Part1());
            Assert.Equal(382, day.Part2());
        }

        private class Robot
        {
            private readonly Intcode controller;

            public Robot(Intcode controller) => this.controller = controller;

            public (int x, int y) Position { get; private set; } = (0, 0);

            public int Distance { get; private set; }

            public bool Oxygen { get; private set; }

            public IEnumerable<Robot> Explore(Map map)
            {
                map.Add(Position, new List<(int x, int y)>());
                for (int direction = 1; direction <= 4; direction++)
                {
                    var newRobot = Explore(direction, map);
                    if (newRobot != null)
                    {
                        map[Position].Add(newRobot.Position);
                        yield return newRobot;
                    }
                }
            }

            public Robot Explore(int direction, Map map)
            {
                var newPosition = GetNewPosition(direction);
                if (map.ContainsKey(newPosition))
                    return null;

                var newController = this.controller.Clone().WithInput(direction).Run();
                var output = newController.Output.Dequeue();
                if (output == 0)
                    return null;

                return new Robot(newController)
                {
                    Position = newPosition,
                    Distance = Distance + 1,
                    Oxygen = output == 2
                };
            }

            private (int x, int y) GetNewPosition(int direction) => direction switch
            {
                1 => (Position.x, Position.y - 1),
                2 => (Position.x, Position.y + 1),
                3 => (Position.x - 1, Position.y),
                4 => (Position.x + 1, Position.y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        private class Map : Dictionary<(int x, int y), List<(int x, int y)>>
        {
        }
    }
}
