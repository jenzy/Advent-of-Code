namespace AdventOfCode.Y2018
{
    internal interface IDay
    {
        string Input { get; set; }

        string InputRaw { get; set; }

        object Part1();

        object Part2();
    }
}