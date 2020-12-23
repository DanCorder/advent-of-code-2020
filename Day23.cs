namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

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
            return 0;
        }

        private const string ProblemInput = @"476138259";
        private const string ProblemTestInput = @"389125467";
    }
}