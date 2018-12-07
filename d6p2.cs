using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D6P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_6_t_1.txt").ToList();
            Assert.AreEqual(16, new Program().RunChallenge(input, 32));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_6.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input, 10000));
        }

        public class Program
        {
            public int RunChallenge(List<string> input, int maxCombinedDistance)
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

                int count = 0;

                for (int x = 0; x < gridSize.X; x++)
                {
                    for (int y = 0; y < gridSize.Y; y++)
                    {
                        var distance = 0;
                        for (int c = 0; c < coordinates.Length; c++)
                        {
                            var coord = coordinates[c];
                            distance = distance + Math.Abs(coord.X - x) + Math.Abs(coord.Y - y);
                        }

                        if (distance < maxCombinedDistance)
                        {
                            count++;

                        }
                    }
                }

                return count;
            }
        }
    }
}
