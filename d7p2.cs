using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D7P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_7_t_1.txt").ToList();
            Assert.AreEqual(15, new Program().RunChallenge(input, 2, 0));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_7.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input, 5, 60));
        }

        public class Program
        {

            private class Step
            {
                public string Name { set; get; }
                public Requirement[] Requirements { set; get; }
                public int WorkLeft { set; get; }
                public bool Started { set; get; }
            }

            private class Requirement
            {
                public string MustBeFinished { set; get; }
                public string BeforeCanBegin { set; get; }
            }

            public int RunChallenge(List<string> input, int workers, int extratime)
            {

                var requirements = input.Select(raw => new Requirement { MustBeFinished = raw.Substring(5, 1), BeforeCanBegin = raw.Substring(36, 1) }).ToList();

                var stepNames = requirements.Select(r => r.BeforeCanBegin).Union(requirements.Select(r => r.MustBeFinished)).Distinct();

                var steps = stepNames.Select(name => new Step {
                    Name = name,
                    Requirements = requirements.Where(r => r.BeforeCanBegin == name).ToArray(),
                    WorkLeft = extratime + StringConstants.ALPHABET.IndexOf(name) + 1
                }).ToList();

                var completionOrder = String.Empty;
                var time = 0;

                do
                {
                    var nextSteps = steps
                        .Where(step =>
                            !requirements.Any(r => r.BeforeCanBegin == step.Name) || // Inital steps
                            step.Requirements.All(r => completionOrder.Contains(r.MustBeFinished))) // Unlocked steps
                        .OrderByDescending(z => z.Started)
                        .ThenBy(z => z.Name)
                        .ToList();

                    nextSteps.Take(workers).ToList().ForEach(step => {
                        step.WorkLeft--;
                        step.Started = true;
                        }
                    );

                    nextSteps.Where(step => step.WorkLeft == 0).ToList().ForEach(step =>
                    {
                        completionOrder += step.Name;
                        steps.Remove(step);
                    });

                    time++;
                } while (steps.Any());

                return time;
            }
        }
    }
}
