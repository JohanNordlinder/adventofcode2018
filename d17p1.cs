using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D17P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_17_t_1.txt").ToList();
            Assert.AreEqual(57, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_17.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            private class Scan
            {
                public int FromX { set; get; }
                public int ToX { set; get; }
                public int FromY { set; get; }
                public int ToY { set; get; }
            }

            public class Square
            {
                public bool IsWell { set; get; }
                public bool DoesCount { set; get; }
                public bool IsClay { set; get; }
                public bool TouchedByWater { set; get; }
                public bool HasWater { set; get; }
                public Coordinate Coordinate { set; get; }
                public FallingWater CreatedBy { set; get; }
            }

            public class FallingWater
            {
                public Coordinate Coordinate { set; get; }
            }

            public class WaterReachedEnd : Exception { }

            public int NumberTouchedByWater = 0;

            public int RunChallenge(List<string> input)
            {
                var scans = new List<Scan>();

                foreach (var raw in input)
                {
                    var splitXY = raw.Split(new string[] { ","}, StringSplitOptions.None);

                    String xString = splitXY[0][0] == 'x' ? splitXY[0] : splitXY[1];
                    String yString = splitXY[0][0] == 'y' ? splitXY[0] : splitXY[1];

                    Func<String, Tuple<int, int>> parseValue = (String valueString) => {
                        if (valueString.IndexOf("..") == -1)
                        {
                            var value = int.Parse(valueString.Substring(2));
                            return Tuple.Create(value, value);
                        }
                        var splitValues = valueString.Split(new string[] { "x=", "y=", ".." }, StringSplitOptions.None);
                        var fromValue = int.Parse(splitValues[1]);
                        var toValue = int.Parse(splitValues[2]);
                        return Tuple.Create(fromValue, toValue);

                    };

                    var xValues = parseValue(xString);
                    var yValues = parseValue(yString);

                    scans.Add(new Scan
                    {
                        FromX = xValues.Item1,
                        ToX = xValues.Item2,
                        FromY = yValues.Item1,
                        ToY = yValues.Item2,
                    });
                }

                var extents = new
                {
                    MaxX = scans.Max(z => z.ToX),
                    MinX= scans.Min(z => z.FromX),
                    MaxY = scans.Max(z => z.ToY),
                    MinY = scans.Min(z => z.FromY)
                };

                var grid = new Dictionary<Tuple<int,int>, Square>();

                for (int y = 0; y <= extents.MaxY; y++)
                {
                    for (int x = extents.MinX - 1; x <= extents.MaxX + 1; x++)
                    {
                        grid.Add(Tuple.Create(x, y), new Square {
                            IsWell = (x == 500 && y == 0),
                            DoesCount = y >= extents.MinY,
                            TouchedByWater = false,
                            HasWater = false,
                            Coordinate = new Coordinate { X = x, Y = y }
                        });
                    }
                }

                foreach (var scan in scans)
                {
                    for (int y = scan.FromY; y <= scan.ToY; y++)
                    {
                        for (int x = scan.FromX; x <= scan.ToX; x++)
                        {
                            grid[Tuple.Create(x, y)].IsClay = true;
                        }
                    }
                }

                Action printAll = () =>
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", false))
                    {
                        for (int y = 0; y <= extents.MaxY; y++)
                        {
                            for (int x = extents.MinX - 1; x <= extents.MaxX + 1; x++)
                            {
                                var square = grid[Tuple.Create(x, y)];
                                var output = square.IsClay ? "#" : square.IsWell ? "+" : square.HasWater ? "~" : square.TouchedByWater ? "|" : ".";
                                file.Write(output);
                            }
                            file.Write(Environment.NewLine);
                        }
                    }
                };

                var fallingWaters = new List<FallingWater>();
                fallingWaters.Add(new FallingWater { Coordinate = new Coordinate { Y = 0, X = 500 } });

                while (fallingWaters.Any())
                {
                    //printAll();
                    foreach (var water in fallingWaters.ToList())
                    {
                        var nextCoordinate = new Coordinate{ Y = water.Coordinate.Y + 1, X = water.Coordinate.X };

                        if (nextCoordinate.Y > extents.MaxY)
                        {
                            fallingWaters.Remove(water);
                            continue;
                        }

                        var nextSquare = grid[Tuple.Create(nextCoordinate.X, nextCoordinate.Y)];

                        if (nextSquare.HasWater && nextSquare.CreatedBy != water)
                        {
                            fallingWaters.Remove(water);
                            continue;
                        }

                        // Continue falling down?
                        if (!nextSquare.IsClay && !nextSquare.HasWater)
                        {
                            MarkAsTouchedByWater(nextSquare);
                            water.Coordinate = nextCoordinate;
                        }
                        else
                        {
                            // Can fall to the side?
                            var fallDownLeft = FindFallDownSquare(Direction.LEFT, water.Coordinate, grid);
                            var fallDownRight = FindFallDownSquare(Direction.RIGHT, water.Coordinate, grid);

                            if (fallDownRight != null && fallDownLeft != null)
                            {
                                water.Coordinate = fallDownRight.Coordinate;
                                fallingWaters.Add(new FallingWater { Coordinate = fallDownLeft.Coordinate });
                            } else if (fallDownRight != null)
                            {
                                water.Coordinate = fallDownRight.Coordinate;
                            } else if (fallDownLeft != null)
                            {
                                water.Coordinate = fallDownLeft.Coordinate;
                            }

                            if (fallDownRight == null && fallDownLeft == null) {
                                // Fill this space
                                FillWithWater(Direction.LEFT, water, grid);
                                FillWithWater(Direction.RIGHT, water, grid);

                                water.Coordinate.Y--;
                                MarkAsTouchedByWater(grid[Tuple.Create(water.Coordinate.X, water.Coordinate.Y)]);
                            }
                        }
                    }
                }
                printAll();
                return NumberTouchedByWater;
            }

            private void MarkAsTouchedByWater(Square square)
            {
                if (!square.TouchedByWater)
                {
                    square.TouchedByWater = true;
                    if (square.DoesCount)
                    {
                        NumberTouchedByWater++;
                    }
                }
            }

            private Square FindFallDownSquare(Direction direction, Coordinate startCoordinate, Dictionary<Tuple<int, int>, Square> grid)
            {
                var x = startCoordinate.X;
                Square nextSquare = null;
                Square nextSquareBelow = null;
                while(true)
                {
                    x = direction == Direction.LEFT ? x - 1 : x + 1;
                    nextSquare = grid[Tuple.Create(x, startCoordinate.Y)];

                    if (nextSquare.IsClay)
                    {
                        return null;
                    }

                    MarkAsTouchedByWater(nextSquare);

                    nextSquareBelow = grid[Tuple.Create(x, startCoordinate.Y + 1)];

                    if (!nextSquareBelow.IsClay && !nextSquareBelow.HasWater)
                    {
                        MarkAsTouchedByWater(nextSquareBelow);
                        return nextSquareBelow;
                    }
                };
            }
            
            private void FillWithWater(Direction direction, FallingWater water, Dictionary<Tuple<int, int>, Square> grid)
            {
                var x = water.Coordinate.X;
                Square thisSquare = null;
                while(true)
                {
                    thisSquare = grid[Tuple.Create(x, water.Coordinate.Y)];

                    if (!thisSquare.IsClay)
                    {
                        thisSquare.HasWater = true;
                        thisSquare.CreatedBy = water;
                        MarkAsTouchedByWater(thisSquare);
                        x = direction == Direction.LEFT ? x - 1 : x + 1;
                    }
                    else
                    {
                        break;
                    }
                    
                };
            }
        }
    }
}
