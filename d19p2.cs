using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D19P2
    {
        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge());
        }

        public class Program
        {
            public int RunChallenge()
            {
                var count = 0;
                for (int i = 1; i <= 10551425; i++)
                {
                    if (10551425 % i == 0)
                    {
                        count += i;
                    }
                }

                return count;
            }
        }
    }
}
