using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D1P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(3, new Program().RunChallenge(new List<string> { "+1", "-2", "+3", "+1" }));
            Assert.AreEqual(3, new Program().RunChallenge(new List<string> { "+1", "+1", "+1" }));
            Assert.AreEqual(0, new Program().RunChallenge(new List<string> { "+1", "+1", "-2" }));
            Assert.AreEqual(-6, new Program().RunChallenge(new List<string> { "-1", "-2", "-3" }));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_1.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var current = 0;
                input.ForEach(z => current = current + int.Parse(z));
                return current;
            }
        }
    }
}
