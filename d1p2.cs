using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D1P2
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(2, new Program().RunChallenge(new List<string> { "+1", "-2", "+3", "+1" }));
            Assert.AreEqual(0, new Program().RunChallenge(new List<string> { "+1", "-1" }));
            Assert.AreEqual(10, new Program().RunChallenge(new List<string> { "+3", "+3", "+4", "-2", "-4" }));
            Assert.AreEqual(5, new Program().RunChallenge(new List<string> { "-6", "+3", "+8", "+5", "-6" }));
            Assert.AreEqual(14, new Program().RunChallenge(new List<string> { "+7", "+7", "-2", "-7", "-4" }));  
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_1.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            private List<int> log = new List<int>();

            public int RunChallenge(List<string> input)
            {
                var current = 0;
                do
                {
                    foreach(var change in input)
                    {
                        if (log.Contains(current))
                        {
                            return current;
                        }
                        else
                        {
                            log.Add(current);
                        }
                        current = current + int.Parse(change);
                    }
                } while (true);
            }
        }
    }
}
