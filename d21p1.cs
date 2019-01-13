using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D21P1
    {

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_21.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public class AnswerException : Exception
            {
                public int Answer;
            }

            public int RunChallenge(List<string> input)
            {
                var opcodes = D16P2.Program.CreateOpCodes().ToDictionary(z => z.Name);

                var instructions = input.Skip(1).Select(s => s.Split(new string[] { " " }, StringSplitOptions.None)).ToArray();

                try { 
                    var minExecutions = int.MaxValue;
                    var minValue = -1;
                    for (int i = 0; i < int.MaxValue; i++) {
                        var result = ExecuteProgram(input, i, opcodes, instructions);
                        if (result < minExecutions)
                        {
                            minExecutions = result;
                            minValue = i;
                        }
                    }
                } catch (AnswerException e)
                {
                    return e.Answer;
                }

                throw new Exception("Unable to find answer :(");
            }

            public static int ExecuteProgram(List<string> input, int startValue, Dictionary<String, D16P2.Program.OpCode> opcodes, string[][] instructions)
            {
                var registers = new int[6] { startValue, 0, 0, 0, 0, 0 };
                var instructionPointer = 0;
                var instructionsExecuted = 0;
                var pointerRegister = int.Parse(input[0].Split(new string[] { " " }, StringSplitOptions.None)[1]);

                while (instructionPointer >= 0 && instructionPointer < input.Count - 1)
                {
                    if (instructionPointer == 28)
                    {
                        throw new AnswerException { Answer = registers[4] } ;
                    }
                    var IPWAS = instructionPointer;
                    var registersBefore = registers.Select(z => z).ToArray();
                    registers[pointerRegister] = instructionPointer;
                    var instruction = instructions[instructionPointer];
                    var opcode = opcodes[instruction[0].ToString()];
                    opcode.Operation(registers, int.Parse(instruction[1].ToString()), int.Parse(instruction[2].ToString()), int.Parse(instruction[3].ToString()));
                    instructionPointer = registers[pointerRegister];
                    instructionPointer++;
                    instructionsExecuted++;
                    System.Diagnostics.Trace.WriteLine("ip=" + IPWAS + " [" + String.Join(", ", registersBefore) + "] " + String.Join(" ", instruction) + " [" + String.Join(", ", registers) + "]");
                }

                return instructionsExecuted;
            }
        }
    }
}
