﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D13P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_13_t_2.txt").ToArray();
            Assert.AreEqual("6,4", new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_13.txt").ToArray();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public enum TrackDirection
            {
               NOTTRACK, FORWARD, TURN_RIGHT, TURN_LEFT, INTERSECTION
            }

            public class Train
            {
                public Direction Direction { set; get; }
                public Coordinate Position { set; get; }
                public Direction TurnTime { set; get; }
                public Direction NextTurn = Direction.LEFT;
                public bool Crashed = false;
            }

            public string RunChallenge(string[] input)
            {
                var size = new { Y = input.Length, X = input[0].Length };

                var trains = new List<Train>();

                var tracks = new TrackDirection[size.X,size.Y];

                for (int y = 0; y < size.Y; y++)
                {
                    for (int x = 0; x < size.X; x++)
                    {
                        var raw = input[y][x];

                        if (raw == ' ')
                        {
                            tracks[x, y] = TrackDirection.NOTTRACK;
                            continue;
                        }; 

                        if (raw == '<' || raw == '>' || raw == '^' || raw == 'v')
                        {
                            trains.Add(new Train { Direction = ParseTrainDirection(raw), Position = new Coordinate { X = x, Y = y } });
                            tracks[x, y] = TrackDirection.FORWARD;
                        } else
                        {
                            tracks[x, y] = ParseTrackDirection(raw);
                        }
                        
                    }
                }

                int ticks = 0;

                string realResturn = "";
                do
                {
                    ticks++;

                    var trainsAtSame = new List<Train>();
                    trains.OrderBy(t => t.Position.Y).ThenBy(t => t.Position.X).ToList().ForEach(train =>
                    {
                        // Move train
                        if (train.Direction == Direction.LEFT)
                        {
                            train.Position.X--;
                        }
                        else if (train.Direction == Direction.RIGHT)
                        {
                            train.Position.X++;
                        }
                        else if (train.Direction == Direction.UP)
                        {
                            train.Position.Y--;
                        }
                        else if (train.Direction == Direction.DOWN)
                        {
                            train.Position.Y++;
                        }

                        var trackDirection = tracks[train.Position.X, train.Position.Y];

                        switch(trackDirection)
                        {
                            case TrackDirection.NOTTRACK:
                                throw new Exception("Oh fuck");
                            case TrackDirection.FORWARD:
                                // Do nothing
                                break;
                            case TrackDirection.TURN_LEFT:
                                if (train.Direction == Direction.RIGHT || train.Direction == Direction.LEFT)
                                {
                                    train.Direction = DirectionTurns.TurnRight(train.Direction);
                                } else
                                {
                                    train.Direction = DirectionTurns.TurnLeft(train.Direction);
                                }
                                break;
                            case TrackDirection.TURN_RIGHT:
                                if (train.Direction == Direction.RIGHT || train.Direction == Direction.LEFT)
                                {
                                    train.Direction = DirectionTurns.TurnLeft(train.Direction);
                                }
                                else
                                {
                                    train.Direction = DirectionTurns.TurnRight(train.Direction);
                                }
                                break;
                            case TrackDirection.INTERSECTION:
                                if (train.NextTurn != Direction.FORWARD)
                                {
                                    if (train.NextTurn == Direction.LEFT)
                                    {
                                        train.Direction = DirectionTurns.TurnLeft(train.Direction);
                                    } else
                                    {
                                        train.Direction = DirectionTurns.TurnRight(train.Direction);
                                    }
                                    
                                }
                                CycleTurnDirectionForTrain(train);
                                break;
                        }
                        var trainsAtSameNow = trains.Where(thistrain => trains.Any(otherTrain => (thistrain != otherTrain) && (thistrain.Position.X == otherTrain.Position.X) && (thistrain.Position.Y == otherTrain.Position.Y))).ToList();
                        trainsAtSame.AddRange(trainsAtSameNow);
                    });


                    if (trainsAtSame.Any(t => !t.Crashed))
                    {
                        trainsAtSame.ToList().ForEach(t => trains.Remove(t));
                    }

                } while (trains.Count > 1);

                return trains.First().Position.X + "," + trains.First().Position.Y;
            }

            private void CycleTurnDirectionForTrain(Train train)
            {
                if (train.NextTurn == Direction.LEFT) { 
                    train.NextTurn = Direction.FORWARD;
                } else if (train.NextTurn == Direction.FORWARD)
                {
                    train.NextTurn = Direction.RIGHT;
                } else if (train.NextTurn == Direction.RIGHT)
                {
                    train.NextTurn = Direction.LEFT;
                }
            }

            private Direction ParseTrainDirection(char raw)
            {
                switch (raw)
                {
                    case '<':
                        return Direction.LEFT;
                    case '>':
                        return Direction.RIGHT;
                    case '^':
                        return Direction.UP;
                    case 'v':
                        return Direction.DOWN;
                    default:
                        throw new Exception("Unable to find TrackDirection");
                };
            }

            private TrackDirection ParseTrackDirection(char raw)
            {
                switch (raw)
                {
                    case '-':
                        return TrackDirection.FORWARD;
                    case '|':
                        return TrackDirection.FORWARD;
                    case '/':
                        return TrackDirection.TURN_RIGHT;
                    case '\\':
                        return TrackDirection.TURN_LEFT;
                    case '+':
                        return TrackDirection.INTERSECTION;
                    default:
                        throw new Exception("Unable to find TrackDirection");
                };
            }
        }
    }
}
