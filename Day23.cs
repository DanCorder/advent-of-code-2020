namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Diagnostics;

    public class Day23
    {
        public static string SolveProblem1()
        {
            var cups = ProblemInput.Select(c => Int32.Parse(c.ToString())).ToList();
            var currentPos = 0;
            var currentCupValue = cups[currentPos];
            

            List<int> pickedUp = null;
            List<int> remaining = null;

            for (var move = 0; move < 100; move++)
            {
                var destPos = 0;
                var destValue = currentCupValue - 1;
                
                if (currentPos < (cups.Count - 3))
                {
                    pickedUp = cups.Skip(currentPos + 1).Take(3).ToList();
                    remaining = cups.Take(currentPos + 1).Concat(cups.Skip(currentPos+4)).ToList();
                }
                else
                {
                    var endPos = (currentPos + 3) % cups.Count;
                    var pickUpEnd = cups.Skip(currentPos + 1).ToList();
                    pickedUp = pickUpEnd.Concat(cups.Take(3 - pickUpEnd.Count)).ToList();
                    remaining = cups.Skip(endPos + 1).Take(cups.Count - 3).ToList();
                }

                while(true)
                {
                    if (remaining.Contains(destValue))
                    {
                        destPos = remaining.IndexOf(destValue);
                        break;
                    }
                    destValue = destValue < 1 ? 9 : (destValue - 1);
                }

                cups = remaining.Take(destPos + 1).Concat(pickedUp).Concat(remaining.Skip(destPos + 1)).ToList();
                currentPos = cups.IndexOf(currentCupValue);
                currentPos = currentPos == cups.Count - 1 ? 0 : currentPos + 1;
                currentCupValue = cups[currentPos];
            }

            var oneIndex = cups.IndexOf(1);
            return string.Join("", cups.Skip(oneIndex + 1).Concat(cups.Take(oneIndex)).Select(c => c.ToString()));
        }

        public static long SolveProblem2()
        {
            var max = 1000000;
            // var max = 9;
            var initialCups = ProblemInput.Select(c => Int32.Parse(c.ToString())).Concat(Enumerable.Range(10, 999991)).ToArray();
            // var initialCups = "389125467".Select(c => Int32.Parse(c.ToString())).Concat(Enumerable.Range(10, 999991)).ToArray();
            // var initialCups = ProblemTestInput.Select(c => Int32.Parse(c.ToString())).ToArray();
            var turns = 10000000;
            // var turns = 100;
            var cups = new LinkedList<int>(initialCups);

            var currentPos = cups.First;
            var pickedUp = new LinkedListNode<int>[3];

            long removingTicks = 0;
            long findingTicks = 0;
            long addingTicks = 0;
            Stopwatch sw = null;

            for (var move = 0; move < turns; move++)
            {
                if (move % 1000 == 0)
                    Console.WriteLine(move);

                // sw = Stopwatch.StartNew();
                for (var p = 0; p < 3; p++)
                {
                    var pick = currentPos.Next ?? cups.First;
                    pickedUp[p] = pick;
                    cups.Remove(pick);
                }
                // sw.Stop();
                // removingTicks += sw.ElapsedMilliseconds;

                var destValue = currentPos.Value - 1;

                var pickedUpValues = pickedUp.Select(n => n.Value).ToHashSet();
                while(true)
                {
                    if (destValue < 1)
                        destValue = max;
                    if (pickedUpValues.Contains(destValue))
                    {
                        destValue--;
                    }
                    else
                        break;
                }

                // sw = Stopwatch.StartNew();
                var insertPoint = cups.Find(destValue);
                // sw.Stop();
                // findingTicks += sw.ElapsedMilliseconds;

                // sw = Stopwatch.StartNew();
                for (var i = 0; i < 3; i++)
                {
                    cups.AddAfter(insertPoint, pickedUp[2-i]);
                }
                // sw.Stop();
                // addingTicks += sw.ElapsedMilliseconds;

                currentPos = currentPos.Next ?? cups.First;
            }

            Console.WriteLine(removingTicks);
            Console.WriteLine(findingTicks);
            Console.WriteLine(addingTicks);
            
            var onePos = cups.Find(1);
            var n1 = onePos.Next ?? cups.First;
            var n2 = n1.Next ?? cups.First;



            return ((long)n1.Value) * ((long)n2.Value);
        }

        private const string ProblemInput = @"476138259";
        private const string ProblemTestInput = @"389125467";
    }
}