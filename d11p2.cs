using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D11P2
    {

        [TestMethod]
        public void TestRun()
        {
            //Assert.AreEqual("90,269,16", new Program().RunChallenge(18));
            Assert.AreEqual("232,251,12", new Program().RunChallenge(42));
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

                var gridSize = 0;
                var maxCellX = 0;
                var maxCellY = 0;
                var maxWeight = 0;

                for (int i = 1; i <= 300; i++)
                {
                    
                    var cell = findMax(powerCells, i);
                    if (cell.Weight > maxWeight)
                    {
                        maxWeight = cell.Weight;
                        maxCellX = cell.X;
                        maxCellY = cell.Y;
                        gridSize = i;
                    }
                }


                return (maxCellX) + "," + (maxCellY) + "," + gridSize;
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

            public static PowerCell findMax(PowerCell[,] powerCells, int gridSize)
            {
                PowerCell maxCell = new PowerCell();

                for (int x = 1; x <= 300 - (gridSize - 1); x++)
                {
                    for (int y = 1; y <= 300 - (gridSize - 1); y++)
                    {
                        var cell = powerCells[x, y];
                        cell.Weight = CalculateWeight(powerCells, x, y, gridSize, gridSize);

                        if (cell.Weight > maxCell.Weight)
                        {
                            maxCell = cell;
                        }
                    }
                }

                return maxCell;
            }

            public static int CalculateWeight(PowerCell[,] powerCells, int startX, int startY, int sizeX, int sizeY)
            {
                var tot = 0;

                for (int x = startX; x < startX + sizeX; x++)
                {
                    for (int y = startY; y < startY + sizeY; y++)
                    {
                        tot += powerCells[x, y].PowerLevel;
                    }
                }

                return tot;
            }
        }
    }
}
