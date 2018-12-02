using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D2P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_2_t_1.txt").ToList();
            Assert.AreEqual(12, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_2.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {

            public int RunChallenge(List<string> input)
            {
                Func<int, int> findFunc = (count) => input.Where(boxId => boxId.Any(character => boxId.Count(otherchar => otherchar == character) == count)).Count();
                return findFunc(2) * findFunc(3);
            }
        }
    }
}
