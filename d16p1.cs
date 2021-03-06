﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D16P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_16_t_1.txt").ToList();
            Assert.AreEqual(1, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_16.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {

            public class OpCode
            {
                public int Code { set; get; }
                public String Name { set; get; }
                public Func<int[], int, int, int, int[]> Operation { set; get;}
            }

            public class Sample
            {
                public int[] Before { set; get; }
                public int[] After { set; get; }
                public int OpCode { set; get; }
                public int Source1 { set; get; }
                public int Source2 { set; get; }
                public int TargetRegister { set; get; }
            }

            public int RunChallenge(List<string> input)
            {
                var opcodes = CreateOpCodes();
                var samples = new List<Sample>();
                for (int i = 0; i < 3124; i = i + 4)
                {
                    var beforeRaw = input[i].Replace(" ", "")
                        .Split(new string[] { "[", ",", "]" }, StringSplitOptions.None)
                        .Skip(1)
                        .Reverse()
                        .Skip(1)
                        .Reverse()
                        .Select(c => int.Parse(c.ToString())).ToArray();
                    var Opcode = input[i + 1].Split(new string[] { " "}, StringSplitOptions.None).Select(c => int.Parse(c.ToString())).ToArray();
                    var afterRaw = input[i + 2].Replace(" ", "")
                        .Split(new string[] { "[", ",", "]" }, StringSplitOptions.None)
                        .Skip(1)
                        .Reverse()
                        .Skip(1)
                        .Reverse()
                        .Select(c => int.Parse(c.ToString())).ToArray();

                    samples.Add(new Sample {
                        Before = beforeRaw,
                        After = afterRaw,
                        OpCode = Opcode[0],
                        Source1 = Opcode[1],
                        Source2 = Opcode[2],
                        TargetRegister = Opcode[3],
                    });
                }

                int result = 0;

                foreach (var sample in samples)
                {
                    var testRegister = sample.Before;
                    var matching = opcodes.Where(opcode =>
                        opcode.Operation(testRegister.Select(z => z).ToArray(), sample.Source1, sample.Source2, sample.TargetRegister).SequenceEqual(sample.After));
                    if (matching.Count() >= 3)
                    {
                        result++;
                    }
                }

                return result;
            }

            private List<OpCode> CreateOpCodes()
            {
                var opcodes = new List<OpCode>
                {
                    new OpCode
                    {
                        Name = "addr",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] + registers[source2];
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "addi",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] + source2;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "mulr",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] * registers[source2];
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "muli",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] * source2;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "banr",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] & registers[source2];
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "bani",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] & source2;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "borr",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] | registers[source2];
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "bori",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] | source2;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "setr",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1];
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "seti",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = source1;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "gtir",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = source1 > registers[source2] ? 1 : 0;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "gtri",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] > source2 ? 1 : 0;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "gtrr",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] > registers[source2] ? 1 : 0;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "eqir",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = source1 == registers[source2] ? 1 : 0;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "eqri",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] == source2 ? 1 : 0;
                            return registers;
                        }
                    },

                    new OpCode
                    {
                        Name = "eqrr",
                        Operation = (int[] registers, int source1, int source2, int targetRegister) =>
                        {
                            registers[targetRegister] = registers[source1] == registers[source2] ? 1 : 0;
                            return registers;
                        }
                    }
                };

                return opcodes;
            }
        }
    }
}
