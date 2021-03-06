namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day18
    {
        public static long SolveProblem1()
        {
            var lines = ProblemInput.SplitToLines().Select(l => l.Replace(" ", ""))
                .Select(calculate)
                .Sum();

            return lines;
        }

        private static long calculate(string problem)
        {
            while(true)
            {
                var i1 = problem.LastIndexOf('(');

                if (i1 != -1)
                {
                    var i2 = problem.IndexOf(')', i1);
                    var bracketValue = calculate2(problem.Substring(i1 + 1, i2 - i1 - 1));
                    problem = problem.Substring(0, i1) + bracketValue + problem.Substring(i2 + 1);
                }
                else
                {
                    return calculate2(problem);
                }
            }
        }

        private static long calculate2(string problem)
        {
            var next = "";
            var digits = "0123456789";
            var firstString = new string(problem.TakeWhile(c => digits.Contains(c)).ToArray());
            var total = Int64.Parse(firstString);
            char nextOperator = '-';

            foreach(var c in problem.Skip(firstString.Length))
            {
                if (digits.Contains(c))
                {
                    next += c;
                }
                else
                {
                    if (next != "")
                    {
                        if (nextOperator == '+')
                        {
                            total += Int64.Parse(next);
                        }
                        else if (nextOperator == '*')
                        {
                            total *= Int64.Parse(next);
                        }
                        next = "";
                    }
                    nextOperator = c;
                }
            }
            if (nextOperator == '+')
            {
                total += Int64.Parse(next);
            }
            else if (nextOperator == '*')
            {
                total *= Int64.Parse(next);
            }

            return total;
        }

        public static long SolveProblem2()
        {
            var lines = ProblemInput.SplitToLines().Select(l => l.Replace(" ", ""))
                .Select(calculate3)
                .Sum();

            return lines;
        }

        private static long calculate3(string problem)
        {
            while(true)
            {
                var i1 = problem.LastIndexOf('(');

                if (i1 != -1)
                {
                    var i2 = problem.IndexOf(')', i1);
                    var bracketValue = calculate4(problem.Substring(i1 + 1, i2 - i1 - 1));
                    problem = problem.Substring(0, i1) + bracketValue + problem.Substring(i2 + 1);
                }
                else
                {
                    return calculate4(problem);
                }
            }
        }

        private static long calculate4(string problem)
        {
            var digits = "0123456789";
            var plusIndex = problem.IndexOf('+');

            while (plusIndex != -1)
            {
                var left = "";
                var right = "";
                var il = 0;
                var ir = 0;

                for (int i = plusIndex - 1; i >= 0; i--)
                {
                    if (digits.Contains(problem[i]))
                    {
                        left = problem[i] + left;
                        il = i;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = plusIndex + 1; i < problem.Length; i++)
                {
                    if (digits.Contains(problem[i]))
                    {
                        right = right + problem[i];
                        ir = i;
                    }
                    else
                    {
                        break;
                    }
                }
                var total = Int64.Parse(left) + Int64.Parse(right);
                problem = problem.Substring(0, il) + total + problem.Substring(ir + 1);
                plusIndex = problem.IndexOf('+');
            }
            return calculate2(problem);
        }

        private const string ProblemInput = @"3 + 8
7 * (7 * 6 + 7 * 5 + (9 + 2 * 2 * 4)) + 7 * 3 * 5
3 * 4 + 2 + 7 + (4 * (9 + 9 + 2 + 3 * 8) + 9 + (6 + 4) + 7) + 3
(7 * 9 + 2 + (5 + 4)) + 5 + 9
4 + 5 * 5 + 8 + (3 * (7 * 7 + 5) * 7) * 5
2 + 8 * (5 + 4 * (8 * 3 * 6 * 7 * 8 * 8) + 8 * (7 + 9 + 8 * 9 * 4) * (3 + 9 * 5 + 2 + 5 * 4)) * 4
(8 * (3 + 3 * 9) * 3 * 9 * 9) + 3
(8 * 2 * 4 + 5) * 2 + 2 + (3 + 4 * 2 * 3 + (4 + 7 + 8 * 7 * 5) * (9 + 9 * 8))
(7 + 2 + (2 * 3)) * ((8 + 3 * 4 + 3 + 9 + 4) + 5) + 8 * 7 + 5 * 5
7 + (3 + 6 + 8) + (5 * 9 + 7 + 2 + (2 * 9 + 8) + 2) * 9
8 + 4 * 4 * 6 + 9 + ((5 + 8) + 7)
(6 + (8 + 6 * 8 + 7 + 3 * 3) + 5 + 7 * 6 + 8) * 2 * 2 * 5 * 2 * 4
(4 + (7 * 4 + 7 * 3 * 9 + 2) * 6 * 2 + 6) * (8 + 5 * 6 * (3 + 3 * 8 + 3) * 9) + 9
7 + 9 * ((5 + 8 * 5 * 6) * 2 * 5) + 9 + 9 * (6 * 8 * 4 + 3)
4 + (8 * 7 * 7 + 9) + (7 * 9 + 4) + 5 * 3 + 6
7 + (3 + 6) * (5 * (5 * 7 + 5 * 6 + 3 + 3) + (3 * 4) * 8) * 2 * 4
2 + (6 * 7 * 8 * 6 + (7 + 4 * 3 + 3 * 2 * 9) + 8) * (3 * 4 * (2 * 9 + 9) * 3) + (8 + (4 + 9 * 5 * 2 + 2) + 3 + (9 * 6) + 5 + 9) * 7 + ((2 * 3) + 3 * 8 * 8 * 6 + 5)
9 * 4 + ((5 * 2 + 5 * 5 * 6) * 5 + 9 + 8 * 9)
3 + ((6 * 3 * 9 * 4 + 2) + 6 * 8)
(3 + 4 + 7 + 3) * (6 + 8 * 7 + 8 + (6 + 6 + 8 * 7)) * 5 * 3
2 + 8 + (9 + 4)
((5 * 9 * 5) + 7 * 5) + 3 * 8
((7 + 9 + 5 + 4) * 9) + ((2 * 2 + 5 * 2 * 7) * (6 + 6 * 7 * 2 + 3) * 8 + 5 + 2 * 6) + 7
2 + (9 * 9 + 5 + 6) * 2 * 9 + 3
(5 * 4 * (7 * 9) + 7) * 8 * 5 * 4 + 6
5 + (7 * 4 * 2) * (8 + 7 + 2 * 2 * 7 * 2)
8 * ((5 + 7 + 2 + 9 * 8 * 2) + 9 * 6 * 9 + 7) + 8 * (3 + 5)
3 * 3 * 2 + 9 + (8 * 2 * 5 * 2) * 6
3 + 5 + 5 + 4
2 * 7 + (7 + 2 + 4 * 7 + (9 * 5 + 7 * 6 + 9 + 9) + 2) * 9 + 6 * 6
(2 * 6 + 8) * 3
4 + (7 + (7 * 7) * 3 + (4 * 5 + 4 + 6 + 3) + 5 * 8)
((8 * 9) + 3 * 8 * 7) + ((2 * 4 + 8 * 2 + 8 + 5) + 9 + 4) + 4 + 9 * (7 + 9 * (5 + 7) + 7 * 5 * 8) + 8
((2 + 6 + 9) + 5 * 4) * 6
9 * 5 * (2 * 6) + 6 + (9 * 2 * 8)
3 * 3 * (3 * 9) + 7
9 * ((2 + 7 * 7) * 3 + 8 * 3 + (6 * 2 + 5 + 3 * 3 * 9)) + 3 * 5
(6 + 6 * 6 * 6 * 3 * 3) * 7 * 4 * 9 * 7
2 * ((5 * 4 * 7 + 3) + 6 * (5 * 5 * 2 + 8 * 9 + 5) + 7) + (7 * 8 + 7 + 7 * 9 * 6) * 7 * 8
(5 * 8 * 9 * (6 + 7) + (5 * 3 * 3 * 6 + 4 + 6)) + 2 * 7 + 9 * 8
2 + (7 + 4 + 8) + (7 + 6 + (2 + 9) * 3)
((3 * 2 + 8 + 2) + 7 * 8 + 6) + 9 * 7
6 + (9 + 2) * 8 + ((2 + 6) * 7 * 2) * (9 * 6) * 3
3 * ((5 + 5 * 3 + 8 + 5 * 2) + (5 * 8 * 4 * 5 + 4 + 8) + 6 + 8 + 5)
((4 + 6 + 8 + 8) * 4 * 9 + (6 * 2 + 4 * 5 + 5) + (9 + 4 + 7 + 4 + 5) + 8) * 7 * 5
8 + 9 + (5 * 3 * 9 + 5 * (4 * 7 * 6 + 5 + 9)) * 5 * (9 + 8 + 7 + 4) * 6
4 * 6 + 9 * (9 + 5 + 7 * 4) + 7
(5 * 6) + (2 + 9 + (6 * 2 * 5) + (2 + 8 + 2) + 9) + 7 * 6 + 5
5 * 4 * 7 * (5 * 7 + 2) + 5 + 9
3 * (7 + 2 + 8 + 7 + 5 + 3)
2 * (2 * 5 + 7 + 6 * (8 * 8 * 5 * 4 * 8 + 8)) + 3
(7 * 9 * 7) * 2 * 9 + (7 + 3 * 6 + 3 * 3) * 8 + 9
5 + 8 * 9 * 7 + (4 + 9 * 5 + 2 * 3) + 4
7 + 3 * (3 + 8 + 5 * 6) * (8 + 6 + 8) * (5 + 2 + 8 + 5 + (4 * 6 * 7 * 5 + 4))
((6 * 6 * 7 * 2 + 7) + (3 * 7 + 9 + 7 + 5 + 5) + 2 * 8 * 3 + (3 * 9 + 7 + 7 * 7)) * ((6 + 8) * 3 + 2 * 4) + 3 + (6 * 4 + 7 + 2 * 9 * 2)
(9 * 7 * 6 + 2) + 9 * 6 + 4 + (5 + 3 * 8)
(4 + 2 + 3 + 9) + 6 * 4 + 2 + 9
9 + ((5 * 8 * 8 * 2 * 5) + 8 + 7 * 4) + ((6 + 8 + 4 + 5 * 3 * 5) + 5 + 4 * 6 * 3)
9 + (6 + 3) + 3 * ((2 + 3) + 8 + 4 + 8 + (2 + 9)) * 4 * 2
7 + 8 + (2 + 9 * 9 + 3 * 9) + 6
(8 + (8 * 6 + 9 * 4 + 2) + 4 + 9) + 7
6 + (2 + (4 * 3 * 5 + 5) + 4) + 6 * 6
7 * (3 * 9 * 3) * (7 + (6 + 8 + 4))
8 * ((4 * 3 + 8 * 6) + 7 + 6 * 3) + (4 + 5 * (8 * 2 * 6 * 9) + 7)
5 * ((8 * 4 + 2 * 9 * 8 + 2) * (3 + 5 + 6 * 9 + 5) * (5 + 9 * 9 + 2 * 8) * 2)
(3 * 4 * 5 * 3) * ((8 + 6) + 2 * (6 * 9 + 9 * 9 + 2 * 7) + 6 * 2) * 5 + 4 + 8 + 2
8 * (5 + 2 * 2) * 2
(3 * 8) + 9 * (6 * 2 + (6 + 3 * 6 + 7 + 8 + 4) + 3 * 9)
3 + 6 * (3 + 6 * 8 * 4 * 5 + 2) + (3 + 9 + 2 * 7 * (4 * 3 + 3 + 8) * (8 + 3))
4 * (4 + 3 + 7 + 9 * 6) * 2 + 4 + 8 + (2 + 2)
((5 + 7 * 2 * 9) + (2 + 3 * 6 + 2 * 5 * 8) * 8 * (8 + 8 * 6) + 5 * (7 * 9)) + 7 * 9 * (6 + 8)
5 * 2 * 4 * ((3 * 4) * 6 + 3 + 5 * 5 * 9) + 7 + 8
6 + 8 + (9 * 2 * 9 * 9 * 4 + 3) + 9 + (4 + 2)
9 + 5 * 7 + 2 * 4 + (4 * 8 + 5)
3 + 5 * 8 + 5 * 5
(6 + 6 + 9 * 7 + 4 * 5) + 2 + 4 * 9
((3 + 2) + 2 * 4 * 4 * 7) * (9 + 8 * 6 * 3 + 4 + 6) + 3 + 9 * (7 + 5 + 5) + 3
7 * (8 + 6 * 9 + 8 * 5)
9 * 4 + 8 + 2 * 3
9 + 2
3 + 2 + 9 + 8 + (2 * 9)
4 * 9 * (3 * (2 * 7 + 6) + 9 + 7 * 5) + (9 * 3 * 5) * (6 * 4 * 9 * (5 * 7 + 7 + 2 * 7) * (5 * 5 + 8 + 8) + 8)
5 * 7 * 4 * (3 * 8 + 6 * 2 * (8 * 2 + 4) * 4) + 7
6 * 3 * 2 + (3 * 8) * 4 + 7
6 + ((4 * 4) * 3 + 8 + 3 * (3 * 6 + 9 * 3 * 4) + (7 * 8 + 2 + 7 * 4)) + 7 + 2
8 * 9 * (2 * 5 + 6 * 7 * 9)
9 + (5 + (8 + 2 + 2 * 3)) + 7 * 2
(5 + (3 * 8 + 4 * 3) + (3 + 6 * 2 * 9) + (8 + 7 * 7 * 5 + 2 + 9) + 6) * 5 + 7 * ((6 + 2 + 9) * 4 + 4 * 7 * 3) * 8
4 * (9 * 4 * 9 * 4 + 4) * 6 * 7
3 + 2 * (6 * 5 * 5 * 7 * (7 * 8 + 4)) * 2 * 7 + 4
9 * (4 + 4 + 8 + 5 * (7 * 5 + 2 + 8 + 3 + 4) * (2 + 9 * 3))
9 + ((9 * 7 * 9 + 4) * 3 + 3) * 2 + 2 * (7 + 6 + 2 * (2 + 8 + 7 + 6) * 5)
8 * (7 + 2 + 4 * 9 * 8 * 4) * 8 + 7 * 9 + 2
(9 + 4 + (9 * 5 * 2 + 7 * 9 * 9) + 3 + 5 + 6) + (8 + 5 * 8 * 5 + 8 + 2) + 8 * 2
((7 + 8 * 6) * 7) + (4 + 3 + (8 + 2 * 9 + 8 + 3 * 3) * (6 + 5 * 4 * 8 + 8)) * 5 * 6 + 5 * 9
((8 * 6 + 6) * (6 * 7 * 9) * 3 + (5 * 4 * 9 + 4 + 7 * 2) * 4) + (8 * (8 + 4 + 8 + 2)) + 4
(2 + 2 * 4 * 6 + 6) + 4
((2 * 4 + 9 * 2 * 3 * 4) * 8 * 5 + 8) * 6 * (5 * (6 * 4 * 9 * 3) + (2 * 2 * 2))
(4 + 9 * 2 + 9 + 2 * 7) * (5 * (9 + 7 + 3) + (6 * 4) * (4 + 6) + (3 + 5 + 7))
2 + (9 + 5 + 6) + (4 * 5 * 9 + 5 + 2 * 2) + 3
((4 + 6 * 2) + 4) + 3 + (3 + 7 * 6 + 7 * 6) + 9 + (3 + 4) * 7
6 * 4 + ((6 + 4 + 2 * 6 * 5) + 4 + 4 + 8 + (4 + 2 * 3 + 9) + 9) + 3
2 + (4 + 5 + 6 * 2 * (5 * 5 * 9 * 9)) + ((8 + 3 + 9 * 5) * 3 * 2 * 4 + 9 * 3) + (9 * 8 + (7 * 4 + 6 * 9 + 8)) * 7
5 + (7 + 3 + 6) * 8 * 5 + 9
8 + (5 * (3 * 7 * 9 + 4 + 7)) * 2 + 8 * ((9 + 2 * 9 * 6 + 7 * 7) * 7 * 3 + (4 + 5 + 4 * 7) * (9 + 2) + 6)
5 + (6 + 9 + (2 * 6 * 3 * 2) * 4 + 5 * 3)
(3 * (3 + 7) * 6) + 5 * 6 * (3 * 2 * 3 * 7 + (3 + 4 + 4 * 7))
9 + (2 + 5) * 6 * (5 + 6 + 9 * 6 * 7)
6 * 5 * ((3 + 3 * 6 * 7 * 5 * 6) + (3 + 5 * 4 * 5) * 8 * 3 * 9 * 8)
(6 + 7 * (5 + 6)) * 6 * 9
2 + (3 + 6 + 5 * 3 + 3) + ((6 * 7 + 5 * 8 * 8) + 7 * 8 + 3 * 2) * 9 + 5
((3 * 9 + 4) * 5 * 9 * 7) * 8
(4 + 6 * 6 + 8 + 4) + 8 * 9 + 8 + 3
2 * (8 + (3 * 9 + 2)) * 7
2 + (4 + 6 + 4 * 9)
5 + 9 + (4 * 8 * (3 * 7 * 9 * 2 * 6) + 7)
(3 + 7 * 6 * 9 + (8 * 6 + 8)) + 3 + 3 * 9 + ((9 * 4) + 5)
(6 * (9 * 5 + 9) + 4 * 2 + 5) + 5 * ((2 + 9 * 7) * 4 + (6 * 5 + 2 * 6 + 8 + 7) + 7) + (5 * 6 + (2 * 5) + 7)
7 + 4 + 2 * (6 * (4 * 4 * 9 * 8 + 5) * 7 + 3)
8 + (7 + 8 * 3 * (6 * 8 + 5) * (2 * 9 * 4 * 6 + 7 * 7) + 3)
(7 + 9 * (3 * 6 + 5 + 2) + 5 * 2 + 2) * 6 * (3 + (3 * 8 * 6) * 2 * (4 + 5 * 6) * 5 + 5) + 8
5 + 5 * 7 * (4 * 2 * 5 + 5) + 4 * 5
3 + 4 + 3 + 5 * (2 * (7 + 9) + 2 * 5)
(2 * 7 * 7) * 4 + 4
((4 * 5 + 6) + 7) + 4 * 3 + 5 * (9 * 7 + 9 + 8 + 6) * 6
7 * 6 + 4 + 5 + (5 * 5 * 2) + (5 * 2 + 5 + 6 + (3 + 3 * 4 * 7 * 9))
5 * (2 * (6 + 6 + 4 + 4 + 9 + 5) * 4 + 9 * 8 + 2) * 4 * 4
2 * 6 * 3 + (7 + (9 * 6 * 5 + 6 * 7) + 2 * 2) * 9 + 5
(9 * 6) + (2 * (8 * 4 + 3 + 8 + 8)) * (2 + 4 * 5 + 3 + 4) + (5 * 7 * 7 + 7 * 7)
7 * 2 * ((6 * 5) + (3 + 9 * 3) * (4 + 3 + 9 + 8 + 3))
8 * ((4 * 4) * (6 + 6 + 3 * 5 * 3) * 4 + 3 * 4) + 5 * (6 * 4 * (5 + 7 + 7 * 5 + 4) * 5) + 7 + 5
((6 + 2 * 9 * 2 * 5 * 7) * 7 * 4) * ((4 + 4 + 4) * 8 + 4 * 8) + (6 + 8)
2 * 7 + 3 + 4
(9 * (4 + 7 * 2 * 6) * (5 * 5 + 2 + 2 * 5)) + 8
4 + ((5 + 3) * 4 * 9 + (3 + 5 * 6) * (3 + 8 * 2 + 3)) * 6 + 9
7 * (4 + 6 + 2) + 7 * 2 * 7 * 6
6 * ((4 * 9 * 5 * 4) * 7) + 4
9 * (6 + 4 + 9 * 9) * 4 * 7 + 2 * 9
(9 * (4 * 7)) * 8 * ((7 * 8 * 7 * 3 + 8) + 2 * 2 * (5 + 6) + (2 * 6 * 5 * 5))
8 + 2 + 2 * 6 * 2 + 8
(5 + 3 + 3 * 5 + 4) * (3 * 6 * 7 * (2 + 4 + 3 + 5 * 4) + 8 + 4)
(6 + 6 * 4 + 7 + 4) + 9 * 7 + (5 + 7) * 5 * 4
((3 * 4 * 8 + 8 * 8 + 9) * 2 + 8) * 5 * 8 + 3
2 * 2
(9 + (3 + 2 * 3 + 4) * 4 + 8) * 8 + (7 + 6 * 3 * 7 * 4) * 9 + 4 * 8
7 + 3 + 6 + 6 + 5 * 2
8 + 2 * 3 * 3
7 * 6 * (5 + 2 * 6 * 7 * 6 * 2) + 6 + ((7 * 5 + 9 + 4) * 7 * 6 + 9 * 8 * 2) * 5
(3 * 3) * 2 + 4 * 2 + 3
4 * (5 * 6 * 2 * 4 * (8 + 3 * 5) * 9) + 5
(8 + 5 + 7 + (7 + 6 * 7)) * 7 + 2
9 * 2 + 8 + (3 * 9 + 6 * (8 + 6 + 8 * 3 * 8)) + ((5 * 5 * 5) * 2)
2 * (2 + (8 + 7 * 7) * 9 * 7 * 3 * 9) + 7 + 4 * 3 + 7
8 * 4 + (2 + 3 + 4 * 8)
((6 + 2 * 6 + 8) + 4 + 4 * 4 + 8 * 7) + 5 * (4 * 4 + 7 + 3 + 2)
7 * (2 + 9 * 8 * 2 + 5 + (8 + 9 + 6))
2 + 8 + (5 + 8 + 2 * 6) * 4 + 2
((9 + 6 * 8 * 4 + 2) * 2 + 4 * 6 * 5) * 5 + 6 * 3 + 4 * 6
(9 + (6 * 7 + 2 * 7 * 7) * 6) * 6 * 6 * 4
(4 + 2 + 3 + 3 * (8 * 5)) + 6 * 2 * (8 + 7 + 3 + 9 * (2 * 4 * 8 * 6 + 8 * 5))
7 * 4 + (9 + 6 + (9 * 4 * 7 * 8 * 3 + 4)) + (2 + 8 + 7 + 5 + 7) * 2 + (8 * 6 + 5 * 6 * 9 * 8)
(9 + (3 * 6) * (5 + 2)) + 5 * 7
9 + (8 * 4) * 2 * 6
8 * 3 * (9 * (7 * 6 + 7) * (7 * 8)) + 3
8 * 7 * 6 * (7 * 7 + 3 + 2 * 6) + (8 + 2 + (7 + 9)) + (5 + 5)
(2 * 5 * 7 * 8) + 6 * (5 * 8 + 9 * 8) * 7
(2 + 8 * 6 + (7 + 9 * 8 + 2)) + 6 + (3 + 4 + 4 * 2 * 4) + 8 * 8
7 + ((4 + 7 + 9 * 9 + 7) + (2 * 5 * 8 * 7 * 7 * 8) * 9 + 6 + 9 * 5) * 4 * 4 * (7 + (3 + 7 + 3 + 4) * 6 * (5 + 4))
3 + ((6 * 6 + 8 + 6 * 2) + 6 + 3 + 2 * 5 * 7)
9 + 8 + 9 * (2 + 7 * 5) * 4
9 + (9 * (8 * 7 * 2 * 6 + 2 * 9)) + 7 * 3 * 2 * 5
8 * 9 * (2 + 2 * 6 + 6) + 5
(6 * 5 + 8 + 8) + 7 * ((4 * 6 * 3 * 8 * 3 * 5) + (5 + 6 * 8 * 3) + 3) * 7 + 6
7 + 2 + ((6 * 7) * 8 + 7) + 6 + 3 * 9
6 * (2 * 2 + 8 * 4 + 3 * 9) + (6 * 2 * 4 + (2 * 5 + 4 * 8) + 9) * 7
7 * 5 * (6 * 2 + (4 * 5 + 2) + (2 * 5 + 5 + 5 * 7 * 6) * (6 + 5 * 8 * 2))
9 * ((7 + 7 + 2) + (5 * 5) + (2 + 3 + 2 * 8 * 7) * 7) * 3
(7 + 6) + 3 * 3 * 4 * (2 + 7 * 4 * 4 * (5 * 3 + 8 + 6 * 8))
4 + ((6 * 3 + 8 * 6) * 3 + (8 * 4 + 9 * 6 + 4 + 3) * 8 * 2) + 4 * 8 * 2 * 9
(9 * (3 * 5 + 8 + 3 + 8) * 3 + 9) + 5 + 5 + 8 + 2
(6 + 3 * 7 + 9) * 9 * 6
(5 * 2 + 2 + 2 * 7) + 7 * 7 + (6 + 7 + 9 + 9 + 5) * 7 * 5
5 + 8 * (2 * 2 + (6 * 3 * 6 * 7 + 8) * 4 + 6)
4 + (8 + 2 + 9 + (2 + 4) + 4) * 5
7 + 8 * ((9 + 8 * 6) * 8 * (4 * 2 + 3 * 7 * 9 + 6) * 2)
(8 + (2 * 8 + 6 * 9 * 6 * 4) * (5 + 5) + 6) + 7 * 8 * 8 + 5 * ((5 * 3 * 7 * 2) * 5 + 4 + (2 * 6 + 6 + 2 + 2) * 4 * 2)
6 + (9 + 8 + 5 + 7) + 3 * (7 * 6 * 5 * (2 * 8 * 7 * 5) + 3 * 5) + 3
9 * (6 * (8 + 4 * 8) + 4)
(9 + (5 * 7 * 7 * 8 + 9) + 5 + 5 * (8 * 7 + 2 + 9 + 9) * (9 + 6)) * 3 * ((8 + 3) + 8 * 4 + 2 * (8 + 6 + 4) * 4) + ((8 + 3 + 8 + 7) + 7 * 8 * (7 * 7) + 5) + 9
((3 * 4 * 9) + 2 + 8 + 5 + 2 + 3) * (6 * 8 * 8 + 6 + (4 + 6 + 8)) * 2 * 6 + 5 + 9
2 + 2 + (9 * (2 + 5 * 8 + 2 * 5 * 5) * 3)
(6 * (2 * 8 + 6 * 4 * 9 + 9) + 8) + 3 + (3 + 2 + 2 + 8 * (3 + 4 + 8 * 9 + 2 + 5)) + 3 * 7 + 5
(2 + 5) * (2 + 5) * 3 * (2 + 7 + 8) + 2 + 8
3 + 8 + (7 * 2 + (9 + 2 + 8 * 4) + 8 * 8) + 4 * (5 + (6 * 5 * 8) + 7 * 8 + 9 + 8)
9 + 4 * (4 + (5 * 3 + 7 + 8 + 7 * 4) * 6 + 8 + 9 + 9) * ((4 * 6 + 8) * 9 * 9 * 6)
8 + 7 + ((3 * 6 + 2) + (8 * 2) + 3) + 7
(6 + 4) * 9 + 9
6 * ((8 + 5) + 7 * 3 * 8 * 5 * 5) * 4 + 4
7 + ((4 + 7 * 3 * 3 + 8) * 5 * (8 + 2) * (2 + 6 + 5 * 7 * 4 * 8) + (7 + 5 + 2) + 8)
9 + 7 + (7 * 8 + 9) + 3 * 6 + 6
(5 * 9) + 5 * 2 + 8
(9 + 4 + 7) * ((9 * 7) + 9) + (3 * 9) + ((7 * 5 * 2) * (6 + 9 * 8 * 2 + 5 * 6) + 8 + 9 * 8 + (4 * 9 * 2)) + 6
4 * 8 + 7 * ((6 + 9 * 2) * (4 * 7)) + 9
8 + ((2 + 2 * 6) * 6 + 2 + 4 + 9 + 5) * 3
8 + 7
3 + 7 + (9 + 4 + (8 * 4 * 7 + 7 * 3 + 5) + (9 + 7 + 2) + 9) + 5 + 5 * 2
6 * 7 + (3 + (3 * 3) + 4 + (2 * 7 * 2 * 3) * 7 + 6)
5 * (4 + (7 * 7 + 4 * 5 * 2 + 6) * 3 + 5 + (4 * 3 + 3 + 6 * 6) * 7) + 3 + ((2 * 8 * 4 + 8) + 6 + (5 * 9 + 2 * 7) + 4 * 8) * ((3 * 5 * 3 * 9 * 7) + 5 + (3 + 9 + 9 + 2 + 5)) + (3 * 7 + 9)
(3 * 7) + 5
(2 * 5 * 6 + 6) * 2 * (9 + 3 + 6)
2 * 2 + (6 + (4 + 2) + 4 + 9 * 9) + ((9 + 3 + 6) + 2 * 3 * 5 + 3 * 3) * 8 * (2 + (6 + 3 + 2 + 9 * 6 * 6))
8 + (6 + 8 * 8 + 3 + (4 * 7 + 6 * 3 + 6 * 8) * 6)
(8 * 8 * 4) * 5 * 9 * 9 * ((9 * 2 * 8 * 2) * 4 + (2 + 2) + 8 + 9) * 4
(6 * 3 + (8 + 5 * 9 + 2 + 8) * 3 * 7 * 2) + 9 + 8 + 2
(2 * 4 * 3) * 4 + 6 + ((5 * 7 * 4) + 7 + 4 + 5)
(8 + 5 * 2 + 5 * 3 * 9) * 8 + 8 * 2 * 8 + 5
4 + (6 + 6 * 9 * 5) * (4 + 5 * 6 + 8) + 8 + 8
6 + (5 + 7) * 3 + (7 + 2 + 4 * 5 + 2 * 4) + (6 + 4 * 5 * 7 + 4 + 5) * (5 + 6 + 8 + 8 + 9)
(5 * 7) * 6 * 8 * 9 * 6 * 6
6 + 8 + 7 + 3 * (2 + 9 * 7)
3 + ((8 * 5 + 8 * 8 * 9 + 8) * 2 + 6) * 2 + (2 + 6)
6 + 6 + (3 * 7 * 8 * (3 * 4 + 2 + 2 + 3)) + ((6 * 3 + 9 + 2 + 5 * 9) * 9 + 2 * 7 * 2 + 4) + 9 + 4
9 * 8 + ((6 + 8 * 2 + 7 * 6 * 4) + 4 + 3 + 7 + 5)
((2 + 9 + 2 * 9 + 3 * 5) * 3 + 3 * 4) * 4 + 4
6 * (8 + 7 * 5 * 8 + 2 * 2) + (4 * 4 + 8 + 3) + 3 * 5
7 + (8 * 3 * (5 + 7 + 6) + (9 + 9 * 9 * 6 + 4 * 7)) * 7 + 9 + 4
6 * (9 + 7 * 6 * 5 * 8) + 5
2 * 6 * (4 + (4 * 7 * 2 + 9 * 9) * 5 + 2) + ((5 + 6) * 3) + 8 + 2
5 + 4 * 9 + (5 * (8 * 9 + 2 + 3) * 6) * 3 * 5
5 + 9 + 9 + (5 + 2 + 8) * 3 * (7 + 6 + (7 * 7 + 7 + 7) * 6 + 7 + 2)
(2 + 6 * 4 + 9 * 2 + 2) + ((3 * 8 * 6) + 2 * (9 * 5))
5 + 3 * (3 * 5 * 3 * (7 + 6 * 9 + 3 + 8 + 4) * 9) + 2 + 7 * (6 * 8 * 7)
(4 + (9 * 5 + 3 + 4 * 5) * 6 + 9 * 6) * 5 * 9
4 * (2 + 4 * 4 + (2 + 6 + 8 * 2 * 8 * 8)) * 4 * 6 + 9 + 3
(8 + 9 + 4) + 2 + ((5 * 7 * 3 * 9 + 3) * 6 + 4) * 3 + (2 * 5 * 3 * 3) + (7 + 2 * 9)
8 + (6 + 4 * 8 * (9 + 6) + 5 * (5 * 8 + 9)) + 2 * 5
7 * 5
(4 * 3 * 3) + 7 * (8 * 4 + 3 + 9 + 5 + (6 + 4 * 8 + 3 + 7)) * 6 * 9
9 * 2 * (9 * 9 + 4 + 2 * 9) * (3 + 5 + 8)
3 + (3 + 6 + 8 + 3 * 7) + 6 + (8 + 7 + 5 + (4 + 8 + 5 + 4)) * 6
7 * 2 + 3 + 8 + 2
9 * 6 * 5 * 9 * (3 * (8 + 9 + 5) * (5 + 3 + 8) + 5) + 5
6 + 6 + ((6 + 2 * 7 + 5 + 3) + 4 * (8 * 4 * 2 * 2 + 7)) + 8 + 5 + 3
9 + (2 * 7 * 4 * 5 + 6 + 4) * 5 * 2
7 + ((2 * 4 + 2 + 4 + 3 + 3) * 9 + 5 + 6)
2 * 9 * 8 * (5 + (4 + 5 + 2 + 7))
2 * 6 * 4
(8 * (9 * 6 * 9 * 2 + 4) * 6 + 7 * 6) * 6
8 * (3 * 3 * 4 + 8) * 8 * 7
(8 * 3 * 8 * 8 + 4 + 8) + 4 * (9 * 3) * 7
((8 * 9 + 2 * 4) * (4 + 3 + 8 + 9 + 8) * (3 * 8 + 7) + 6 * 3) + 5
2 + (8 * 9 * (7 + 6) * (4 + 7 + 7 + 7))
4 + (8 + 6 + (8 + 6 * 6 + 7 * 9) + (3 + 6 * 5 * 6)) + 5 * 8 * 7 * 9
(7 * 7 + 8 * 4 + 4) * 3 + 2 * 4 + 5 * (4 * 7)
((4 + 2 * 6 * 8 * 9) + (8 * 5 + 8) * 9) * 9 + 7 * 7 + 9
6 + 3 + ((5 + 6 * 6 * 4 * 3 * 6) + 4) + 4 + (6 * 7 * 4 * 7 + 4 * 5) * 4
3 * 2 + (4 * 4 * 3)
9 + 7 + (8 + 4 * 5 * 7 + 3) * 9 * 6 + 4
(3 * 8 + 9 + 8 + 5) + (2 * 6 * (2 * 4)) * 7 + (7 + 9 * (4 + 9 * 2 * 9 * 9) * 2) + (4 * 2 * 3 * 9)
2 + 3 * 4 * 6 + 6
8 * 4 * 8 + (2 * (5 + 9) * 3) * 6 * 4
8 + (5 + 3 * 3) * 8 * (9 * 9 + 3 * (9 + 9 * 5 + 8 * 4 + 4) + 9) * ((5 * 7 * 7) * (9 + 6) * 6)
2 + 5 + 7 + 8 * (4 + 9 * 4) + 7
(7 + 9 + 7 + 2 * (5 + 3 * 6 * 5) * 4) + 5 + 8 * 3
8 * 7 + (4 * 8 * 9 * 7 * 7 * 7) * 5 + 6 * 4
((8 + 6 * 9 + 7 + 3) + (8 * 2 + 9 + 6) * 3 * 8 + 9 * 5) + (3 * 8 * 2 * 9 * 5) * 3 + (7 + (7 + 4 + 3) + 3) * (9 + 9 + 6 * 9 + 2 * 9)
(9 * 3 + 2 + 5 * 9) * 5 + 6
(3 * 6 + 5 * 9 + 2 * 6) + 2 * ((9 * 3) * 2 + (9 + 7) * 4 + (2 * 8 + 2 * 4 * 9) + 7) + 9 * (5 * (6 * 8 * 9) * 4 * 6 + 2) + 2
2 + 9 + 6 + (6 + 6) * ((4 * 7 * 6 * 8 + 3 + 3) + 9 + 7) + ((2 * 2 * 6) + (9 + 2 + 6 + 2 + 8) + 4 * 7 * (7 + 4 + 4 * 5 + 8) * 4)
7 * 4 + 9 + (5 + 9 + 9 + 4)
2 * 8 * 2 + (4 * 9)
7 * 3 + (5 + 8 * 4 * (9 + 6 * 8 + 2 * 3 + 9) + 2 * 9) * 9 * 5 + 9
2 * (5 * 6 + 8 * 2 + 6) * 3 * 3 + 7
(6 * 5) + 8 * 3 + 9 * (7 * 4 * (2 * 6 * 6 * 2)) + 6
6 * 5 * 2 * 8 + 8 + (6 * 5 + (7 * 7) + 3 * (9 * 8 * 2 * 5 + 4 + 5))
2 + 3 + 7 + (8 * 2 * 2)
6 + 3 * ((9 + 7 * 4 + 4 * 3 * 6) + (3 * 5 + 8 + 3 * 3 + 4) * 5 + 5 * 5) * ((4 + 2) + 9 * 4 * 9 * 5)
8 + 7 + 6 * 2 * ((8 * 3) * 3) * (2 + 5 * 5 + 5 * 8 + 8)
5 * 2 * 8 * 4 * 7 * 2
4 * 9 + 8 * ((9 * 5) + 5 * 2 * (6 * 4 + 2) * (2 + 8 * 8 + 8 * 2))
3 * 2 + ((8 * 8) * 2 + 8 + 5 * 8 + 9) * ((4 * 7 * 4 + 5 + 6) * 6 * 3)
(3 * (3 + 3) * 2 * 3 * (4 * 2 + 9 + 3 * 5 * 3)) * (2 * 2) * 3 + (5 * 5 * 6 + 2 + 2 * 3) + 7 * 2
4 * (3 + 2)
3 * 9 * 2 + (5 * 7 * 5 + 7 * 5 * 9) + 9
2 + (5 * 7) + 3 + 7 * 8 * 8
((4 * 6 * 8 + 4 + 4) * (2 + 3 + 5 * 2)) + (2 * 5) + 3
9 + (5 * (3 + 8 * 7) + 8 + 4 + 8 * (6 * 2 * 7))
9 + 6 + 6 * 3 * (2 + 7 + (5 + 7 + 4) + 3 * 2) + 5
7 + (5 * 5 + (4 * 5 + 7 * 4 + 3 + 3) * 4 + 7)
9 + 5 + 3 + ((7 + 7 + 2) + (6 + 9 + 7 + 4 * 5 + 7)) + 6 * 6
5 * 8 * 2 + ((6 + 6 * 3 + 6 * 8 * 6) + 6 * 3) + 3 * 2
(5 + 5 * 5 * (3 * 7 * 3 + 2) + 6 + 6) + 7 * 4 + (6 * (8 * 8 + 5) * 9 * 8 * 8 + 4) + 6
(7 + 3 + 6 + 2 * 8 + 5) * 6 * (2 * (3 + 7 + 9 + 6) + 5 + 2) + 8
((7 + 4) * 6 * 9) + 5
2 * 7 + 5 + 4 + 7
9 * (2 * 2 * 7 + 4 * (4 * 6 * 5 * 8))
5 * 9 + 8
6 * 2 * 8 + (5 + 3 + (8 + 9 + 6))
3 * 5 + 9 * (4 + 2 + 9 + (5 * 3) * 3 + 6) * 5
4 * 8 + (3 + 9 * 6) * 7 * (7 * 3 * 9 + 8 + 6 + (9 * 9 + 7 + 4)) + 8
(2 * 9) * (6 * 8 * 6 + 5)
9 * 2 * 3
(6 + 2 * 9 + 5) * 6
(8 + 3 + 2 * (2 + 8 * 2 + 6 * 2 + 2)) * 2
(4 + 8 * 6) * 3 * 6 + 4 * (3 * 2 * 5 + 9 + 9) + 8
3 + (9 * (8 + 6 + 8 * 7) * 8 * 7 + 7) * 7 + 8 + (2 + 3 + 6 * 6 + (2 + 4 + 6 + 5 * 3)) * 5
((5 + 2) + (7 * 4 * 9 + 3) * (4 + 5 * 8 + 6 * 6) + (9 + 2) * 4) + 6 + 9 * 3 * 4 * 5
2 + (7 * 5) + 4 + 3 + (3 * (7 + 2 * 6 * 7 + 6)) + 4
2 + 6 + (3 + 2 + (2 + 3) + 7 * 4 * (8 * 7 * 2 * 4 + 8 * 5))
4 * 3 + 6 + 5 * (9 + (2 * 4 * 8 * 8 + 8) + 2 + 4) + 9
4 * 6 + (9 * 8 * 9 * 5 * 9 + 7) + 7
(4 + (9 + 5 + 2 + 4 * 4 + 6) * 5 + 8) + (2 + 8 + (6 + 5 * 8 * 7) + (7 * 5)) + 5
3 + 2 + (6 * 9 + 9 * 2) * (7 * (5 * 8) + 9 + 6 * (7 + 7 * 9 + 2 + 4) * 7) * 9
5 + 9 * 7 * (8 * (6 + 4 * 5 + 3 * 2 * 4) + (5 + 2) * (6 * 3 * 7 * 6 + 8 + 3))
5 * 7 * 2 + 7 * 6 + (2 * 7 + 5 * 5 * (6 + 7 + 5 + 7))
4 + 7 * 2
9 + ((6 * 7) + (9 + 7 + 7)) * 4
5 + 6 * (3 * (8 * 8 * 3 + 4 * 5) + 4 + (2 + 7 + 3))
(2 + 7 + 6) + 5 + 7 * 6 * 8 * 9
2 * (6 + 9) * 8 * (9 + 4) + 7 + 8
5 + 3 * 6 * 9 + (4 * 8 + 3 * 2)
9 * (2 + 5 * 5) * 9 + (8 + 2 * (5 + 2 * 7 + 7 + 4 * 9) + 3 * 6 + 9) + 8
8 + 4 + (2 + (9 * 2) * 5 * (5 * 8)) * 8 * 7
5 * (3 * 5 + 6 * 4) + (7 * 5 * (6 + 6 * 7 * 8 * 6) + 8)
5 + (4 * 6) + 6 * 5 + 6
7 * 9 + ((6 + 3 + 6 * 4 * 4 * 6) + 7 + (6 + 4 * 7 * 3 + 9 * 6) + 4 + 9) + 6
(8 * 6) + (3 * (3 + 2 * 7 * 9 + 9 + 9) * 3 + (2 + 8 * 5))
3 * 4 * 6 * 4 * ((2 + 3 * 6) * 9)
5 * ((8 * 5 + 7) + 4 + 7 + 6 * (9 * 6 * 3 + 9 * 7 + 2) + (4 + 4)) + 8 * (7 * (2 * 8 * 6) + 8 + 9 + 6) + 7
2 * 8 * 5 * ((9 * 4 * 9 + 5 + 2) + (3 * 9) + (6 + 9 * 4 * 9 * 2) * 5 + 5) * 9
5 + (7 * 8 + 7) + 2 * 5
4 * (7 + 4 + 5 + (6 * 2 + 3)) * (7 * 3 * 7 * 8 * 7) * 2 * 2 + 2
(4 + (5 + 5 * 8) + (5 * 5 + 3 * 5) + 6) + 6
6 + (8 + 6 * 4 + 8 + (7 * 6) * 6) + 2 + 3
(6 + 3) + ((8 * 7 * 6 * 6 + 6 * 7) * 4 * 6 * (3 + 3 * 2)) + 6
4 * 2 * 3 * 3 * 7 + 9
3 * 4 * 8 * (6 * 6 * 3 + (3 * 9 * 3 * 3 + 6)) + 5 + 4
((7 * 2 + 7) + 9) * 2 * 8 * 3 + 3 * 3
2 * 3 + ((2 * 5) + 8 * 7 * 5 + (8 * 2 * 4) * 8) * 3 + 7 * 8
9 + (4 + 6) * 3
(9 + 5) * 3 * 2 * 7 * (6 * 4 * 3)
(3 * 3 * (3 * 3 * 5)) * 7 * 8 + (6 * 7 + 4) * 2 * 2
(2 + 4 * (9 + 9 * 7 * 9) + 4) + 4 + 3
(9 + 4 + 3) * 9 * 3 + 7 * 3
6 + 4 + (6 + 2 + 4) + (9 + 7 * 8)
7 + 9 + 5 * (3 + 9 * 7 + (5 + 8 + 9 * 4 + 6) * 5 * 8) + 9 * 6
(3 * 7 * 7 * 3 + 7) + 4 * 3 + 5
(8 + 7 * 2 + 5) * 4 + 7 * 2 + 5 + 3
9 * 5 * ((5 * 8 + 2) * 7) + 2
8 * 4 * 9 + 2
4 + (3 + 5 * 7 * 3 * 8 + 5) * 6 + (3 + 2 + 9) * 5
(3 * 5 + 8 * 8 * (7 * 6 + 2 * 6 + 9 * 7) * 9) * 7 * 9 + 9 + 3
5 * 9 + 6
6 * (8 + 3 + 4)
2 + 5 + (4 + 5 + 8 * (8 + 6 * 9 * 8 + 3) * (3 + 9 * 3) + 3) + 7 + 2
9 * (6 + 4) * 9 * 4 * 2 * ((4 + 3 + 2 * 9) + 6 + 5 + 8 * 7 + (7 * 7 + 6 + 2 + 8))
(7 * 2 * (4 + 2 + 7 * 5 * 6 * 5) * (3 * 4 + 2) + 6) + 9 + 5 + 3
4 * 5 * ((5 + 9 * 7 + 2) + 4 * 7 + 9) + 9 + 7
7 * 2 * 6 + 9 + 5 * 6
3 + 3 * 7 * ((3 * 3 + 2 + 7 * 5 + 2) + 7 * 6 * 2 * 5 * 9) + (4 * 5) + 7
9 + ((5 + 5 * 7) + 7 * 2) + 7 + 6
4 * 9 * 9 + ((9 + 3 + 7 * 4) * 4 + 2 + 9 + 8 + 6) * 6
(6 * 2 * 5 + 4 + 5) + (8 + (8 * 5 + 7 * 7 + 6 * 2) * 6 + 6) + 4 * 8
(5 + 6 * 2) * 5 * 6 * (5 * 5) + 9
(7 + (9 * 4)) * ((4 + 3 + 5) + 7 * 5)
2 * 5 + ((9 * 6 * 3 * 6 + 4) * 9 * 4) * 2 * 3 * (8 + 6)
3 * 6 * 5 + 9 * (6 + 4 * 2 + 5 * 6)
((7 + 8) + 3 + 7 + (8 * 3 * 6 + 8) * 2) * ((3 * 5 * 9) + 5) * 8 + 8
((3 * 4 + 5 + 2) + 6 * 6) * 9
4 + 6 * (9 * 6 * 7 + (2 * 5 + 5 * 9 + 6)) * 8
4 * (2 + 2 + 9) + 5 + 5 * ((5 + 3) + 4 * 7)
6 + 8 * 3 + 4
2 + 8 + (8 + (7 + 5 * 3 * 8 * 7 + 2) * 6) + 3";
        private const string ProblemTestInput = @"";
    }
}