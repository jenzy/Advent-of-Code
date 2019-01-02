namespace AdventOfCode.Y2018
{
    internal abstract class DayBase : IDay
    {
        public string Input { get; set; }

        public string InputRaw { get; set; }

        public abstract object Part1();

        public abstract object Part2();
    }
}
