namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day13
    {
        public static long SolveProblem1()
        {
            var lines = ProblemInput.SplitToLines().ToList();
            var startTime = Int64.Parse(lines[0]);
            var schedules = lines[1].Split(',').Where(b => b != "x").Select(Int64.Parse).OrderBy(x => x).ToList();
            var waits = schedules.Select(s => new { wait = s - (startTime % s), s }).ToList();
            var answer = waits.OrderBy(w => w.wait).First();
            return answer.wait * answer.s;
        }

        public static long SolveProblem2()
        {
            var lines = ProblemInput.SplitToLines().ToList();
            var schedules = lines[1].Split(',')
                .Select((x, i) => new { value = x, index = i })
                .Where(x => x.value != "x")
                .Select(x => new Tuple<long, long>(Int64.Parse(x.value), x.index )) // bus, offset
                .OrderBy(x => x.Item1)
                .ToList();
            
            var accumulatedSchedule = schedules[0];

            for (int i = 1; i < schedules.Count; i++)
            {
                accumulatedSchedule = CombineSchedules(accumulatedSchedule, schedules[i]);
            }

            return accumulatedSchedule.Item1 - accumulatedSchedule.Item2;
        }

        private static Tuple<long, long> CombineSchedules(Tuple<long, long> r1, Tuple<long, long> r2)
        {
            // b1 = 5, o1 = 2
            // b2 = 11, o2 = 4
            // 3, 8, 13, 18, 23, 28, 33, 73
            // 7, 18, 29, 40, 51, 62, 73, 84, 95, 106, 117, 128 :: 18, 73, 128
            // (b1 * x) - o1 = (b2 * y) - o2
            // ((b2 * y) - o2 + o1) % b1 == 0

            // b1 = 7, o1 = 0
            // b2 = 13, o2 = 1
            // 7, 14, 21, 28, 35, 42... 77, 84, 91... 154, 168
            // 12, 25, 38, 51, 64, 77, 90, 103, 116, 129, 142, 155, 168,
            // time matches: 77, 168

            // b1 = 13, o1 = 2
            // b2 = 17, o2 = 0
            // 11, 24, 37, 50, 63, 76, 89, 102, 115, 128, 141, 154, 167, 180, 193, 206, 219, 232, 245
            // 17, 34, 51, 68, 85, 102, 119, 136, 153, 170, 187, 204, 221, 238, 255
            // time matches: 102, 323 => bus 221, offset 221 - 102 = 119 
            var count = 1;
            var firstMatchTime = -1L;
            while (true)
            {
                var candidateTime = (r1.Item1 * count) - r1.Item2;
                if ((candidateTime + r2.Item2) % r2.Item1 == 0)
                {
                    if (firstMatchTime == -1)
                    {
                        firstMatchTime = candidateTime;
                    }
                    else
                    {
                        return new Tuple<long, long>(candidateTime - firstMatchTime, candidateTime - firstMatchTime - firstMatchTime);
                    }
                }
                count++;
            }
        }

        // Naive solution - would have taken a little under 2 days
        // public static long SolveProblem2()
        // {
        //     var lines = ProblemInput.SplitToLines().ToList();
        //     var schedules = lines[1].Split(',')
        //         .Select((x, i) => new { value = x, index = i })
        //         .Where(x => x.value != "x")
        //         .Select(x => new { bus = Int64.Parse(x.value), offset = x.index })
        //         .ToList();
            
        //     // long startTime = 100000000000000;
        //     long startTime = 1012171816100000;
        //     // long startTime = 0;

        //     var maxBus = schedules.Select(s => s.bus).Max();
        //     var maxBusOffset = schedules.Where(s => s.bus == maxBus).Single().offset;
        //     var time = startTime + maxBus - (startTime % maxBus) - maxBusOffset;
        //     while (true)
        //     {
        //         if ((time + maxBusOffset) % (10000000 * maxBus) == 0)
        //             Console.WriteLine(time);
        //         if (schedules.All(s => 
        //             (time + s.offset) % s.bus == 0
        //         ))
        //             return time;

        //         time += maxBus;
        //     }
        // }

        private const string ProblemInput = @"1009310
19,x,x,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x,599,x,29,x,x,x,x,x,x,x,x,x,x,x,x,x,x,17,x,x,x,x,x,23,x,x,x,x,x,x,x,761,x,x,x,x,x,x,x,x,x,41,x,x,13";
        private const string ProblemTestInput = @"939
7,13,x,x,59,x,31,19";

private const string ProblemTestInput2 = @"939
17,x,13,19";

private const string ProblemTestInput3 = @"939
67,7,59,61";

private const string ProblemTestInput4 = @"939
67,x,7,59,61";

private const string ProblemTestInput5 = @"939
67,7,x,59,61";

private const string ProblemTestInput6 = @"939
1789,37,47,1889";

private const string ProblemTestInput7 = @"939
x,x,5,x,11";
    }
}