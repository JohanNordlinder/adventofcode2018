using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D8P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_8_t_1.txt").First().Split(' ').Select(z => int.Parse(z)).ToArray();
            Assert.AreEqual(66, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_8.txt").First().Split(' ').Select(z => int.Parse(z)).ToArray();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        private class SubProgramResult
        {
            public int Value { set; get; }
            public int EndIndex { set; get; }
        }

        public class Program
        {
            private int[] Input = null;

            public int RunChallenge(int[] input)
            {
                Input = input;
                return RunProgram(0).Value;
            }

            private SubProgramResult RunProgram(int startIndex)
            {
                var numberOfSubPrograms = Input[startIndex];
                var numberOfMetadata = Input[startIndex + 1];
                var lastSubProgramEnd = startIndex + 2;
                var thisProgramValue = 0;
                var subProgramValues = new int[numberOfSubPrograms];

                for (int i = 0; i < numberOfSubPrograms; i++)
                {
                    var subProgramResult = RunProgram(lastSubProgramEnd);
                    lastSubProgramEnd = subProgramResult.EndIndex;
                    subProgramValues[i] = subProgramResult.Value;
                }

                if (numberOfSubPrograms > 0)
                {
                    for (int i = 0; i < numberOfMetadata; i++)
                    {
                        var metaDataValue = Input[lastSubProgramEnd + i];
                        if (subProgramValues.ElementAtOrDefault(metaDataValue - 1) != 0)
                        {
                            thisProgramValue += subProgramValues[metaDataValue - 1];
                        }

                    }
                } else
                {

                    for (int i = 0; i < numberOfMetadata; i++)
                    {
                        thisProgramValue += Input[lastSubProgramEnd + i];
                    }
                }
                
                return new SubProgramResult {
                    EndIndex = lastSubProgramEnd + numberOfMetadata,
                    Value = thisProgramValue
                };
            }
        }
    }
}
