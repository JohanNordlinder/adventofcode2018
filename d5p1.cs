﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D5P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(10, new Program().RunChallenge("dabAcCaCBAcCcaDA"));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_5.txt").ToList().First();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {

            public int RunChallenge(string input)
            {
                var modified = input;
                var reaction = false;

                do
                {
                    reaction = false;

                    for (int i = 0; i < modified.Length - 1; i++)
                    {
                        if ((
                            (modified[i].ToString().ToLower() == modified[i+1].ToString().ToLower()) &&
                            (
                                (char.IsLower(modified[i]) && char.IsUpper(modified[i + 1])) ||
                                (char.IsUpper(modified[i]) && char.IsLower(modified[i + 1]))
                            )
                           ))
                        {
                            modified = modified.Remove(i, 2);
                            reaction = true;
                            break;
                        }
                    }
                } while (reaction);

                return modified.Count();
            }
        }
    }
}
