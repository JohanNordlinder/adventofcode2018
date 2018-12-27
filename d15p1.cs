using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D15P1
    {

        [TestMethod]
        public void TestRunDijkstras1()
        {
            TestRunDijkstrasBase("d_15_d_1.txt", Tuple.Create(1, 1), Tuple.Create(7, 5), 10);
        }

        private void TestRunDijkstrasBase(string file, Tuple<int,int> start, Tuple<int, int> end, int expectedShortestPath)
        {
            var input = System.IO.File.ReadAllLines(file).ToArray();
            var program = new Program();
            program.Setup(input);
            var shortestPaths = Dijkstras.GetShortestPath(program.grid.Values.Where(z => !z.IsWall), program.grid[start].Location, program.grid[end].Location);
            if (shortestPaths != null)
            {
                shortestPaths.ToList().ForEach(path =>
                {
                    program.PrintGridWithPath(path.Select(step => step.Location).ToList());
                });
            }
            Assert.AreEqual(expectedShortestPath, shortestPaths != null ? shortestPaths.First().Count() : -1);
        }

        [TestMethod]
        public void TestRunDijkstras2()
        {
            TestRunDijkstrasBase("d_15_d_2.txt", Tuple.Create(4, 1), Tuple.Create(4, 5), 6);
        }

        [TestMethod]
        public void TestRunDijkstras3()
        {
            TestRunDijkstrasBase("d_15_d_3.txt", Tuple.Create(2, 6), Tuple.Create(26, 29), 47);
        }

        [TestMethod]
        public void TestRunDijkstras4()
        {
            TestRunDijkstrasBase("d_15_d_4.txt", Tuple.Create(1, 1), Tuple.Create(4, 5), -1);
        }


        [TestMethod]
        public void TestRun_Movement()
        {
            var input = System.IO.File.ReadAllLines("d_15_t_1.txt").ToArray();

            var program = new Program();
            program.Setup(input);
            program.RunGame();
        }

        [TestMethod]
        public void TestRun_Combat1()
        {
            var input = System.IO.File.ReadAllLines("d_15_t_2.txt").ToArray();
            var program = new Program();
            program.Setup(input);
            Assert.AreEqual(27730, program.RunGame());
        }

        [TestMethod]
        public void TestRun_Combat2()
        {
            var input = System.IO.File.ReadAllLines("d_15_t_3.txt").ToArray();
            var program = new Program();
            program.Setup(input);
            Assert.AreEqual(36334, program.RunGame());
        }

        [TestMethod]
        public void TestRun_Combat3()
        {
            var input = System.IO.File.ReadAllLines("d_15_t_4.txt").ToArray();
            var program = new Program();
            program.Setup(input);
            Assert.AreEqual(39514, program.RunGame());
        }

        [TestMethod]
        public void TestRun_Combat4()
        {
            var input = System.IO.File.ReadAllLines("d_15_t_5.txt").ToArray();
            var program = new Program();
            program.Setup(input);
            Assert.AreEqual(27755, program.RunGame());
        }

        [TestMethod]
        public void TestRun_Combat5()
        {
            var input = System.IO.File.ReadAllLines("d_15_t_6.txt").ToArray();
            var program = new Program();
            program.Setup(input);
            Assert.AreEqual(28944, program.RunGame());
        }

        [TestMethod]
        public void TestRun_Combat6()
        {
            var input = System.IO.File.ReadAllLines("d_15_t_7.txt").ToArray();
            var program = new Program();
            program.Setup(input);
            Assert.AreEqual(18740, program.RunGame());
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_15.txt").ToArray();
            var program = new Program();
            program.Setup(input);
            Console.WriteLine("Result: " + program.RunGame());
        }
       

        public class Program
        {

            public class Unit
            {
                public enum UnitTypes { Goblin, Elf }
                public int Health = 200;
                public Coordinate Location { set; get; }
                public UnitTypes UnitType;
                public bool IsDead = false;
            }

            public class Square
            {
                public Coordinate Location { set; get; }
                public bool IsWall { set; get; }
            }

            private int GridSizeX;
            private int GridSizeY;
            public List<Unit> units = new List<Unit>();
            public Dictionary<Tuple<int, int>, Square> grid = new Dictionary<Tuple<int, int>, Square>();

            private class CombatEnds : Exception {};

            public void Setup(string[] input)
            {
                GridSizeX = input[0].Length;
                GridSizeY = input.Length;
                ParseInput(input);
                PrintGrid();
            }

            public int RunGame()
            {
                var rounds = 0;

                try
                {
                    do
                    {
                        rounds++;
                        System.Diagnostics.Trace.WriteLine("Round Begins: " + rounds);
                        units.OrderBy(u => u.Location.Y).ThenBy(u => u.Location.X).ToList().ForEach(unit =>
                        {
                            if (!unit.IsDead)
                            {

                                var targets = units.Where(u => !u.IsDead && u.UnitType != unit.UnitType);

                                if (!targets.Any())
                                {
                                    throw new CombatEnds();
                                }

                                Func<IEnumerable<Unit>> unitsInRange = () => targets.Where(target => Math.Abs(target.Location.X - unit.Location.X) + Math.Abs(target.Location.Y - unit.Location.Y) == 1);
                                Action attack = () => {
                                    var selectedTarget = unitsInRange().OrderBy(u => u.Health).ThenBy(u => u.Location.Y).ThenBy(u => u.Location.X).First();
                                    selectedTarget.Health -= 3;
                                    selectedTarget.IsDead = selectedTarget.Health <= 0;
                                };

                                if (unitsInRange().Any())
                                {
                                    attack();
                                }
                                else
                                {
                                    var paths = targets.Select(target => 
                                        Dijkstras.GetShortestPath(
                                            grid
                                                .Where(square => !square.Value.IsWall && !IsThereUnitOnThisSquare(square.Value, new List<Unit> { unit, target }))
                                                .Select(sq => sq.Value),
                                            unit.Location,
                                            target.Location
                                        )
                                    );
                                    
                                    // Move
                                    var canFindReachablTargetSquareNextToAnyTarget = paths.Any(d => d != null);

                                    if (canFindReachablTargetSquareNextToAnyTarget)
                                    {

                                        unit.Location = paths.Where(d => d != null) // Ignore units where there is no path
                                                        .OrderBy(x => x.Min(path => path.Count())) // Find the pathGroup to any unit where the number of steps is least
                                                        .ThenBy(z => z.First().First().Location.Y) // Take the path within the group which is first in reading order
                                                        .ThenBy(z => z.First().First().Location.X) // Take the path within the group which is first in reading order
                                                        //.OrderBy(path => path[0].Location.X)
                                                        //.ThenBy(path => path[0].Location.Y)
                                                        .First() // Take the closest unit
                                                        .First() // Take the path with the first step which is in reading order
                                                        .First() // Take first step along path
                                                        .Location;

                                        // TODO: Check if units are in range again and attack

                                    }

                                    // After movement check again if there are targets
                                    if (unitsInRange().Any())
                                    {
                                        attack();
                                    }
                                }
                            }
                        });
                        System.Diagnostics.Trace.WriteLine("Round ends: " + rounds);
                        PrintGrid();
                    } while (true);
                }
                catch (CombatEnds e)
                {
                    System.Diagnostics.Trace.WriteLine("Program stop in the middle of round " + rounds);
                    PrintGrid();
                    return (rounds - 1) * units.Where(z => !z.IsDead).Sum(u => u.Health); // Last round never completed, or maybe it did if it was the last move of the round that finished it !?!?! and how to know if it was?..
                }

                return -1;
            }

            private bool IsThereUnitOnThisSquare(Square square, List<Unit> ignoreTheseUnits)
            {
                return units.Any(unit => !ignoreTheseUnits.Contains(unit) && !unit.IsDead && unit.Location.X == square.Location.X && unit.Location.Y == square.Location.Y);
            }

            private void ParseInput(string[] input)
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    for (int x = 0; x < GridSizeX; x++)
                    {
                        var raw = input[y][x];

                        var newSquare = new Square { Location = new Coordinate { X = x, Y = y } };

                        grid.Add(Tuple.Create(x, y), newSquare);

                        switch (raw)
                        {
                            case '#':
                                newSquare.IsWall = true;
                                break;
                            case '.':
                                newSquare.IsWall = false;
                                break;
                            case 'E':
                                units.Add(new Unit { Location = newSquare.Location, UnitType = Unit.UnitTypes.Elf });
                                newSquare.IsWall = false;
                                break;
                            case 'G':
                                units.Add(new Unit { Location = newSquare.Location, UnitType = Unit.UnitTypes.Goblin });
                                newSquare.IsWall = false;
                                break;
                            default:
                                throw new Exception("Cannot parse Square");
                        }
                    }
                }
            }

            private void PrintGrid()
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    for (int x = 0; x < GridSizeX; x++)
                    {
                        var unit = units.FirstOrDefault(u => !u.IsDead && u.Location.X == x && u.Location.Y == y);
                        var outPut = unit != null ? (unit.UnitType == Unit.UnitTypes.Elf ? "E" : "G") : (grid[Tuple.Create(x, y)].IsWall ? "#" : ".");
                        System.Diagnostics.Trace.Write(outPut);
                    }
                    System.Diagnostics.Trace.Write(Environment.NewLine);
                }
            }

            public void PrintGridWithPath(List<Coordinate> path = null)
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    for (int x = 0; x < GridSizeX; x++)
                    {
                        var unit = path.FirstOrDefault(u => u.X == x && u.Y == y);
                        var outPut = unit != null ? "*" : (grid[Tuple.Create(x, y)].IsWall ? "#" : ".");
                        System.Diagnostics.Trace.Write(outPut);
                    }
                    System.Diagnostics.Trace.Write(Environment.NewLine);
                }
            }
        }

        class Dijkstras
        {
            public class Node
            {
                public Coordinate Location { set; get; }
                public int Distance { set; get; }
                public List<Node> Ancestors { set; get; }
                public bool Visited { set; get; }
            }

            public static IEnumerable<IEnumerable<Node>> GetShortestPath(IEnumerable<Program.Square> grid, Coordinate start, Coordinate end)
            {
                var nodes = new Dictionary<Coordinate, Node>();

                grid.ToList().ForEach(z => nodes.Add(z.Location, new Node
                {
                    Location = z.Location,
                    Distance = int.MaxValue,
                    Ancestors = new List<Node>(),
                    Visited = false
                }));

                var startNode = nodes[start];
                startNode.Distance = 0;
                //startNode.Visited = true;

                while(nodes.Any(node => !node.Value.Visited))
                {
                    var currentNodeIFExists = nodes.Where(n => !n.Value.Visited && n.Value.Distance != int.MaxValue).OrderBy(n => n.Value.Distance);

                    if (!currentNodeIFExists.Any())
                    {
                        // If there are nodes left but not reachable
                        break;
                    }

                    var currentNode = currentNodeIFExists.First();

                    var nearByNodes = nodes.Where(n => !n.Value.Visited && n.Key != currentNode.Key && (Math.Abs(n.Key.X - currentNode.Key.X) + Math.Abs(n.Key.Y - currentNode.Key.Y) == 1));

                    foreach (var nearbynode in nearByNodes)
                    {
                        if (nearbynode.Value.Distance >= currentNode.Value.Distance + 1)
                        {
                            nearbynode.Value.Distance = currentNode.Value.Distance + 1;

                            nearbynode.Value.Ancestors.Add(currentNode.Value);
                            if(nearbynode.Value.Ancestors.Count() > 1) {
                                nearbynode.Value.Ancestors.Remove(nearbynode.Value.Ancestors.OrderBy(z => z.Location.Y).ThenBy(z => z.Location.X).Last());
                            }
                        }
                    }
                    currentNode.Value.Visited = true;
                }

                var endnode = nodes[end];

                if (endnode.Distance == int.MaxValue)
                {
                    return null;
                }

                var paths = new List<List<Node>>();
                paths.Add(new List<Node> { endnode });

                FindParentPath(paths, paths.First(), endnode);

                /*while (path.Last().Ancestors.Any())
                {
                    path.Add(path.Last().Ancestor);
                }
                */

                paths.ForEach(z => z.Reverse());

                return paths.Select(z => z.Skip(1));
            }

            private static void FindParentPath(List<List<Node>> paths, List<Node> thisPath, Node node)
            {
                if (node.Ancestors.Any())
                {
                    if (node.Ancestors.Count > 1)
                    {
                        node.Ancestors.Skip(1).ToList().ForEach(z =>
                        {
                            var newPath = new List<Node>();
                            newPath.AddRange(thisPath);
                            newPath.Add(z);
                            paths.Add(newPath);
                            FindParentPath(paths, newPath, z);
                        });
                    }

                    thisPath.Add(node.Ancestors.First());
                    FindParentPath(paths, thisPath, node.Ancestors.First());
                }

                return;
            }
        }
    }
}
