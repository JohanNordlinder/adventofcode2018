using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AdventOfCode2018
{
    [TestClass]
    public class D22P2
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(45, new Program().RunChallenge(510, 10, 10));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(10647, 7, 770));
        }

        public class Program
        {
            public Region[,] regions;

            public int RunChallenge(int depth, int targetX, int targetY)
            {
                var size = new { X = 50, Y = 800 }; // Guestimates from failed attempts, should make gridsize dynamic...
                regions = new Region[size.X + 1, size.Y + 1];

                Func<Region> startAndFinish = () => new Region { GeologicalIndex = 0, ErosionLevel = (depth % 20183), Type = GetTypeFromErosionLevel((depth % 20183)) };
                regions[0, 0] = startAndFinish();
                regions[targetX, targetY] = startAndFinish();

                for (int y = 0; y <= size.Y; y++)
                {
                    var geoIndex = y * 48271;
                    var erosionLevel = (depth + geoIndex) % 20183;
                    Region.RegionType type = GetTypeFromErosionLevel(erosionLevel);
                    regions[0, y] = new Region { GeologicalIndex = geoIndex, ErosionLevel = erosionLevel, Type = type };
                }

                for (int x = 0; x <= size.X; x++)
                {
                    var geoIndex = x * 16807;
                    var erosionLevel = (depth + geoIndex) % 20183;
                    Region.RegionType type = GetTypeFromErosionLevel(erosionLevel);
                    regions[x, 0] = new Region { GeologicalIndex = geoIndex, ErosionLevel = erosionLevel, Type = type };
                }

                for (int x = 1; x <= size.X; x++)
                {
                    for (int y = 1; y <= size.Y; y++)
                    {
                        if (x == targetX && y == targetY)
                        {
                            continue;
                        }
                        var geoIndex = regions[x - 1, y].ErosionLevel * regions[x, y - 1].ErosionLevel;
                        var erosionLevel = (depth + geoIndex) % 20183;
                        Region.RegionType type = GetTypeFromErosionLevel(erosionLevel);
                        regions[x, y] = new Region { GeologicalIndex = geoIndex, ErosionLevel = erosionLevel, Type = type };
                    }
                }

                //PrintAll();

                var shortestPath = Dijkstras.GetShortestPath(regions, new Coordinate { X = 0, Y = 0 }, new Coordinate { X = targetX, Y = targetY });

                return shortestPath;
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
                        break;
                    case 2:
                        type = Region.RegionType.NARROW;
                        break;
                }

                return type;
            }

            private void PrintAll()
            {
                for (int y = 0; y < regions.GetLength(1); y++)
                {
                    for (int x = 0; x < regions.GetLength(0); x++)
                    {
                        Region region = regions[x, y];
                        System.Diagnostics.Trace.Write(region.Type == Region.RegionType.NARROW ? "|" : region.Type == Region.RegionType.ROCKY ? "." : region.Type == Region.RegionType.WET ? "=" : "?");
                    }
                    System.Diagnostics.Trace.Write(Environment.NewLine);
                }
            }
        }

        public enum Tool { TOURCH, CLIMBING_GEAR, NEITHER}

        public class Region
        {
            public enum RegionType { NULL, ROCKY, NARROW, WET}
            public RegionType Type { set; get; }
            public int ErosionLevel { set; get; }
            public int GeologicalIndex { set; get; }
        }
        class Dijkstras
        {

            public class AncestorConnection
            {
                public Node Ancestor { set; get; }
                public Tool CurrentToolFromThisAncestor { set; get; }
            }

            public class Node
            {
                public int X { set; get; }
                public int Y { set; get; }
                public int Distance { set; get; }
                public Stack<Tool> PossibleTool { set; get; }
                public bool Visited { set; get; }
                public Region.RegionType Type { set; get; }
                public bool IsEndNode { set; get; }
                public Node Parent { set; get; }

            }

            public static int GetShortestPath(Region[,] regions, Coordinate start, Coordinate end)
            {
                var nodes = new Node[regions.GetLength(0), regions.GetLength(1)];

                for (int x = 0; x < regions.GetLength(0); x++)
                {
                    for (int y = 0; y < regions.GetLength(1); y++)
                    {
                        nodes[x, y] = new Node
                        {
                            X = x,
                            Y = y,
                            Distance = int.MaxValue,
                            Visited = false,
                            Type = regions[x, y].Type,
                            IsEndNode = false,
                            PossibleTool = new Stack<Tool>()
                        };
                    }
                }

                var startNode = nodes[start.X, start.Y];
                startNode.Distance = 0;
                startNode.PossibleTool.Push(Tool.TOURCH);

                nodes[end.X, end.Y].IsEndNode = true;
                
                while (true)
                {
                    #region Find next to check
                    int minDistance = int.MaxValue;
                    Node currentNode = null;
                    for (int x = 0; x < regions.GetLength(0); x++)
                    {
                        for (int y = 0; y < regions.GetLength(1); y++)
                        {
                            var node = nodes[x, y];
                            if (!node.Visited && node.Distance < minDistance)
                            {
                                minDistance = node.Distance;
                                currentNode = node;
                            }
                        }
                    }
                    #endregion

                    if (currentNode == null || (currentNode.X == end.X && currentNode.Y == end.Y))
                    {
                        // There are no nodes left to search
                        break;
                    }

                    var nearbyNodes = GetNearbyNodes(nodes, currentNode.X, currentNode.Y);
                    foreach (var nearbynode in nearbyNodes)
                    {
                        foreach (var tool in currentNode.PossibleTool)
                        {
                            // Currentnode could have many equal ancestors but that let us end up with different tools on this node
                            var traversalCost = CalculateTraversalCost(currentNode.Type, tool, nearbynode);

                            if (currentNode.Distance + traversalCost.Cost < nearbynode.Distance) // HMMM SHOULD BE EQUAL HERE AS WELL, is the problem not so advanced that this is taken into consideration?!??!?!?
                            {
                                nearbynode.Distance = currentNode.Distance + traversalCost.Cost;
                                nearbynode.Parent = currentNode;
                                if (!nearbynode.PossibleTool.Contains(traversalCost.ToolAfterTraversal))
                                {
                                    nearbynode.PossibleTool.Push(traversalCost.ToolAfterTraversal);
                                }
                            }
                        }
                    }
                  
                    currentNode.Visited = true;
                }

                var endnode = nodes[end.X, end.Y];

                return endnode.PossibleTool.Contains(Tool.TOURCH) ? endnode.Distance : endnode.Distance + 7;
            }

            class TraversalCost
            {
                public int Cost { set; get; }
                public Tool ToolAfterTraversal { set; get; }
            }

            private static TraversalCost CalculateTraversalCost(Region.RegionType fromType, Tool fromTool, Node to)
            {
                if (fromTool == Tool.TOURCH)
                {
                    if (to.Type == Region.RegionType.ROCKY || to.Type == Region.RegionType.NARROW) {
                        return new TraversalCost { Cost = 1, ToolAfterTraversal = Tool.TOURCH };
                    } else
                    {
                        // to.Type == WET, tools that can be used here are climbing gear or nether tool, what is allowed in the current region will decide
                        return new TraversalCost { Cost = 8, ToolAfterTraversal = fromType == Region.RegionType.ROCKY ? Tool.CLIMBING_GEAR : Tool.NEITHER };
                    }
                } else if (fromTool == Tool.CLIMBING_GEAR)
                {
                    if (to.Type == Region.RegionType.ROCKY || to.Type == Region.RegionType.WET)
                    {
                        return new TraversalCost { Cost = 1, ToolAfterTraversal = Tool.CLIMBING_GEAR };
                    }
                    else
                    {
                        // to.Type == NARROW, tools that can be used here are climbing gear or nether tool, what is allowed in the current region will decide
                        return new TraversalCost { Cost = 8, ToolAfterTraversal = fromType == Region.RegionType.ROCKY ? Tool.TOURCH : Tool.NEITHER };
                    }
                } else
                {
                    // from.CurrentTool == Tool.NEITHER
                    if (to.Type == Region.RegionType.WET || to.Type == Region.RegionType.NARROW)
                    {
                        return new TraversalCost { Cost = 1, ToolAfterTraversal = Tool.NEITHER };
                    }
                    else
                    {
                        // to.Type == ROCKY, tools that can be used here are climbing gear or nether tool, what is allowed in the current region will decide
                        return new TraversalCost { Cost = 8, ToolAfterTraversal = fromType == Region.RegionType.WET ? Tool.CLIMBING_GEAR : Tool.TOURCH };
                    }
                }
            }

            private static List<Node> GetNearbyNodes(Node[,] nodes, int x, int y)
            {
                var nearby = new List<Node>();

                Action<Node> addIfNotNull = (Node node) =>
                {
                    if (node != null && !node.Visited)
                    {
                        nearby.Add(node);
                    }
                };

                addIfNotNull(x > 0 ? nodes[x - 1, y] : null);
                addIfNotNull(x < nodes.GetLength(0) - 1 ? nodes[x + 1, y] : null);
                addIfNotNull(y > 0 ? nodes[x, y - 1] : null);
                addIfNotNull(y < nodes.GetLength(1) - 1 ? nodes[x, y + 1] : null);

                return nearby;
            }
        }
    }
}
