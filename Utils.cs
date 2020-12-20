namespace advent_of_code_2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    public static class Utils
    {
        public static IEnumerable<string> SplitToLines(this string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (System.IO.StringReader reader = new System.IO.StringReader(input))
            {
                string line;
                while( (line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static char[][] ToJaggedCharArray(this IEnumerable<string> strings)
        {
            return strings.Select(s => s.ToArray()).ToArray();
        }

        public static char[,] ToRectangularCharArray(this IEnumerable<string> strings)
        {
            var jagged = strings.ToJaggedCharArray();

            var grid = new char[ jagged[0].Length, jagged.Length ];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x,y] = jagged[y][x];
                }
            }

            return grid;
        }

        public static bool ContainsPoint<T>(this T[,] grid, int x, int y)
        {
            return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
        }

        public static IEnumerable<T> ElementsWhere<T>(this T[,] grid, Func<T, bool> test)
        {
            var matches = new List<T>();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (test(grid[x,y]))
                    {
                        matches.Add(grid[x,y]);
                    }
                }
            }
            return matches;
        }

        public static void ForAllElements<T>(this T[,] grid, Action<int, int> visitor)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    visitor(x, y);
                }
            }
        }

        public static bool ElementsMatch<T>(this T[,] grid, T[,] otherGrid)
        {
            return grid.Rank == otherGrid.Rank &&
                Enumerable.Range(0, grid.Rank).All(dimension => grid.GetLength(dimension) == otherGrid.GetLength(dimension)) &&
                grid.Cast<T>().SequenceEqual(otherGrid.Cast<T>());
        }

        public static T[] GetRow<T>(this T[,] grid, int rowNumber)
        {
            return Enumerable.Range(0, grid.GetLength(0))
                    .Select(x => grid[x, rowNumber])
                    .ToArray();
        }

        public static T[] GetColumn<T>(this T[,] grid, int columnNumber)
        {
            return Enumerable.Range(0, grid.GetLength(1))
                    .Select(x => grid[columnNumber, x])
                    .ToArray();
        }

        public static T[,] RotateClockwise<T>(this T[,] grid)
        {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            var ret = new T[height, width];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    ret[x,y] = grid[y,height-x-1];
                }
            }

            return ret;
        }

        public static T[,] FlipHorizontally<T>(this T[,] grid)
        {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            var ret = new T[width, height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    ret[x,y] = grid[width-x-1,y];
                }
            }

            return ret;
        }

        public static void DebugPrint(this char[,] grid)
        {
            var sb = new StringBuilder();
            
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    sb.Append(grid[x,y]);
                }
                sb.Append(Environment.NewLine);
            }

            var gridString = sb.ToString();

            Debug.WriteLine(gridString);
        }

        public static int[][] EightDirections = new int[][] {
            new [] { 1, 1 },
            new [] { 1, 0 },
            new [] { 1, -1},
            new [] { 0, 1 },
            new [] { 0, -1},
            new [] { -1, 1 },
            new [] { -1, 0 },
            new [] { -1, -1}
        };

        public static int[][] FourDirections = new int[][] {
            new [] { 1, 0 },
            new [] { 0, 1 },
            new [] { 0, -1},
            new [] { -1, 0 }
        };

        public static void ProcessEightNeightbours<T>(this T[,] grid, int x, int y, Action<int, int> process)
        {
            foreach (int[] direction in EightDirections)
            {
                if (!grid.ContainsPoint(x + direction[0], y + direction[1]))
                    continue;
                process(x + direction[0], y + direction[1]);
            }
        }

        public static long SetBit(this long current, int position, bool value)
        {
            return value ?
                current | (1L << position) :
                current & ~(1L << position);
        }

        public static int SetBit(this int current, int position, bool value)
        {
            return value ?
                current | (1 << position) :
                current & ~(1 << position);
        }

        public static bool ReadBit(this long current, int position)
        {
            return (current & (1L << position)) != 0;
        }

        public static bool ReadBit(this int current, int position)
        {
            return (current & (1 << position)) != 0;
        }
    }
}