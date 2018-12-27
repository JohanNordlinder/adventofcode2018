using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D14P2
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(9, new Program2().RunChallenge("51589"));
            Assert.AreEqual(5, new Program2().RunChallenge("01245"));
            Assert.AreEqual(18, new Program2().RunChallenge("92510"));
            Assert.AreEqual(2018, new Program2().RunChallenge("59414"));
            Assert.AreEqual(10, new Program2().RunChallenge("15891"));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program2().RunChallenge("598701"));
        }

        public class Program
        {
            private string values = "37";
            private int Elf1 = 0;
            private int Elf2 = 1;

            public int RunChallenge(string searched)
            {
                do
                {
                    var elf1value = int.Parse(values[Elf1].ToString());
                    var elf2value = int.Parse(values[Elf2].ToString());

                    values += elf1value + elf2value;

                    Elf1 = (Elf1 + elf1value + 1) % values.Length;
                    Elf2 = (Elf2 + elf2value + 1) % values.Length;

                }
                while (values.Length < 10 || values.Substring(values.Length - searched.Length) != searched);

                return values.Length - searched.Length;
            }
        }

        public class Program2
        {
            private char[] values = new char[500000000];
            private long Elf1 = 0L;
            private long Elf2 = 1L;
            private long currentCount = 2;

            public long RunChallenge(string searched)
            {
                values[0] = '3';
                values[1] = '7';

                var lastMatch = false;
                var closeToLastMatch = false;

                do
                {
                    var elf1value = int.Parse(values[Elf1].ToString());
                    var elf2value = int.Parse(values[Elf2].ToString());

                    var newValue = elf1value + elf2value;

                    newValue.ToString().ToArray().ToList().ForEach(n =>
                    {
                        values[currentCount] = n;
                        currentCount++;
                    }
                    );

                    Elf1 = (Elf1 + elf1value + 1) % currentCount;
                    Elf2 = (Elf2 + elf2value + 1) % currentCount;

                    lastMatch = currentCount > searched.Length && GetLast(searched.Length, currentCount) == searched;
                    closeToLastMatch = currentCount > searched.Length && GetLast(searched.Length, currentCount - 1) == searched;
                }
                while (!lastMatch && !closeToLastMatch);

                return currentCount - searched.Length + (closeToLastMatch ? -1 : 0);
            }

            private string GetLast(int numberOfLast, long endSearchAt)
            {
                var toReturn = "";
                for (long i = endSearchAt - numberOfLast; i < endSearchAt; i++)
                {
                    toReturn += values[i];
                }
                return toReturn;
            }
        }
    }
}
