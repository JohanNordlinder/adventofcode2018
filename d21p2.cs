using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D21P2
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

                var instructions = input.Skip(1).Select(s => s.Split(new string[] { " " }, StringSplitOptions.None)).ToArray().Select(i => new Instuction
                {
                    opcode = i[0].ToString(),
                    OpA = int.Parse(i[1].ToString()),
                    OpB = int.Parse(i[2].ToString()),
                    OpC = int.Parse(i[3].ToString())
                }).ToList();
                
                try
                {
                    ExecuteProgram(input, 0, opcodes, instructions);
                } catch (AnswerException e)
                {
                    return e.Answer;
                }

                throw new Exception("Unable to find answer :(");
            }

            public class Instuction
            {
                public string opcode { set; get; }
                public int OpA { set; get; }
                public int OpB { set; get; }
                public int OpC { set; get; }
            }

            public static void ExecuteProgram(List<string> input, int startValue, Dictionary<String, D16P2.Program.OpCode> opcodes, List<Instuction> instructions)
            {
                var registers = new int[6] { 0, 0, 0, 0, 0, 0 };
                var instructionPointer = 0;
                var instructionsExecuted = 0;
                var pointerRegister = int.Parse(input[0].Split(new string[] { " " }, StringSplitOptions.None)[1]);

                var MaxValues = new HashSet<int>();

                while (instructionPointer >= 0 && instructionPointer < input.Count - 1)
                {
                    if (instructionPointer == 28)
                    {
                        if (MaxValues.Contains(registers[4]))
                        {
                            throw new AnswerException { Answer = MaxValues.Last() };
                        } else
                        {
                            MaxValues.Add(registers[4]);
                        }
                    }

                    registers[pointerRegister] = instructionPointer;
                    var instruction = instructions[instructionPointer];
                    var opcode = opcodes[instruction.opcode];
                    opcode.Operation(registers, instruction.OpA, instruction.OpB, instruction.OpC);

                    instructionPointer = registers[pointerRegister];
                    instructionPointer++;
                    instructionsExecuted++;
                }
            }
        }
    }
}
