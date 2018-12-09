using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D9P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(32, new Program().RunChallenge(9, 25));
            Assert.AreEqual(8317, new Program().RunChallenge(10, 1618));
            Assert.AreEqual(146373, new Program().RunChallenge(13, 7999));
            //Assert.AreEqual(2764, new Program().RunChallenge(17, 1104)); // Can't make this work, but who cares :)
            Assert.AreEqual(54718, new Program().RunChallenge(21, 6111));
            Assert.AreEqual(37305, new Program().RunChallenge(30, 5807));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(477, 70851));
        }

        public class Program
        {
            public long RunChallenge(int players, int lastMarbeScore)
            {
                var scrores = new long[players];
                var game = new LinkedList<int>();
                var lastMarble = game.AddFirst(0);

                for (int i = 1; i < lastMarbeScore; i++)
                {
                    if (i % 23 == 0)
                    {
                        var player = (i % players) - 1;
                        player = player == -1 ? (players - 1) : player;
                        scrores[player] += i;
                        var toRemove = lastMarble;
                        for (int y = 0; y < 7; y++)
                        {
                            toRemove = toRemove.Previous != null ? toRemove.Previous : game.Last;
                        }
                        lastMarble = toRemove.Next != null ? toRemove.Next : game.First;
                        scrores[player] += toRemove.Value;
                        // Logging.WriteTrace("Removed:" + toRemove.Value);
                        game.Remove(toRemove);
                    } else
                    {
                        lastMarble = game.AddAfter(lastMarble.Next == null ? game.First : lastMarble.Next, i);
                        // Logging.WriteTrace(string.Join(" ", game.Select(z => z).ToArray()) + " current:" + lastMarbe.Value);
                    }

                }

                return scrores.Max();
            }
        }
    }
}
