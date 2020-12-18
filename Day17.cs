namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day17
    {
        public static int SolveProblem1()
        {
            var lines = ProblemInput.SplitToLines();
            var liveCells = lines
                .SelectMany((l, i1) => l.Select((c, i2) => c == '.' ? null : new Tuple<int, int, int>(i2, i1, 0)))
                .Where(x => x != null)
                .ToHashSet();

            var cycles = 6;

            for (int i = 0; i < cycles; i++)
            {
                liveCells = step(liveCells);
            }
            return liveCells.Count;
        }

        private static HashSet<Tuple<int, int, int>> step(HashSet<Tuple<int, int, int>> current)
        {
            var proccessedPoints = new HashSet<Tuple<int, int, int>>();
            var newLive = new HashSet<Tuple<int, int, int>>();

            foreach(var currentLive in current)
            {
                foreach(var direction in DirectionsPlusPoint)
                {
                    var candidate = new Tuple<int, int, int>(
                        currentLive.Item1 + direction[0],
                        currentLive.Item2 + direction[1],
                        currentLive.Item3 + direction[2]);

                    if (!proccessedPoints.Contains(candidate))
                    {
                        var neighbours = 0;
                        foreach (var d in Directions)
                        {
                            var candidateNeighbour = new Tuple<int, int, int>(
                                candidate.Item1 + d[0],
                                candidate.Item2 + d[1],
                                candidate.Item3 + d[2]);
                            
                            if (current.Contains(candidateNeighbour))
                                neighbours++;
                        }

                        if (current.Contains(candidate) && (neighbours == 2 || neighbours == 3))
                        {
                            newLive.Add(candidate);
                        }
                        else if (!current.Contains(candidate) && neighbours == 3)
                        {
                            newLive.Add(candidate);
                        }

                        proccessedPoints.Add(candidate);
                    }
                }
                
            }

            return newLive;
        }

        private static int[][] Directions = new int[][] {
            new [] { 1, 1, 1 },
            new [] { 1, 0, 1 },
            new [] { 1, -1, 1 },
            new [] { 0, 1, 1 },
            new [] { 0, 0, 1 },
            new [] { 0, -1, 1 },
            new [] { -1, 1, 1 },
            new [] { -1, 0, 1 },
            new [] { -1, -1, 1 },
            
            new [] { 1, 1, 0 },
            new [] { 1, 0, 0 },
            new [] { 1, -1, 0 },
            new [] { 0, 1, 0 },
            new [] { 0, -1, 0 },
            new [] { -1, 1, 0 },
            new [] { -1, 0, 0 },
            new [] { -1, -1, 0 },

            new [] { 1, 1, -1 },
            new [] { 1, 0, -1 },
            new [] { 1, -1, -1 },
            new [] { 0, 1, -1 },
            new [] { 0, 0, -1 },
            new [] { 0, -1, -1 },
            new [] { -1, 1, -1 },
            new [] { -1, 0, -1 },
            new [] { -1, -1, -1 }
        };

        private static int[][] DirectionsPlusPoint = new int[][] {
            new [] { 1, 1, 1 },
            new [] { 1, 0, 1 },
            new [] { 1, -1, 1 },
            new [] { 0, 1, 1 },
            new [] { 0, 0, 1 },
            new [] { 0, -1, 1 },
            new [] { -1, 1, 1 },
            new [] { -1, 0, 1 },
            new [] { -1, -1, 1 },
            
            new [] { 1, 1, 0 },
            new [] { 1, 0, 0 },
            new [] { 1, -1, 0 },
            new [] { 0, 1, 0 },
            new [] { 0, 0, 0 },
            new [] { 0, -1, 0 },
            new [] { -1, 1, 0 },
            new [] { -1, 0, 0 },
            new [] { -1, -1, 0 },

            new [] { 1, 1, -1 },
            new [] { 1, 0, -1 },
            new [] { 1, -1, -1 },
            new [] { 0, 1, -1 },
            new [] { 0, 0, -1 },
            new [] { 0, -1, -1 },
            new [] { -1, 1, -1 },
            new [] { -1, 0, -1 },
            new [] { -1, -1, -1 }
        };

        public static int SolveProblem2()
        {
            var lines = ProblemInput.SplitToLines();
            var liveCells = lines
                .SelectMany((l, i1) => l.Select((c, i2) => c == '.' ? null : new Tuple<int, int, int, int>(i2, i1, 0, 0)))
                .Where(x => x != null)
                .ToHashSet();

            var cycles = 6;

            for (int i = 0; i < cycles; i++)
            {
                liveCells = step2(liveCells);
            }
            return liveCells.Count;
        }

        private static HashSet<Tuple<int, int, int, int>> step2(HashSet<Tuple<int, int, int, int>> current)
        {
            var proccessedPoints = new HashSet<Tuple<int, int, int,int>>();
            var newLive = new HashSet<Tuple<int, int, int,int>>();

            foreach(var currentLive in current)
            {
                foreach (var w1 in Enumerable.Range(-1, 3))
                {
                    foreach(var direction in DirectionsPlusPoint)
                    {
                        var candidate = new Tuple<int, int, int, int>(
                            currentLive.Item1 + direction[0],
                            currentLive.Item2 + direction[1],
                            currentLive.Item3 + direction[2],
                            currentLive.Item4 + w1);

                        if (!proccessedPoints.Contains(candidate))
                        {
                            var neighbours = 0;
                            foreach (var w2 in Enumerable.Range(-1, 3))
                            {
                                foreach (var d in DirectionsPlusPoint)
                                {
                                    if (w2 == 0 && d[0] == 0 && d[1] == 0 && d[2] == 0)
                                        continue;
                                    var candidateNeighbour = new Tuple<int, int, int, int>(
                                        candidate.Item1 + d[0],
                                        candidate.Item2 + d[1],
                                        candidate.Item3 + d[2],
                                        candidate.Item4 + w2);
                                    
                                    if (current.Contains(candidateNeighbour))
                                        neighbours++;
                                }
                            }

                            if (current.Contains(candidate) && (neighbours == 2 || neighbours == 3))
                            {
                                newLive.Add(candidate);
                            }
                            else if (!current.Contains(candidate) && neighbours == 3)
                            {
                                newLive.Add(candidate);
                            }

                            proccessedPoints.Add(candidate);
                        }
                    }
                }
            }

            return newLive;
        }

        private const string ProblemInput = @"......##
####.#..
.##....#
.##.#..#
........
.#.#.###
#.##....
####.#..";
        private const string ProblemTestInput = @".#.
..#
###";
    }
}