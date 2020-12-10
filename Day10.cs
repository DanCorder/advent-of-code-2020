namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day10
    {
        public static int SolveProblem1()
        {
            var lines = ProblemInput.SplitToLines().Select(Int32.Parse).OrderBy(n => n);

            var jolts = 0;
            var diff1 = 0;
            var diff3 = 1;

            foreach(var output in lines)
            {
                var diff = output - jolts;
                if (diff == 1)
                    diff1++;
                if (diff == 3)
                    diff3++;
                jolts = output;
            }

            return diff1 * diff3;
        }

        public static long SolveProblem2()
        {
            var adapters = ProblemInput.SplitToLines().Select(Int32.Parse).OrderBy(n => n).ToList();
            var max = adapters.Last();
            adapters = (new List<int>() {0}).Concat(adapters).Concat(new List<int>() {max+3}).ToList();

            var bigGaps = new List<int>();
            for (var i = 1; i < adapters.Count; i++)
            {
                if (adapters[i] - adapters[i-1] == 3)
                {
                    bigGaps.Add(i);
                }
            }
            var splitAdapters = new List<List<int>>();
            var lastSplit = 0;
            for (var i = 0; i < bigGaps.Count; i++)
            {
                splitAdapters.Add(adapters.Skip(lastSplit).Take(bigGaps[i]-lastSplit).ToList());
                lastSplit = bigGaps[i];
            }
            return splitAdapters
                .Select(l => countCombinations(l, 0))
                .Aggregate(1L, (a,b) => a*b);
        }

        private static long countCombinations(List<int> adapters, int index)
        {
            var count = 1L;
            while (index < adapters.Count - 2)
            {
                var gap = adapters[index + 2] - adapters[index];
                if (gap <= 3)
                {
                    var newAdapters = adapters.Take(index+1).Concat(adapters.Skip(index+2)).ToList();
                    count += countCombinations(newAdapters, index);
                }
                index++;
            }
            return count;
        }

        private const string ProblemInput = @"71
30
134
33
51
115
122
38
61
103
21
12
44
129
29
89
54
83
96
91
133
102
99
52
144
82
22
68
7
15
93
125
14
92
1
146
67
132
114
59
72
107
34
119
136
60
20
53
8
46
55
26
126
77
65
78
13
108
142
27
75
110
90
35
143
86
116
79
48
113
101
2
123
58
19
76
16
66
135
64
28
9
6
100
124
47
109
23
139
145
5
45
106
41";
        private const string ProblemTestInput = @"16
10
15
5
1
11
7
19
6
12
4";


        private const string ProblemTestInput2 = @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";
    }
}