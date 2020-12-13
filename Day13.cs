namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day13
    {//8:24 8:36
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
                .Select(x => new { bus = Int64.Parse(x.value), offset = x.index })
                .ToList();
            
            // long startTime = 100000000000000;
            long startTime = 110596099559000;
            // long startTime = 0;

            var maxBus = schedules.Select(s => s.bus).Max();
            var maxBusOffset = schedules.Where(s => s.bus == maxBus).Single().offset;
            var time = startTime + maxBus - (startTime % maxBus) - maxBusOffset;
            while (true)
            {
                if ((time + maxBusOffset) % (10000000 * maxBus) == 0)
                    Console.WriteLine(time);
                if (schedules.All(s => 
                    (time + s.offset) % s.bus == 0
                ))
                    return time;

                time += maxBus;
            }
        }

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
    }
}