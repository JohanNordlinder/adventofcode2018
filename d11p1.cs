﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D11P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(4, Program.CalculatePowerLevel(3, 5, 8));
            Assert.AreEqual(-5, Program.CalculatePowerLevel(122, 79, 57));
            Assert.AreEqual(0, Program.CalculatePowerLevel(217, 196, 39));
            Assert.AreEqual(4, Program.CalculatePowerLevel(101, 153, 71));

            Assert.AreEqual("33,45", new Program().RunChallenge(18));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(5719));
        }

        public class Program
        {
            public class PowerCell
            {
                public int PowerLevel = 0;
                public int Weight = 0;
                public int X = 0;
                public int Y = 0;
            }

            public string RunChallenge(int serial)
            {
                var powerCells = new PowerCell[301, 301];

                for (int x = 1; x <= 300; x++)
                {
                    for (int y = 1; y <= 300; y++)
                    {
                        var cell = new PowerCell { X = x, Y = y };
                        cell.PowerLevel = CalculatePowerLevel(x, y, serial);
                        powerCells[x, y] = cell;
                    }
                }

                PowerCell maxCell = new PowerCell();

                for (int x = 1; x <= 298; x++)
                {
                    for (int y = 1; y <= 298; y++)
                    {
                        var cell = powerCells[x, y];
                        cell.Weight = CalculateWeight(powerCells, x, y);

                        if(cell.Weight > maxCell.Weight)
                        {
                            maxCell = cell;
                        }
                    }
                }

                return (maxCell.X) + "," + (maxCell.Y);
            }

            public static int CalculatePowerLevel(int x, int y, int serial)
            {
                var rackId = x + 10;
                var powerLevel = rackId * y;
                powerLevel += serial;
                powerLevel *= rackId;
                var hundred = powerLevel < 100 ? 0 : (powerLevel % 1000 - powerLevel % 100) / 100;
                return hundred - 5;
            }

            public static int CalculateWeight(PowerCell[,] powerCells, int startX, int startY)
            {
                var tot = 0;

                for (int x = startX; x < startX + 3; x++)
                {
                    for (int y = startY; y < startY + 3; y++)
                    {
                        tot += powerCells[x, y].PowerLevel;
                    }
                }

                return tot;
            }
        }
    }
}
