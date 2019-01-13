using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D19P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_19_t_1.txt").ToList();
            Assert.AreEqual(6, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_19.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var pointerRegister = int.Parse(input[0].Split(new string[] { " " }, StringSplitOptions.None)[1]);
                var instructionPointer = 0;
                var opcodes = D16P2.Program.CreateOpCodes();
                var registers = new int[6] { 0, 0, 0, 0, 0, 0 };

                var instructions = input.Skip(1).Select(s => s.Split(new string[] { " " }, StringSplitOptions.None)).ToArray();

                while (instructionPointer >= 0 && instructionPointer < input.Count - 1)
                {
                    registers[pointerRegister] = instructionPointer;
                    var instruction = instructions[instructionPointer];
                    var opcode = opcodes.First(op => op.Name == instruction[0].ToString());
                    opcode.Operation(registers, int.Parse(instruction[1].ToString()), int.Parse(instruction[2].ToString()), int.Parse(instruction[3].ToString()));
                    instructionPointer = registers[pointerRegister];
                    instructionPointer++;
                }

                return registers[0];
            }
        }
    }
}
