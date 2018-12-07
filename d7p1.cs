using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D7P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_7_t_1.txt").ToList();
            Assert.AreEqual("CABDFE", new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_7.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {

            private class Step
            {
                public string Name { set; get; }
                public Requirement[] Requirements { set; get; }
            }

            private class Requirement
            {
                public string MustBeFinished { set; get; }
                public string BeforeCanBegin { set; get; }
            }

            public string RunChallenge(List<string> input)
            {

                var requirements = input.Select(raw => new Requirement {
                    MustBeFinished = raw.Substring(5, 1),
                    BeforeCanBegin = raw.Substring(36, 1)
                }).ToList();

                var stepNames = requirements.Select(r => r.BeforeCanBegin).Union(requirements.Select(r => r.MustBeFinished)).Distinct();

                var steps = stepNames.Select(name => new Step
                {
                    Name = name,
                    Requirements = requirements.Where(r => r.BeforeCanBegin == name).ToArray()
                }).ToList();

                var completionOrder = String.Empty;

                do
                {
                    var nextStep = steps
                        .Where(step =>
                            !requirements.Any(r => r.BeforeCanBegin == step.Name) || // First step
                            step.Requirements.All(r => completionOrder.Contains(r.MustBeFinished))) // Unlocked Step
                        .OrderBy(z => z.Name)
                        .ToList();
                    completionOrder += nextStep.First().Name;
                    steps.Remove(nextStep.First());

                } while (steps.Any());

                return completionOrder;
            }
        }
    }
}
