using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D12P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_12_t_1.txt").ToList();
            Assert.AreEqual(325, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_12.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public class Note
            {
                public bool PreviousPrevious { set; get; }
                public bool Previous { set; get; }
                public bool Current { set; get; }
                public bool Next { set; get; }
                public bool NextNext { set; get; }
                public bool Result { set; get; }
            }

            public class Pot
            {
                public int Index { set; get; }
                public bool CurrentValue { set; get; }
                public bool LastValue { set; get; }
                public int AddedAtGeneration { set; get; }
            }

            public LinkedList<Pot> pots = new LinkedList<Pot>();

            public long Generations = 500000000000;

            public long RunChallenge(List<string> input)
            {

                var notes = input.Skip(2).Select(raw => new Note
                {
                    PreviousPrevious = raw[0] == '#',
                    Previous = raw[1] == '#',
                    Current = raw[2] == '#',
                    Next = raw[3] == '#',
                    NextNext = raw[4] == '#',
                    Result = raw[9] == '#'
                }).ToList();

                var initial = input[0].Substring(15);
                var currentGeneration = 0;

                pots.AddFirst(GetNewValueContainer(0, currentGeneration, initial[0] == '#'));

                for (int i = 1; i < initial.Length; i++)
                {
                    pots.AddAfter(pots.Last, GetNewValueContainer(pots.Last.Value.Index + 1, currentGeneration, initial[i] == '#'));
                }

                GetPrevious(GetPrevious(pots.First, 0), 0);

                System.Diagnostics.Trace.WriteLine(String.Join("", pots.OrderBy(z => z.Index).Select(z => z.CurrentValue ? "#" : ".")));

                var lastIncrease = 0;
                var lastResult = 0;
                var lastIndexTried = 0;

                for (int i = 1; i <= Generations; i++)
                {
                    var current = pots.First;
                    // Push values into history
                    //System.Diagnostics.Trace.WriteLine("b "+ String.Join("", pots.OrderBy(z => z.Index).Select(z => z.CurrentValue ? "#" : ".")));

                    //pots.ToList().ForEach(z => z.LastValue = z.CurrentValue);

                    var currentTemp = pots.First;
                    currentTemp.Value.LastValue = currentTemp.Value.CurrentValue;
                    do { currentTemp = currentTemp.Next;  currentTemp.Value.LastValue = currentTemp.Value.CurrentValue; } while (currentTemp.Next != null);

                    var runNext = false;
                    do
                    {
                        
                        var lastFalse = current.Previous == null || !current.Previous.Value.LastValue;
                        var lastLastFalse = current.Previous == null || current.Previous.Previous == null || !current.Previous.Previous.Value.LastValue;
                        var nextFalse = current.Next == null || !current.Next.Value.LastValue;
                        var nextNextFalse = current.Next == null || current.Next.Next == null || !current.Next.Next.Value.LastValue;

                        if (lastFalse && lastLastFalse && nextFalse && nextNextFalse && !current.Value.LastValue && (current.Value.Index > initial.Length || current.Value.Index < 0))
                        {
                            //runNext = true;
                        }
                        
                        var anyNoteMatch = false;
                        notes.ForEach(note =>
                        {
                            if (NoteMatch(note, current, i))
                            {
                                current.Value.CurrentValue = note.Result;
                                anyNoteMatch = true;
                            }
                        });

                        if(!anyNoteMatch)
                        {
                            current.Value.CurrentValue = false;
                        }

                        if (current.Next != null && current.Next.Value.AddedAtGeneration != i)
                        {
                            current = current.Next;
                        } else
                        {
                            current = null;
                        }
                    } while (current != null && !runNext);
                    System.Diagnostics.Trace.WriteLine(String.Join("", pots.OrderBy(z => z.Index).Select(z => z.CurrentValue ? "#" : ".")));

                    var newResult = pots.Where(z => z.CurrentValue).Sum(z => z.Index);
                    var currentIncrease = newResult - lastResult;
                    if (currentIncrease == lastIncrease)
                    {
                        lastResult = newResult;
                        lastIncrease = currentIncrease;
                        lastIndexTried = i;
                        break;
                    }
                    else
                    {
                        lastResult = newResult;
                        lastIncrease = currentIncrease;
                    }
                }

                return lastResult + (50000000000 - lastIndexTried) * lastIncrease;
            }

            public bool NoteMatch(Note note, LinkedListNode<Pot> node, int currentGeneration)
            {
                var test = note.Current == node.Value.LastValue &&
                    note.Previous == GetPrevious(node, currentGeneration).Value.LastValue &&
                    note.PreviousPrevious == GetPrevious(node.Previous, currentGeneration).Value.LastValue &&
                    note.Next == GetNext(node, currentGeneration).Value.LastValue &&
                    note.NextNext == GetNext(node.Next, currentGeneration).Value.LastValue;
                return test;
            }

            private LinkedListNode<Pot> GetNext(LinkedListNode<Pot> node, int currentGeneration)
            {
                return node.Next ?? pots.AddAfter(node, GetNewValueContainer(node.Value.Index + 1, currentGeneration, false));
            }

            private LinkedListNode<Pot> GetPrevious(LinkedListNode<Pot> node, int currentGeneration)
            {
                return node.Previous ?? pots.AddBefore(node, GetNewValueContainer(node.Value.Index - 1, currentGeneration, false));
            }

            private Pot GetNewValueContainer(int position, int currentGeneration, bool value)
            {
                return new Pot { Index = position, LastValue = value, CurrentValue = value, AddedAtGeneration = currentGeneration};
            }
        }
    }
}
