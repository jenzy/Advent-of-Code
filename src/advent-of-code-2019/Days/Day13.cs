using AdventOfCode.Common;
using AdventOfCode.Y2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Y2019.Days
{
    /*
--- Day 13: Care Package ---

As you ponder the solitude of space and the ever-increasing three-hour roundtrip for messages between you and Earth,
you notice that the Space Mail Indicator Light is blinking. To help keep you sane, the Elves have sent you a care package.

It's a new game for the ship's arcade cabinet! Unfortunately, the arcade is all the way on the other end of the ship.
Surely, it won't be hard to build your own - the care package even comes with schematics.

The arcade cabinet runs Intcode software like the game the Elves sent (your puzzle input).
It has a primitive screen capable of drawing square tiles on a grid. The software draws tiles to the screen with output instructions:
every three output instructions specify the x position (distance from the left), y position (distance from the top), and tile id.
The tile id is interpreted as follows:

    0 is an empty tile. No game object appears in this tile.
    1 is a wall tile. Walls are indestructible barriers.
    2 is a block tile. Blocks can be broken by the ball.
    3 is a horizontal paddle tile. The paddle is indestructible.
    4 is a ball tile. The ball moves diagonally and bounces off objects.

For example, a sequence of output values like 1,2,3,6,5,4 would draw a horizontal paddle tile
(1 tile from the left and 2 tiles from the top) and a ball tile (6 tiles from the left and 5 tiles from the top).

Start the game. How many block tiles are on the screen when the game exits?

--- Part Two ---

The game didn't run because you didn't put in any quarters. Unfortunately, you did not bring any quarters.
Memory address 0 represents the number of quarters that have been inserted; set it to 2 to play for free.

The arcade cabinet has a joystick that can move left and right. The software reads the position of the joystick with input instructions:

    If the joystick is in the neutral position, provide 0.
    If the joystick is tilted to the left, provide -1.
    If the joystick is tilted to the right, provide 1.

The arcade cabinet also has a segment display capable of showing a single number that represents the player's current score.
When three output instructions specify X=-1, Y=0, the third output instruction is not a tile;
the value instead specifies the new score to show in the segment display.
For example, a sequence of output values like -1,0,12345 would show 12345 as the player's current score.

Beat the game by breaking all the blocks. What is your score after the last block is broken?

     */

    public class Day13 : DayBase
    {
        public override object Part1()
        {
            var output = new Intcode(Parse(Input)).Run().Output;

            var grid = new Dictionary<(int x, int y), int>();
            while (output.Count > 0)
            {
                int x = (int)output.Dequeue(), y = (int)output.Dequeue();
                grid[(x, y)] = (int)output.Dequeue();
            }

            return grid.Values.Count(x => x == 2);
        }

        public override object Part2()
        {
            var memory = Parse(Input).ToList();
            memory[0] = 2;
            var game = new Intcode(memory);
            var grid = new Dictionary<(int x, int y), int>();
            bool manual = false;
            int xBall = -1, xPaddle = -1, score = 0;

            while (game.State != Intcode.IntcodeState.Done)
            {
                game.Run();

                while (game.Output.Count > 0)
                {
                    int x = (int)game.Output.Dequeue(), y = (int)game.Output.Dequeue(), val = (int)game.Output.Dequeue();
                    if (x == -1 && y == 0)
                    {
                        score = val;
                    }
                    else
                    {
                        grid[(x, y)] = val;
                        if (val == 3)
                            xPaddle = x;
                        else if (val == 4)
                            xBall = x;
                    }
                }

                int input = 0;
                if (xBall > -1 && xPaddle > -1 && xBall != xPaddle)
                    input = xBall > xPaddle ? 1 : -1;

                if (manual)
                {
                    Draw(grid, score);
                    Console.Write("> ");
                    game.Input.Enqueue(int.Parse(Console.ReadLine()));
                }
                else
                {
                    game.Input.Enqueue(input);
                }
            }

            return score;
        }

        private static void Draw(Dictionary<(int x, int y), int> grid, int score)
        {
            var limX = grid.Keys.MinMax(x => x.x);
            var limY = grid.Keys.MinMax(x => x.y);

            Console.Clear();
            for (int y = limY.min; y <= limY.max; y++)
            {
                for (int x = limX.min; x <= limX.max; x++)
                {
                    grid.TryGetValue((x, y), out var val);
                    char c = ' ';
                    switch (val)
                    {
                        case 1: c = '#'; break;
                        case 2: c = '$'; break;
                        case 3: c = '-'; break;
                        case 4: c = '*'; break;

                    }

                    Console.Write(c);
                }

                Console.Write("\n");
            }

            Console.Write($"\nScore: {score}\n\n");
        }

        private static IEnumerable<long> Parse(string input) => input.Split(',').Select(long.Parse);

        [Fact]
        public static void Test()
        {
            var day = Program.CreateInstance(13);
            Assert.Equal(320, day.Part1());
            Assert.Equal(15156, day.Part2());
        }
    }
}
