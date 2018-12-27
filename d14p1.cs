using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D14P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual("5158916779", new Program().RunChallenge(9));
            Assert.AreEqual("0124515891", new Program().RunChallenge(5));
            Assert.AreEqual("9251071085", new Program().RunChallenge(18));
            Assert.AreEqual("5941429882", new Program().RunChallenge(2018));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(598701));
        }

        public class Program
        {
            private string values = "37";
            private int Elf1 = 0;
            private int Elf2 = 1;

            public string RunChallenge(int recepies)
            {
                do
                {
                    var elf1value = int.Parse(values[Elf1].ToString());
                    var elf2value = int.Parse(values[Elf2].ToString());

                    values += elf1value + elf2value;

                    Elf1 = (Elf1 + elf1value + 1) % values.Length;
                    Elf2 = (Elf2 + elf2value + 1) % values.Length;

                } while (values.Length < recepies + 10);

                return values.Substring(recepies, 10);
            }
        }
    }
}
