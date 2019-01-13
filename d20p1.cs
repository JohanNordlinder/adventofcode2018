using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D20P1
    {

        [TestMethod]
        public void TestRun1()
        {
            var input = System.IO.File.ReadAllLines("d_20_t_1.txt").ToList();
            Assert.AreEqual(3, new Program().RunChallenge(input[0]));
        }

        [TestMethod]
        public void TestRun2()
        {
            var input = System.IO.File.ReadAllLines("d_20_t_2.txt").ToList();
            Assert.AreEqual(10, new Program().RunChallenge(input[0]));
        }

        [TestMethod]
        public void TestRun3()
        {
            var input = System.IO.File.ReadAllLines("d_20_t_3.txt").ToList();
            Assert.AreEqual(18, new Program().RunChallenge(input[0]));
        }

        [TestMethod]
        public void TestRun4()
        {
            var input = System.IO.File.ReadAllLines("d_20_t_4.txt").ToList();
            Assert.AreEqual(23, new Program().RunChallenge(input[0]));
        }

        [TestMethod]
        public void TestRun5()
        {
            var input = System.IO.File.ReadAllLines("d_20_t_5.txt").ToList();
            Assert.AreEqual(31, new Program().RunChallenge(input[0]));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_20.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input[0]));
        }

        public class Program
        {

            public class Square
            {
                public enum SquareType { UNKNNOWN, WALL, ROOM, DOOR_VERTICAL, DOOR_HORIZONTAL }
                public SquareType Type { set; get; }
                public Coordinate Coordinate { set; get; }
                public List<Square> ConnectedTo = new List<Square>();
                public int DoorsToGetHere = int.MaxValue;
        }

            Dictionary<Coordinate, Square> grid = new Dictionary<Coordinate, Square>();

            public int RunChallenge(string input)
            { 
                var origin = new Coordinate { Y = 0, X = 0 };
                Parse(input, 1, origin, 0);

                foreach (var square in grid.Values.Where(s => s.Type == Square.SquareType.UNKNNOWN))
                {
                    square.Type = Square.SquareType.WALL;
                }

                //PrintAll();
                var max = grid.Values.Where(v => v.DoorsToGetHere != int.MaxValue).Max(v => v.DoorsToGetHere);

                return max;
            }

            private void PrintAll()
            {
                for (int y = grid.Keys.Max(c => c.Y); y >= grid.Keys.Min(c => c.Y); y--)
                {
                    for (int x = grid.Keys.Min(c => c.X); x <= grid.Keys.Max(c => c.X); x++)
                    {
                        Square square;
                        ;
                        if (grid.TryGetValue(new Coordinate { X = x, Y = y }, out square))
                        {
                            System.Diagnostics.Trace.Write(square.Type == Square.SquareType.UNKNNOWN ? "?" : square.Type == Square.SquareType.WALL ? "#" : square.Type == Square.SquareType.ROOM ? "." : square.Type == Square.SquareType.DOOR_HORIZONTAL ? "-" : "|");
                        }
                        else
                        {
                            System.Diagnostics.Trace.Write(" ");

                        }
                    }
                    System.Diagnostics.Trace.Write(Environment.NewLine);
                }
            }

            private int Parse(string input, int startIndex, Coordinate startCoordinate, int doorsPassed)
            {
                var doorsPassedInThisThread = doorsPassed;
                var currentLocation = new Coordinate { X = startCoordinate.X, Y = startCoordinate.Y };
                Coordinate newLocation;
                for (int i = startIndex; i < input.Length - 1; i++)
                {
                    switch (input[i])
                    {
                        case '(':
                            i = Parse(input, i + 1, new Coordinate { X = currentLocation.X, Y = currentLocation.Y }, doorsPassedInThisThread);
                            continue;
                        case ')':
                            return i;
                        case '|':
                            currentLocation = startCoordinate;
                            doorsPassedInThisThread = doorsPassed;
                            continue;
                        case 'N':
                            doorsPassedInThisThread++;
                            newLocation = new Coordinate { X = currentLocation.X, Y = currentLocation.Y + 2 };
                            HandleStep(currentLocation, newLocation, Compass.NORTH, doorsPassedInThisThread);
                            currentLocation = newLocation;
                            break;
                        case 'E':
                            doorsPassedInThisThread++;
                            newLocation = new Coordinate { X = currentLocation.X + 2, Y = currentLocation.Y };
                            HandleStep(currentLocation, newLocation, Compass.EAST, doorsPassedInThisThread);
                            currentLocation = newLocation;
                            break;
                        case 'S':
                            doorsPassedInThisThread++;
                            newLocation = new Coordinate { X = currentLocation.X, Y = currentLocation.Y - 2 };
                            HandleStep(currentLocation, newLocation, Compass.SOUTH, doorsPassedInThisThread);
                            currentLocation = newLocation;
                            break;
                        case 'W':
                            doorsPassedInThisThread++;
                            newLocation = new Coordinate { X = currentLocation.X - 2, Y = currentLocation.Y };
                            HandleStep(currentLocation, newLocation, Compass.WEST, doorsPassedInThisThread);
                            currentLocation = newLocation;
                            break;
                    }
                }

                return input.Length - 1;
            }

            private void HandleStep(Coordinate previousLocation, Coordinate newLocation, Compass compass, int doorsPassed)
            {
                foreach (var location in new Coordinate[] { previousLocation, newLocation })
                {
                    CreateSquareIfNeeded(location.X, location.Y, Square.SquareType.ROOM, doorsPassed);
                    CreateSquareIfNeeded(location.X - 1, location.Y - 1, Square.SquareType.WALL);
                    CreateSquareIfNeeded(location.X + 1, location.Y + 1, Square.SquareType.WALL);
                    CreateSquareIfNeeded(location.X - 1, location.Y + 1, Square.SquareType.WALL);
                    CreateSquareIfNeeded(location.X + 1, location.Y - 1, Square.SquareType.WALL);
                    CreateSquareIfNeeded(location.X - 1, location.Y, Square.SquareType.UNKNNOWN);
                    CreateSquareIfNeeded(location.X + 1, location.Y, Square.SquareType.UNKNNOWN);
                    CreateSquareIfNeeded(location.X, location.Y + 1, Square.SquareType.UNKNNOWN);
                    CreateSquareIfNeeded(location.X, location.Y - 1, Square.SquareType.UNKNNOWN);
                }

                grid[previousLocation].ConnectedTo.Add(grid[newLocation]);
                grid[newLocation].ConnectedTo.Add(grid[previousLocation]);

                switch (compass)
                {
                    case Compass.WEST:
                        CreateSquareIfNeeded(previousLocation.X - 1, previousLocation.Y, Square.SquareType.DOOR_VERTICAL);
                        break;
                    case Compass.SOUTH:
                        CreateSquareIfNeeded(previousLocation.X, previousLocation.Y - 1, Square.SquareType.DOOR_HORIZONTAL);
                        break;
                    case Compass.EAST:
                        CreateSquareIfNeeded(previousLocation.X + 1, previousLocation.Y, Square.SquareType.DOOR_VERTICAL);
                        break;
                    case Compass.NORTH:
                        CreateSquareIfNeeded(previousLocation.X, previousLocation.Y + 1, Square.SquareType.DOOR_HORIZONTAL);
                        break;
                }
            }

            private void CreateSquareIfNeeded(int x, int y, Square.SquareType expectedType, int? doorsPassed = null)
            {
                var coord = new Coordinate { X = x, Y = y };
                Square square;
                if (!grid.TryGetValue(coord, out square))
                {
                    square = new Square { Coordinate = coord, Type = expectedType };
                    grid.Add(coord, square);
                }
                else if (square.Type == Square.SquareType.UNKNNOWN)
                {
                    square.Type = expectedType;
                }

                if (doorsPassed != null && expectedType == Square.SquareType.ROOM)
                {
                    if (square.DoorsToGetHere > doorsPassed)
                    {
                        square.DoorsToGetHere = (int) doorsPassed;
                    }
                }
            }
        }
    }
}
