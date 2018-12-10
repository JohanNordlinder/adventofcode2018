using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D10P1_AND_2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_10_t_1.txt").ToList();
            Assert.AreEqual(3, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_10.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var particles = input.Select(raw =>
                {
                    var parsed = raw.Split(new string[] { "<", ",", ">"}, StringSplitOptions.None).ToArray();

                    return new Particle
                    {
                        Coordinate = new Coordinate {
                            X = int.Parse(parsed[1].Trim()),
                            Y = int.Parse(parsed[2].Trim()),
                        },
                        Velocity = new Velocity
                        {
                            X = int.Parse(parsed[4].Trim()),
                            Y = int.Parse(parsed[5].Trim()),
                        }
                    };
                }).ToList();

                var text = false;
                var seconds = 0;

                while(!text)
                {
                    seconds++;
                    particles.ForEach(p =>
                    {
                        p.Coordinate.X = p.Coordinate.X + p.Velocity.X;
                        p.Coordinate.Y = p.Coordinate.Y + p.Velocity.Y;
                    });

                    text = particles.All(particle =>
                     {
                         return particles.Any(otherParticle =>
                             particle != otherParticle &&
                             Math.Abs(particle.Coordinate.X - otherParticle.Coordinate.X) + Math.Abs(particle.Coordinate.Y - otherParticle.Coordinate.Y) <= 2
                         );
                     });
                }

                Print(particles);

                return seconds;
            }

            private void Print(List<Particle> particles)
            {
                var extents = new
                {
                    MaxX = particles.Max(p => p.Coordinate.X),
                    MaxY = particles.Max(p => p.Coordinate.Y),
                    MinX = particles.Min(p => p.Coordinate.X),
                    MinY = particles.Min(p => p.Coordinate.Y),
                };

                for (int y = extents.MinY; y <= extents.MaxY; y++)
                {
                    for (int x = extents.MinX; x <= extents.MaxX; x++)
                    {
                        var coordinate = particles.FirstOrDefault(p => p.Coordinate.X == x && p.Coordinate.Y == y);
                        System.Diagnostics.Trace.Write(coordinate != null ? "#" : ".");
                    }
                    System.Diagnostics.Trace.Write(Environment.NewLine);
                }
            }
        }
    }
}
