using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D6P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_6_t_1.txt").ToList();
            Assert.AreEqual(17, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_6.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            private class Point
            {
                public Coordinate Coordinate { set; get; }
                public int ClosestTo { set; get; }
            }

            private class Area
            {
                public int Size { set; get; }
                public int Id { set; get; }
                public bool infinite { set; get; }
            }

            public int RunChallenge(List<string> input)
            {
                var coordinates = input.Select(raw =>
                {
                    var split = raw.Split(new string[] { ",", }, StringSplitOptions.None).ToArray();
                    return new Coordinate {
                        X = int.Parse(split[0].Trim()),
                        Y = int.Parse(split[1].Trim())
                    };

                }).ToArray();

                var gridSize = new { X = coordinates.Max(c => c.X), Y = coordinates.Max(c => c.Y) };

                var points = new Point[gridSize.X, gridSize.Y];

                for (int x = 0; x < gridSize.X; x++)
                {
                    for (int y = 0; y < gridSize.Y; y++)
                    {
                        var p = new Point { Coordinate = new Coordinate { X = x, Y = y } };

                        var lowestIndex = -1;
                        var lowestDistance = 99999;
                        for (int c = 0; c < coordinates.Length; c++)
                        {

                            var coord = coordinates[c];
                            var distance = Math.Abs(coord.X - x) + Math.Abs(coord.Y - y);

                            if (distance < lowestDistance)
                            {
                                lowestIndex = c;
                                lowestDistance = distance;
                            } else if (distance == lowestDistance)
                            {
                                lowestIndex = -1;
                                lowestDistance = distance;
                            }
                        }
                        p.ClosestTo = lowestIndex;
                        points[x, y] = p;
                    }
                }

                // iterare punkter, hitta grupper och radera grupper som ligger på kanten på griddet
                var areas = new List<Area>();

                for (int x = 0; x < gridSize.X; x++)
                {
                    for (int y = 0; y < gridSize.Y; y++)
                    {
                        var point = points[x, y];
                        if (!areas.Any(a => a.Id == point.ClosestTo))
                        {
                            areas.Add(new Area { Id = point.ClosestTo, Size = 1 });
                        } else
                        {
                            areas.First(a => a.Id == point.ClosestTo).Size++;
                        }

                        if ((x == gridSize.X || x == 0) || (y == gridSize.Y || y == 0))
                        {
                            areas.First(a => a.Id == point.ClosestTo).infinite = true;
                        }
                    }
                }

                return areas.Where(a => !a.infinite).Max(z => z.Size);
            }
        }
    }
}
