using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D25P1
    {

        [TestMethod]
        public void TestRun1()
        {
            var input = System.IO.File.ReadAllLines("d_25_t_1.txt").ToList();
            Assert.AreEqual(2, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void TestRun2()
        {
            var input = System.IO.File.ReadAllLines("d_25_t_2.txt").ToList();
            Assert.AreEqual(4, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void TestRun3()
        {
            var input = System.IO.File.ReadAllLines("d_25_t_3.txt").ToList();
            Assert.AreEqual(3, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void TestRun4()
        {
            var input = System.IO.File.ReadAllLines("d_25_t_4.txt").ToList();
            Assert.AreEqual(8, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_25.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var points = new List<int[]>();
                foreach (var raw in input)
                {
                    var parsed = raw.Split(new string[] { "," }, StringSplitOptions.None).ToArray();
                    points.Add(parsed.Select(z => int.Parse(z)).ToArray());
                }
                var groups = new List<List<int[]>>();

                foreach(var point in points)
                {
                    var addedToGroups = new List<List<int[]>>();
                    var foundGroupForPoint = false;
                    foreach (var group in groups)
                    {
                        if (group.Any(p => IsConnected(p, point)))
                        {
                            group.Add(point);
                            foundGroupForPoint = true;
                            addedToGroups.Add(group);
                        }
                    }
                    if (addedToGroups.Count > 1)
                    {
                        addedToGroups.ForEach(g => groups.Remove(g));
                        groups.Add(addedToGroups.SelectMany(z => z).ToList());
                    }
                    if (!foundGroupForPoint)
                    {
                        groups.Add(new List<int[]>() { point });
                    }
                }
                return groups.Count();
            }

            public static bool IsConnected(int[] point1, int[] point2)
            {
                return (Math.Abs(point1[0] - point2[0]) + Math.Abs(point1[1] - point2[1]) + Math.Abs(point1[2] - point2[2]) + Math.Abs(point1[3] - point2[3])) <= 3;
            }
        }
    }
}
