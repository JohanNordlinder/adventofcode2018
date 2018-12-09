using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D9P2
    {

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new D9P1.Program().RunChallenge(477, 70851 * 100));
        }
    }
}
