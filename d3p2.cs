using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D3P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_3_t_1.txt").ToList();
            Assert.AreEqual(3, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_3.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            private class Claim
            {
                public int Id { set; get; }
                public int StartX { set; get; }
                public int StartY { set; get; }
                public int LengthX { set; get; }
                public int LengthY { set; get; }
                public bool HasBeenOverlapped { set; get; }

            }

            private class FabricInch
            {
                public int x { set; get; }
                public int y { set; get; }
                public List<Claim> claims = new List<Claim>();
            }

            public int RunChallenge(List<string> input)
            {
                FabricInch[,] inches = new FabricInch[1000,1000];

                for (int x = 0; x < 1000; x++)
                {
                    for (int y = 0; y < 1000; y++)
                    {
                        inches[x, y] = new FabricInch { x = x, y = y };
                    }
                }

                var claims = input.Select(raw =>
                {
                    var split = raw.Split(new string[] { "#", "@", ",", ":", "x" }, StringSplitOptions.None).Skip(1).Select(z => int.Parse(z.Trim())).ToArray();
                    return new Claim {
                        Id = split[0],
                        StartX = split[1],
                        StartY = split[2],
                        LengthX = split[3],
                        LengthY = split[4],
                        HasBeenOverlapped = false
                    };
                }).ToList();

                claims.ForEach(claim =>
                {
                    for (int x = claim.StartX; x < claim.StartX + claim.LengthX; x++)
                    {
                        for (int y = claim.StartY; y < claim.StartY + claim.LengthY; y++)
                        {
                            var claimsHere = inches[x, y].claims;
                            claimsHere.Add(claim);
                            if (claimsHere.Count() > 1)
                            {
                                claimsHere.ForEach(c => c.HasBeenOverlapped = true);
                            }
                        }
                    }

                });

                return claims.First(claim => !claim.HasBeenOverlapped).Id;
            }
        }
    }
}
