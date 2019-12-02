namespace AdventOfCode.Y2019
{
    internal interface IDay
    {
        string Input { get; set; }

        string InputRaw { get; set; }

        object Part1();

        object Part2();
    }
}