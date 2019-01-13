using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D22P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(114, new Program().RunChallenge(510, 10, 10));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(10647, 7, 770));
        }

        public class Program
        {
            public Region[,] Regions;
            public int TotalRiskLevel = 0;

            public int RunChallenge(int depth, int targetX, int targetY)
            {
                Regions = new Region[targetX + 1, targetY + 1];

                Func<Region> startAndFinish = () => new Region { GeologicalIndex = 0, ErosionLevel = (depth % 20183), Type = GetTypeFromErosionLevel((depth % 20183)) };
                Regions[0, 0] = startAndFinish();
                Regions[targetX, targetY] = startAndFinish();

                for (int y = 0; y <= targetY; y++)
                {
                    var geoIndex = y * 48271;
                    var erosionLevel = (depth + geoIndex) % 20183;
                    Region.RegionType type = GetTypeFromErosionLevel(erosionLevel);
                    Regions[0, y] = new Region { GeologicalIndex = geoIndex, ErosionLevel = erosionLevel, Type = type };
                }

                for (int x = 0; x <= targetX; x++)
                {
                    var geoIndex = x * 16807;
                    var erosionLevel = (depth + geoIndex) % 20183;
                    Region.RegionType type = GetTypeFromErosionLevel(erosionLevel);
                    Regions[x, 0] = new Region { GeologicalIndex = geoIndex, ErosionLevel = erosionLevel, Type = type };
                }

                for (int x = 1; x <= targetX; x++)
                {
                    for (int y = 1; y <= targetY; y++)
                    {
                        if (x == targetX && y == targetY)
                        {
                            continue;
                        }
                        var geoIndex = Regions[x - 1, y].ErosionLevel * Regions[x, y - 1].ErosionLevel;
                        var erosionLevel = (depth + geoIndex) % 20183;
                        Region.RegionType type = GetTypeFromErosionLevel(erosionLevel);
                        Regions[x, y] = new Region { GeologicalIndex = geoIndex, ErosionLevel = erosionLevel, Type = type };
                    }
                }

                return TotalRiskLevel;
            }

            public Region.RegionType GetTypeFromErosionLevel(int erosionLevel)
            {
                Region.RegionType type = Region.RegionType.NULL;
                switch (erosionLevel % 3)
                {
                    case 0:
                        type = Region.RegionType.ROCKY;
                        break;
                    case 1:
                        type = Region.RegionType.WET;
                        TotalRiskLevel++;
                        break;
                    case 2:
                        type = Region.RegionType.NARROW;
                        TotalRiskLevel += 2;
                        break;
                }

                return type;
            }
        }

        public class Region
        {
            public enum RegionType { NULL, ROCKY, NARROW, WET}
            public RegionType Type { set; get; }
            public int ErosionLevel { set; get; }
            public int GeologicalIndex { set; get; }
        }
    }
}
