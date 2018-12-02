using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D2P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_2_t_2.txt").ToList();
            Assert.AreEqual("fgij", new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_2.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {

            public string RunChallenge(List<string> input)
            {
                foreach (var boxId1 in input) {
                    foreach (var boxId2 in input)
                    {
                        var diffcount = 0;
                        var matchString = "";
                        for (int i = 0; i < boxId1.Length; i++)
                        {
                            if (boxId1[i] == boxId2[i])
                            {
                                matchString += boxId1[i];
                            } else
                            {
                                diffcount++;
                            }
                        }

                        if (diffcount == 1)
                        {
                            return matchString;
                        }
                    }
                }
                throw new Exception("Could not find a match");
            }
        }
    }
}
