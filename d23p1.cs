using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D23P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_23_t_1.txt").ToList();
            Assert.AreEqual(7, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_23.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public class Nanobot
            {
                public Coordinate Coordinate { set; get; }
                public int Radius { set; get; }
            }

            public List<Nanobot> Nanobots;

            public int RunChallenge(List<string> input)
            {
                Nanobots = input.Select(raw =>
                {
                    var parsed = raw.Split(new string[] { "pos=<", ",", ">", "r=" }, StringSplitOptions.None).ToArray();
                    return new Nanobot
                    {
                        Coordinate = new Coordinate
                        {
                            X = int.Parse(parsed[1]),
                            Y = int.Parse(parsed[2]),
                            Z = int.Parse(parsed[3]),
                        },
                        Radius = int.Parse(parsed[6])
                    };
                }).ToList();

                var strongest = Nanobots.First(z => z.Radius == Nanobots.Max(n => n.Radius));

                var inRange = 0;
                foreach (var nanobot in Nanobots)
                {
                    var xDist = Math.Abs(strongest.Coordinate.X - nanobot.Coordinate.X);
                    var yDist = Math.Abs(strongest.Coordinate.Y - nanobot.Coordinate.Y);
                    var zDist = Math.Abs(strongest.Coordinate.Z - nanobot.Coordinate.Z);

                    if (xDist + yDist + zDist <= strongest.Radius)
                    {
                        inRange++;
                    }
                }

                return inRange;
            }
        }
    }
}
