using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D8P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_8_t_1.txt").First().Split(' ').Select(z => int.Parse(z)).ToArray();
            Assert.AreEqual(138, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_8.txt").First().Split(' ').Select(z => int.Parse(z)).ToArray();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            private int Sum = 0;
            private int[] Input = null;

            public int RunChallenge(int[] input)
            {
                Input = input;
                RunProgram(0);
                return Sum;
            }

            private int RunProgram(int startIndex)
            {
                var numberOfSubPrograms = Input[startIndex];
                var numberOfMetadata = Input[startIndex + 1];
                var lastSubProgramEnd = startIndex + 2;

                for (int i = 0; i < numberOfSubPrograms; i++)
                {
                    lastSubProgramEnd = RunProgram(lastSubProgramEnd);
                }

                for (int i = 0; i < numberOfMetadata; i++)
                {
                    Sum += Input[lastSubProgramEnd + i];
                }

                return lastSubProgramEnd + numberOfMetadata;
            }
        }
    }
}
