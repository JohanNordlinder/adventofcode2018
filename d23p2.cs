using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Z3;

namespace AdventOfCode2018
{
    [TestClass]
    public class D23P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_23_t_2.txt").ToList();
            new Program().RunChallenge(input);
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_23.txt").ToList();
            new Program().RunChallenge(input);
        }

        [TestMethod]
        public void TestZ3()
        {
            using (Context ctx = new Context())
            {
                Expr x = ctx.MkConst("x", ctx.MkIntSort());
                Expr y = ctx.MkConst("y", ctx.MkIntSort());
                Expr z = ctx.MkConst("z", ctx.MkIntSort());
                Expr zero = ctx.MkNumeral(0, ctx.MkIntSort());
                Expr one = ctx.MkNumeral(1, ctx.MkIntSort());
                Expr Two = ctx.MkNumeral(2, ctx.MkIntSort());
                Expr Three = ctx.MkNumeral(3, ctx.MkIntSort());
                Expr Four = ctx.MkNumeral(4, ctx.MkIntSort());

                Expr Minus2 = ctx.MkNumeral(-2, ctx.MkIntSort());


                var solver = ctx.MkSolver();
                solver.Add(ctx.MkEq(
                    ctx.MkAdd(ctx.MkMul((ArithExpr)Three, (ArithExpr)x), ctx.MkMul((ArithExpr)Two, (ArithExpr)y), ctx.MkUnaryMinus((ArithExpr)z)),
                    (ArithExpr)one)); // 3*x + 2*y - z == 1
                solver.Add(ctx.MkEq(
                    ctx.MkAdd(ctx.MkMul((ArithExpr)Two, (ArithExpr)x), ctx.MkUnaryMinus(ctx.MkMul((ArithExpr)Two, (ArithExpr)y)), ctx.MkMul((ArithExpr)Four, (ArithExpr)z)),
                    ctx.MkUnaryMinus((ArithExpr)Two))); // 3*x + 2*y - z == 1
                solver.Add(ctx.MkEq(
                    ctx.MkAdd(ctx.MkUnaryMinus((ArithExpr)x), ctx.MkDiv((ArithExpr)y, (ArithExpr)Two), ctx.MkUnaryMinus((ArithExpr)z))
                    , (ArithExpr)zero)); // 3*x + 2*y - z == 1

                var status = solver.Check();

                System.Diagnostics.Trace.WriteLine(solver.Model);

            }

        }

        [TestMethod]
        public void TestZ3Optimize()
        {
            using (Context ctx = new Context())
            {
                var x = (ArithExpr) ctx.MkConst("x", ctx.MkIntSort());

                var opt = ctx.MkOptimize();
                var handle = opt.MkMinimize((ArithExpr) x);

                opt.Add(ctx.MkGt(x, (ArithExpr)ctx.MkNumeral(2, ctx.MkIntSort())));
                opt.Add(ctx.MkLt(x, (ArithExpr)ctx.MkNumeral(6, ctx.MkIntSort())));

                var status = opt.Check();

                System.Diagnostics.Trace.WriteLine(opt.Model);

            }
        }

        public class Program
        {

            public class Nanobot
            {
                public int Number { set; get; }
                public Coordinate Coordinate { set; get; }
                public int Radius { set; get; }
                public HashSet<Nanobot> InGroupWith = new HashSet<Nanobot>();
            }

            public List<Nanobot> Nanobots;
            public Dictionary<Tuple<int,int>, bool> InRanges = new Dictionary<Tuple<int, int>, bool>();

            public void RunChallenge(List<string> input)
            {
                var count = 0;
                var Nanobots = new List<Nanobot>();

                foreach (var raw in input)
                {
                    var parsed = raw.Split(new string[] { "pos=<", ",", ">", "r=" }, StringSplitOptions.None).ToArray();
                    var bot = new Nanobot
                    {
                        Number = count,
                        Coordinate = new Coordinate
                        {
                            X = int.Parse(parsed[1]),
                            Y = int.Parse(parsed[2]),
                            Z = int.Parse(parsed[3]),
                        },
                        Radius = int.Parse(parsed[6])
                    };
                    Nanobots.Add(bot);
                    count++;
                }

                foreach (var bot in Nanobots)
                {
                    foreach (var otherbot in Nanobots)
                    {
                        if (bot.Number != otherbot.Number && InRange(bot, otherbot))
                        {
                            InRanges.Add(Tuple.Create(bot.Number, otherbot.Number), true);
                        }
                        else
                        {
                            InRanges.Add(Tuple.Create(bot.Number, otherbot.Number), false);
                        }
                    }
                }

                var finalGroup = Nanobots.Select(n => n).ToList();

                Nanobot toRemove = null;
                int biggestNumberOfMissmatch = 0;
                do
                {
                    toRemove = null;

                    foreach (var nano in finalGroup)
                    {
                        var thisBotMismatch = 0;

                        foreach (var otherMano in finalGroup)
                        {
                            if (nano.Number != otherMano.Number && !InRanges[Tuple.Create(nano.Number, otherMano.Number)])
                            {
                                thisBotMismatch++;
                            }
                        }

                        if (thisBotMismatch > biggestNumberOfMissmatch)
                        {
                            toRemove = nano;
                            biggestNumberOfMissmatch = thisBotMismatch;
                        }
                    }

                    finalGroup.Remove(toRemove);
                    biggestNumberOfMissmatch = 0;

                } while (toRemove != null);

                using (Context ctx = new Context())
                {
                    var x = (ArithExpr) ctx.MkConst("x", ctx.MkIntSort());
                    var y = (ArithExpr) ctx.MkConst("y", ctx.MkIntSort());
                    var z = (ArithExpr) ctx.MkConst("z", ctx.MkIntSort());

                    var Zero = (ArithExpr)ctx.MkNumeral(0, ctx.MkIntSort());

                    Func<int, ArithExpr> getNumber = (int number) => (ArithExpr)ctx.MkNumeral(number, ctx.MkIntSort());

                    var optimizer = ctx.MkOptimize();

                    var absX = (ArithExpr) ctx.MkITE(ctx.MkLt(x, Zero), -x, x);
                    var absY = (ArithExpr) ctx.MkITE(ctx.MkLt(y, Zero), -y, y);
                    var absZ = (ArithExpr) ctx.MkITE(ctx.MkLt(z, Zero), -z, z);

                    optimizer.MkMinimize(ctx.MkAdd(absX, absY, absZ));

                    foreach (var nano in finalGroup)
                    {
                        var funcX = (ArithExpr) ctx.MkITE(ctx.MkLt(x - getNumber(nano.Coordinate.X), Zero), -(x - getNumber(nano.Coordinate.X)), x - getNumber(nano.Coordinate.X));
                        var funcY = (ArithExpr) ctx.MkITE(ctx.MkLt(y - getNumber(nano.Coordinate.Y), Zero), -(y - getNumber(nano.Coordinate.Y)), y - getNumber(nano.Coordinate.Y));
                        var funcZ = (ArithExpr) ctx.MkITE(ctx.MkLt(z - getNumber(nano.Coordinate.Z), Zero), -(z - getNumber(nano.Coordinate.Z)), z - getNumber(nano.Coordinate.Z));

                        optimizer.Add(ctx.MkOr(
                            ctx.MkEq(
                                ctx.MkAdd(funcX, funcY, funcZ),
                                getNumber(nano.Radius)
                            ),
                            ctx.MkLt(
                                ctx.MkAdd(funcX, funcY, funcZ),
                                getNumber(nano.Radius)
                            )
                        ));
                    }

                    var status = optimizer.Check();

                    System.Diagnostics.Trace.WriteLine(optimizer.Model);
                }
            }

            private static bool InRange(Nanobot bot, Nanobot otherBot)
            {
                var xDist = Math.Abs(bot.Coordinate.X - otherBot.Coordinate.X);
                var yDist = Math.Abs(bot.Coordinate.Y - otherBot.Coordinate.Y);
                var zDist = Math.Abs(bot.Coordinate.Z - otherBot.Coordinate.Z);

                if (xDist + yDist + zDist <= bot.Radius + otherBot.Radius)
                {
                    return true;
                }
                return false;
            }

            private static int Distance(int x, int y, int z, Nanobot bot)
            {
                var xDist = Math.Abs(x - bot.Coordinate.X);
                var yDist = Math.Abs(y - bot.Coordinate.Y);
                var zDist = Math.Abs(z - bot.Coordinate.Z);

                return xDist + yDist + zDist - bot.Radius;
            }

            private static bool InRange(int x, int y, int z, Nanobot bot)
            {
                var xDist = Math.Abs(x - bot.Coordinate.X);
                var yDist = Math.Abs(y - bot.Coordinate.Y);
                var zDist = Math.Abs(z - bot.Coordinate.Z);

                if (xDist + yDist + zDist <= bot.Radius)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
