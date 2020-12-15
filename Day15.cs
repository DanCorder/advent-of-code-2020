namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day15
    {
        public static int SolveProblem1()
        {
            var numbers = ProblemInput.Split(',').Select(Int32.Parse).ToList();

            for (var i = numbers.Count; i < 2020; i++)
            {
                var lastNumber = numbers.Last();
                var index = numbers.Take(i - 1).ToList().LastIndexOf(lastNumber);
                if (index == -1)
                {
                    numbers.Add(0);
                }
                else
                {
                    numbers.Add(i - index - 1);
                }
            }
            return numbers.Last();
        }

        public static int SolveProblem2()
        {
            var end = 30000000;
            // var end = 2020;
            var numbers = ProblemInput.Split(',').Select(Int32.Parse).ToList();
            var nextNumber = numbers.Last();
            numbers = numbers.Take(numbers.Count - 1).ToList();

            var lastSeen = numbers
                .Select((c, i) => new { c, i })
                .ToDictionary(x => x.c, x => x.i);

            for (var i = numbers.Count; i < end; i++)
            {
                if (i % 100000 == 0)
                    Console.WriteLine(i);

                if (lastSeen.ContainsKey(nextNumber))
                {
                    var lastIndex = lastSeen[nextNumber];
                    lastSeen[nextNumber] = i;
                    nextNumber = i - lastIndex;
                }
                else
                {
                    lastSeen[nextNumber] = i;
                    nextNumber = 0;
                }
                // 0, 3, 6, 0, 3, 3, 1, 0, 4, 0
            }
            return lastSeen.Where(kvp => kvp.Value == end - 1).Single().Key;
        }

        private const string ProblemInput = @"18,8,0,5,4,1,20";
        private const string ProblemTestInput = @"0,3,6";
    }
}