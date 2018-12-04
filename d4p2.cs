using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D4P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_4_t_1.txt").ToList();
            Assert.AreEqual(4455, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_4.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {

            private class Guard
            {
                public int Id { set; get; }
                public int[] AsleepAt = new int[60];
            }

            private class Sleep
            {
                public int GuardId { set; get; }
                public DateTime Start { set; get; }
                public DateTime End { set; get; }
            }

            public class Event
            {
                public enum EventType { WAKES_UP, FALLS_ASLEEP, SHIFT_BEGINS}
                public DateTime Time { set; get; }
                public EventType Type { set; get; }
                public int GuardId { set; get; }
            }

            public int RunChallenge(List<string> input)
            {

                var events = input.Select(raw =>
                {
                    var split = raw.Split(new string[] { "[", "]"}, StringSplitOptions.None).Skip(1).ToArray();
                    var et = new Event {
                        Time = Convert.ToDateTime(split[0]),
                    };;
                    if (split[1].Contains("wakes up"))
                    {
                        et.Type = Event.EventType.WAKES_UP;
                    } else if (split[1].Contains("falls asleep")){
                        et.Type = Event.EventType.FALLS_ASLEEP;
                    } else
                    {
                        et.Type = Event.EventType.SHIFT_BEGINS;
                        et.GuardId = int.Parse(split[1].Split(new string[] { "#", " " }, StringSplitOptions.None)[3]);
                    }
                    return et;
                }).OrderBy(e => e.Time).ToArray();

                var sleepPeriods = new List<Sleep>();
                var currentGuard = 0;
                Sleep currentSleep = null;
                for(int i = 0; i < events.Count(); i++)
                {
                    var e = events[i];
                    if (e.Type == Event.EventType.SHIFT_BEGINS)
                    {
                        currentGuard = e.GuardId;
                    } else if(e.Type == Event.EventType.FALLS_ASLEEP)
                    {
                        currentSleep = new Sleep
                        {
                            GuardId = currentGuard,
                            Start = e.Time
                        };
                        sleepPeriods.Add(currentSleep);
                    } else
                    {
                        currentSleep.End = e.Time;
                    }
                }

                var guards = new List<Guard>();

                sleepPeriods.Select(z => z.GuardId).Distinct().ToList().ForEach(id => guards.Add(new Guard { Id = id }));

                sleepPeriods.ForEach(sleep =>
                {
                    var guard = guards.First(z => z.Id == sleep.GuardId);
                    for (int i = sleep.Start.Minute; i < sleep.End.Minute; i++)
                    {
                        guard.AsleepAt[i]++;
                    }
                });

                var maxGuard = guards.OrderByDescending(g => g.AsleepAt.Max()).Take(1).First();
                var maxMinute = maxGuard.AsleepAt.ToList().IndexOf(maxGuard.AsleepAt.Max());

                return maxGuard.Id * maxMinute;
            }
        }
    }
}
