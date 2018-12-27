using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D18P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_18_t_1.txt").ToList();
            Assert.AreEqual(1147, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_18.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public class Acre
            {
                public enum AcreType { UNKNOWN, OPEN, TREES, LUMBERYARD}
                public AcreType TypeThisMinute { set; get; }
                public AcreType TypeLastMinute { set; get; }
            }

            public int RunChallenge(List<string> input)
            {
                var grid = new Dictionary<Tuple<int, int>, Acre>();
                var extents = new { MaxX = input[0], MaxY = input.Count() };

                for (int y = 0; y < extents.MaxY; y++)
                {
                    for (int x = 0; x < extents.MaxX.Length; x++)
                    {
                        var raw = input[y][x];
                        Acre.AcreType type = Acre.AcreType.UNKNOWN;

                        switch(raw)
                        {
                            case '.':
                                type = Acre.AcreType.OPEN;
                                break;
                            case '|':
                                type = Acre.AcreType.TREES;
                                break;
                            case '#':
                                type = Acre.AcreType.LUMBERYARD;
                                break;
                        }
                        grid.Add(Tuple.Create(x, y), new Acre { TypeThisMinute = type });
                    }
                }

                Action printAll = () =>
                {
                    for (int y = 0; y < extents.MaxY; y++)
                    {
                        for (int x = 0; x < input[0].Length; x++)
                        {
                            var type = grid[Tuple.Create(x, y)].TypeThisMinute;
                            System.Diagnostics.Trace.Write(type == Acre.AcreType.OPEN ? "." : type == Acre.AcreType.TREES ? "|" : "#");
                        }
                        System.Diagnostics.Trace.Write(Environment.NewLine);
                    }
                };

                printAll();

                for (int minutes = 1; minutes <= 10; minutes++)
                {
                    foreach (var acre in grid)
                    {
                        // Push into history
                        acre.Value.TypeLastMinute = acre.Value.TypeThisMinute;
                    }

                    foreach (var acre in grid)
                    {
                        var newType = Acre.AcreType.UNKNOWN;

                        switch(acre.Value.TypeLastMinute)
                        {
                            case Acre.AcreType.OPEN:
                                newType = NumerOfAcreTypeAdjacent(grid, acre, Acre.AcreType.TREES) >= 3 ? Acre.AcreType.TREES : acre.Value.TypeLastMinute;
                                break;
                            case Acre.AcreType.TREES:
                                newType = NumerOfAcreTypeAdjacent(grid, acre, Acre.AcreType.LUMBERYARD) >= 3 ? Acre.AcreType.LUMBERYARD : acre.Value.TypeLastMinute;
                                break;
                            case Acre.AcreType.LUMBERYARD:
                                newType = (NumerOfAcreTypeAdjacent(grid, acre, Acre.AcreType.LUMBERYARD) >= 1 && NumerOfAcreTypeAdjacent(grid, acre, Acre.AcreType.TREES) >= 1) ? Acre.AcreType.LUMBERYARD : Acre.AcreType.OPEN;
                                break;
                        }

                        acre.Value.TypeThisMinute = newType;
                    }
                    System.Diagnostics.Trace.WriteLine("After minutes: " + minutes);
                    printAll();
                }
                
                return grid.Count(a => a.Value.TypeThisMinute == Acre.AcreType.TREES) * grid.Count(a => a.Value.TypeThisMinute == Acre.AcreType.LUMBERYARD);
            }

            private int NumerOfAcreTypeAdjacent(Dictionary<Tuple<int, int>, Acre> grid, KeyValuePair<Tuple<int, int>, Acre> acre, Acre.AcreType acreType)
            {
                Func<int,int,bool> checkAdjacent = (int x, int y) => {
                    Acre adjucent;
                    var exists = grid.TryGetValue(Tuple.Create(x, y), out adjucent);
                    return exists ? adjucent.TypeLastMinute == acreType : false;
                };
                int count = 0;
                count = checkAdjacent(acre.Key.Item1 - 1, acre.Key.Item2 - 1) ? count + 1 : count; // TOP LEFT
                count = checkAdjacent(acre.Key.Item1, acre.Key.Item2 - 1) ? count + 1 : count; // TOP
                count = checkAdjacent(acre.Key.Item1 + 1, acre.Key.Item2 - 1) ? count + 1 : count; // TOP RIGHT
                count = checkAdjacent(acre.Key.Item1 - 1, acre.Key.Item2) ? count + 1 : count; // LEFT
                count = checkAdjacent(acre.Key.Item1 + 1, acre.Key.Item2) ? count + 1 : count; // RIGHT
                count = checkAdjacent(acre.Key.Item1 - 1, acre.Key.Item2 + 1) ? count + 1 : count; // BOTTOM LEFT
                count = checkAdjacent(acre.Key.Item1, acre.Key.Item2 + 1) ? count + 1 : count; // BOTTOM
                count = checkAdjacent(acre.Key.Item1 + 1, acre.Key.Item2 + 1) ? count + 1 : count; // BOTTOM RIGHT
                return count;
            }
        }
    }
}
