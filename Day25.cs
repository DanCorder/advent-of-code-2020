namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day25
    {
        public static long SolveProblem1()
        {
            var lines = ProblemInput.SplitToLines().Select(Int64.Parse).ToList();

            var loops = 0;
            var subjectNumber = 7L;
            var value = 1L;

            while(true)
            {
                value *= subjectNumber;
                value = value % 20201227;
                loops++;

                if (lines[0] == value)
                {
                    return transform(lines[1], loops);
                }
                else if (lines[1] == value)
                {
                    return transform(lines[0], loops);
                }
            }
        }

        private static long transform(long subjectNumber, int loops)
        {
            var value = 1L;
            for (var i = 0; i < loops; i++)
            {
                value *= subjectNumber;
                value = value % 20201227;
            }
            return value;
        }

        public static int SolveProblem2()
        {
            return 0;
        }

        private const string ProblemInput = @"3248366
4738476";
        private const string ProblemTestInput = @"5764801
17807724";
    }
}