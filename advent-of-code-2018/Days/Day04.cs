using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AdventOfCode2018.Days
{
    /*

     */
    internal class Day04 : IDay
    {
        public string Part1(string input)
        {
            var events = Parse(input);
            var list = new List<GuardAsleep>();

            int guardId = 0;
            DateTime? from = null;
            foreach (var @event in events.OrderBy(e => e.Timestamp))
            {
                if (@event.EventType == EventType.StartShift && @event.GuardId != null)
                {
                    guardId = @event.GuardId.Value;
                }
                else if (@event.EventType == EventType.FallAsleep)
                {
                    from = @event.Timestamp;
                }
                else if (from != null)
                {
                    list.Add(new GuardAsleep
                    {
                        From = from.Value,
                        To = @event.Timestamp,
                        Id = guardId
                    });
                    from = null;
                }
            }

            var tmp = list.GroupBy(g => g.Id)
                          .Select(g => new
                          {
                              id = g.Key,
                              minutesAsleep = g.Sum(x => x.MinutesAssleep)
                          })
                          .OrderByDescending(g => g.minutesAsleep)
                          .First();

            int minute = list.Where(g => g.Id == tmp.id)
                             .Select(x => Enumerable.Range(x.From.Minute, x.To.Minute - x.From.Minute))
                             .SelectMany(x => x)
                             .GroupBy(x => x)
                             .OrderByDescending(g => g.Count())
                             .Select(g => g.Key)
                             .First();

            return (tmp.id * minute).ToString();
        }

        public string Part2(string input)
        {
            var events = Parse(input);
            var list = new List<GuardAsleep>();

            int guardId = 0;
            DateTime? from = null;
            foreach (var @event in events.OrderBy(e => e.Timestamp))
            {
                if (@event.EventType == EventType.StartShift && @event.GuardId != null)
                {
                    guardId = @event.GuardId.Value;
                }
                else if (@event.EventType == EventType.FallAsleep)
                {
                    from = @event.Timestamp;
                }
                else if (from != null)
                {
                    list.Add(new GuardAsleep
                    {
                        From = from.Value,
                        To = @event.Timestamp,
                        Id = guardId
                    });
                    from = null;
                }
            }

            var tmp = list.GroupBy(g => g.Id)
                          .Select(guard => new
                          {
                              id = guard.Key,
                              minute = guard.Select(x => Enumerable.Range(x.From.Minute, x.To.Minute - x.From.Minute))
                                            .SelectMany(x => x)
                                            .GroupBy(x => x)
                                            .OrderByDescending(g => g.Count())
                                            .Select(g => new {g.Key, count = g.Count()})
                                            .First()
                          })
                          .OrderByDescending(g => g.minute.count)
                          .First();

            return (tmp.id * tmp.minute.Key).ToString();
        }

        private static List<Event> Parse(string input)
        {
            return input.Split("\n")
                        .Select(l =>
                        {
                            var spl1 = l.Split("]");
                            var ixHash = spl1[1].IndexOf('#');
                            var ixSpace = ixHash > 0 ? spl1[1].IndexOf(' ', ixHash) : -1;
                            return new Event
                            {
                                Timestamp = DateTime.ParseExact(spl1[0].TrimStart('['), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                                GuardId = ixHash > 0 ? int.Parse(spl1[1].Substring(ixHash+1, ixSpace - ixHash-1)) : (int?)null,
                                EventType = ixHash > 0 ?  EventType.StartShift : spl1[1].EndsWith("up") ? EventType.WakeUp : EventType.FallAsleep
                            };
                        })
                        .ToList();
        }

        private class Event
        {
            public EventType EventType { get; set; }

            public int? GuardId { get; set; }

            public DateTime Timestamp { get; set; }
        }

        private class GuardAsleep
        {
            public int Id { get; set; }

            public DateTime From { get; set; }

            public DateTime To { get; set; }

            public int MinutesAssleep => To.Minute - From.Minute;
        }

        private enum EventType
        {
            StartShift,
            FallAsleep,
            WakeUp
        }
    }
}
