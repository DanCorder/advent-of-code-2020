namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day11
    {
        public static int SolveProblem1()
        {
            var lines = ProblemInput.SplitToLines().ToArray();
            var borderRow = new char[lines.First().Length + 2];
            borderRow = borderRow.Select(c => '.').ToArray();
            var grid = new char[lines.First().Length + 2, lines.Length + 2];
            var newGrid = new char[lines.First().Length + 2, lines.Length + 2];

            for (var x = 0; x < lines.First().Length; x++)
            {
                for (var y = 0; y < lines.Length; y++)
                {
                    grid[x+1,y+1] = lines[y][x];
                }
            }
            for (var x = 0; x < lines.First().Length; x++)
            {
                grid[x,0] = '.';
                grid[x,lines.Length+1] = '.';
            }
            for (var y = 0; y < lines.Length; y++)
            {
                grid[0,y] = '.';
                grid[lines.First().Length+1,y] = '.';
            }
            
            while(!areSame(newGrid, grid))
            {
                step(grid, newGrid);
                var tmp = grid;
                grid = newGrid;
                newGrid = tmp;
            }

            var count = 0;
            for (var x = 0; x < lines.First().Length; x++)
            {
                for (var y = 0; y < lines.Length; y++)
                {
                    if (grid[x+1,y+1] == '#')
                        count++;
                }
            }
            return count;
        }

        private static void step(char[,] grid, char[,] newGrid)
        {
            for (var x = 1; x < grid.GetLength(0) - 1; x++)
            {
                for (var y = 1; y < grid.GetLength(1) - 1; y++)
                {
                    newGrid[x,y] = process(grid, x, y);
                }
            }
        }

        private static void step2(char[,] grid, char[,] newGrid)
        {
            for (var x = 1; x < grid.GetLength(0) - 1; x++)
            {
                for (var y = 1; y < grid.GetLength(1) - 1; y++)
                {
                    newGrid[x,y] = process2(grid, x, y);
                }
            }
        }

        private static bool areSame(char[,] grid, char[,] newGrid)
        {
            for (var x = 1; x < grid.GetLength(0) - 1; x++)
            {
                for (var y = 1; y < grid.GetLength(1) - 1; y++)
                {
                    if (newGrid[x,y] != grid[x,y])
                        return false;
                }
            }
            return true;
        }

        private static char process(char[,] grid, int x, int y)
        {
            if (grid[x,y] == 'L')
            {
                for (var i = -1; i < 2; i++)
                {
                    for (var j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        if (grid[x+i,y+j] == '#')
                            return grid[x,y];
                    }
                }
                return '#';
            }
            else if (grid[x,y] == '#')
            {
                var occupied = 0;
                for (var i = -1; i < 2; i++)
                {
                    for (var j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        if (grid[x+i,y+j] == '#')
                            occupied++;

                        if (occupied >= 4)
                        {
                            return 'L';
                        }
                    }
                }
                return grid[x,y];
            }
            return grid[x,y];
        }

        private static char process2(char[,] grid, int x, int y)
        {
            if (grid[x,y] == 'L')
            {
                for (var i = -1; i < 2; i++)
                {
                    for (var j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        if (firstSeat(grid, x, y, i, j) == '#')
                            return grid[x,y];
                    }
                }
                return '#';
            }
            else if (grid[x,y] == '#')
            {
                var occupied = 0;
                for (var i = -1; i < 2; i++)
                {
                    for (var j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        if (firstSeat(grid, x, y, i, j) == '#')
                            occupied++;

                        if (occupied >= 5)
                        {
                            return 'L';
                        }
                    }
                }
                return grid[x,y];
            }
            return grid[x,y];
        }

        private static char firstSeat(char[,] grid, int x, int y, int xdiff, int ydiff)
        {
            var xPos = x + xdiff;
            var yPos = y + ydiff;

            while(xPos > 0 && xPos < grid.GetLength(0) && yPos > 0 && yPos < grid.GetLength(1))
            {
                if (grid[xPos,yPos] == '#' || grid[xPos,yPos] == 'L')
                    return grid[xPos,yPos];
                
                xPos += xdiff;
                yPos += ydiff;
            }
            return '.';
        }

        public static int SolveProblem2()
        {
            var lines = ProblemInput.SplitToLines().ToArray();
            var borderRow = new char[lines.First().Length + 2];
            borderRow = borderRow.Select(c => '.').ToArray();
            var grid = new char[lines.First().Length + 2, lines.Length + 2];
            var newGrid = new char[lines.First().Length + 2, lines.Length + 2];

            for (var x = 0; x < lines.First().Length; x++)
            {
                for (var y = 0; y < lines.Length; y++)
                {
                    grid[x+1,y+1] = lines[y][x];
                }
            }
            for (var x = 0; x < lines.First().Length; x++)
            {
                grid[x,0] = '.';
                grid[x,lines.Length+1] = '.';
            }
            for (var y = 0; y < lines.Length; y++)
            {
                grid[0,y] = '.';
                grid[lines.First().Length+1,y] = '.';
            }
            
            while(!areSame(newGrid, grid))
            {
                step2(grid, newGrid);
                var tmp = grid;
                grid = newGrid;
                newGrid = tmp;
            }

            var count = 0;
            for (var x = 0; x < lines.First().Length; x++)
            {
                for (var y = 0; y < lines.Length; y++)
                {
                    if (grid[x+1,y+1] == '#')
                        count++;
                }
            }
            return count;
        }

        private const string ProblemInput = @"LLLLL.LLLLLLLLL.LLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL
LLLLL.LL.L.LLLL.LLL..LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLLLLLLLLLLLLLLLLLL.L..LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.L.LLLLLLLLLL
LLLLLLLLLL.LLLL.LLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL
LLLLLLLLLLLLLLL.LLLL.LLLLL.LLLLLLL.LL.LLLLLL..LLLLLLLLLLLLL.LL.LLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLL
LLLLLLLLLL.LLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLL
LLLLLL.LLL.LLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLL.LL.LLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLL
.LL..LL.L..L.L...LL..L..L.L...LL..L...LL..LLLL.L.L.L.....L....L.LL...L.LL.LL....L...L..L....L....L
LLLLL.LLLL.LLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLL.LLLL.LLLLLLLLLLLLLLL
LLLLLLLLLL.LLLL.LLLL.LLL.L.LLLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLLL.LLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLL
LLLLL.LLLL.LLLL.LLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL...LLLLLLLLLL.LLLLLLL
LLLLLL.LLLLLLLL.LLLL.LLLLLLLL.LLLL.LLLLLLLLL..LLLL.L.LLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLLL.LLL.LL
L.L...L.....LLL.L..LL...L......L.LLL........LL..LLL..L.LLL....L..L.L.L..L..L.LL..L.......LL..L.L..
LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
LLLLLLLLL.LLLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LL.LLLL.LLLLLLLLLL.LLLLLLLLL
LLLLLLLLLL.LLLL.LLLL.LLLL.LLLLLLL.LLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLL.L.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLLLLLLL.LLLL.LLLLL.LLLLL.LLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL..LLLLLL.LLLL.LLLLLLL
LLLLL.LLLL.LLLL.LLLL.LLLLLLLLL.LLLLLLLLLLLLL.LLLLLLL.LLLL.LLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLL
..L..L.L........LL.L.L..LLL........L....LL..LLL.LL....L.......LL.LLLL....L...L.LL........L.L......
LLLLL.L.LL.LLLLLLLLL.LLLLL.LLLLLLLLL.LLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.L.LLLLLLLL.L
LLLLL.LLLL.LLLL..LLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL
.LLLLLLLLL.LLLL.LLLL..LLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLL.L.L..LLLL.LLLLLLLLLLLLLLLLLLLL
LLLLL.LLLL.LLLL.LLLL.LLLLLLL.LLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLL.LLLLL
.....LL.L.LL...L..LL.LL..L...L..L.LL..LL..L.L..........L....L...L.L....L.L.L....LL.........LL..LL.
LLLLL.LLLL.LLLL.LLLLLLLLLLLLLLLLLL.LLL.LLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.L.L.LLL.LLLLLLLLLLLL
LLLLLLLLLL.LLLL.LLLL.LLLLL.LLLLLLLLLLLLLLL.LLLL.LLLL.LLLLLLL.LLLLLLLL.LLL.LLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLLLL.LL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LL.LLLLL..LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLL.LL
LLLLLLLL.LLLLLL.LLLL.LLLLL.LLLLLLL.LLLLLL.LL.LLLLLLLLLLLLLLL.L..LLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLL.LLLLLLLLLLL.LLLLLLLLLLLL
L..LL.L.......L.LL..L.....L..L.L..L.L...L....LLL..........LLL.LL......L.L..L.LL..L....LL..L.......
LLLLL.LLLL.LLLL.LLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL
LLLLL.LL.L.LLL..LLLL.LLLLL.LLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLL.
LLLLL.LLLL.LLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLL.L.LLLLLLLLLL
LLLLL.LLLL.LLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLL.LL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL
LLLLL.LLLL.L.LL.LLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLL.LLL..LLLLL.LLLLLL..LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.L.LLLLL.LLLLLLLLLLLLLLLLLLLL
LLLLLLLLLL.LLLL.LLLLLLLLLL.LLLL.L..LLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLL.LLLL.LLLLLLLLLLLLL.LL.LLLLLL.L.LLLLL.LLLLLLLLLLLLLLL..LLLLLLL.LLLLLLLLLLLLLLLLLLLL
...LLLLL..L...L..L..L.............L..L...L..L.L.L..LLL.L.....L.L.L.L.L..L.LL.L..L.L..LLL..LL...LLL
LLLLL.LLLL.LLLL.LLLL.LLLLL.LLL.LLL.LLLLLLLLL.LL.LLLL.LLLL.LLLLLLLLLLL.LLLLLL..LLLLLLL.LLLLLLLLLLLL
LLLLLLLLLL.L.LL.LLLLLL.LLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL..L.LLLL.LLLL.LL.LLLLLLL.LLLLLLLLLLLL
.LLLLLLLLL.LLLL.LLLL.LLLLL.LL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LL.LLLLLLLLL
LLLLL.LLLL.LLLL.LLLL.LLLLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLL
LLLL..LLLLLLLLL.LLLL.LL.LL..LLLLLL.LLLLLLL.L.LLLLLLL.LLL.LLLLL.LLLLLLLLL.LLLLLLLLLLLL.LLLL.LLLLLLL
LLLLLL.LLL.LLLL.LLLL.LLLLL.LLL...LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLL.LL.LLL.LLLLL
LLLLLLLLLLLLLLLLL.LL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLLLLLLL..LLL.LLLL.LLLLL.LLLLLLL.LL.LLLLLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLL
.LL..L....L..LL....L.......L........LL......L...L..............L.L...L.L....LLLL.....L......LLL..L
LLLLL.LLLL.LLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLL.LLL..LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL
LLLLL.LLLLLLLLL.LLLLLLLLLL.LL.LLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLL.LLLLLLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLLLLLLLLLLLLL.LLLLLLL.LL.LLLLLL.LLLLLL.LLLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLL.LLLLLLL.LLLL
LLLLL.LLLL.LLLL.LLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLLLLLLL.LLLL.LLLLL.LLLLLL..LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLL
...L.L.L.LLL..L......LLL.L..L......L...LLL....L....L..LL.L.L..L..L......L.....LL.L.....L........L.
LL.LLLLL.L.LLLL.LLLL.LLLLL.LLL.LLL.LLLLLLLLL.LLLLLLL.LLLLLLLL..LLLLLL.LLLLLLL.LLLLLLL.L.LLLLLLLLLL
LLLLL.LLLL.LLLLLLLLLLLLLLL.LLLLLLL.L.LLLLLL..LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL
LLLLL.LLL..LLLLLLLLL.L.LLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLL
LLLLL.LLL..LLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLL.LL..LLLLLLL.L.LLL.LL.LLLLL.LLLLLLLLLLLLLLLLLLLLLL
LLLLL.LLLL.LLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLL.LLLLLLL.LLLLLLL.L.LLLLLLLLLL
LLLLLLLLLLLLLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL.LL.
LLLLL.LLLL.LLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LL
LLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLL..LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLL
...L...L...L...LL...LL.L.L..LL..L....LL...LLLL.L........L......L..L....L.L....L...L.L.L.LLL.L..L.L
LLLLL.L.LL.LLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLL.LL.LL.LLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLLLLLLL.LLLLL.LLLL.LL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL
LLLLLLLLLL.LLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLL
LLLLL.LLLL.LLLL.LLLLLLLLLLLLLL.LLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLL.LLLLLLLL.LLL
LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLLL.L.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
LLLLL.LLLLLLLLL.LL.L.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLL..LLLLLLLLLLLL
LL.L...LL..LL..L..L.L....L...L..L..LL......LL.L.......L..LL........LL.LLLLL.......L.....LL.LL.....
LLLLL.LLLLLLL.L.LLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL..LLLLLL.LLLLLLLLLLLL.LL.LLLLLLLLLLLL
LLLLL.LLLLLLL.L.LLLL.LLLLL.LLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLLLLLLLL.LLLL.L.LLLLL.LLLLLLLLL.LLLLLLL.LLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLL
LLL.L.LLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLL.LL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLL.LLLLLLLL.LLL
..L.L....LL.LL.....L.LL......L.LL..LLLLLL......LL.L.LL.LLL...LLLL......L.L.L...LL..L..L..L...LL..L
LLLLL.LLLLLLLLL.LLL..LLLLL.LLLLLLL.LLLLLLLLL.LLLLLLL..LLLLLLLL.LLLLLLLLL.LLLL.LLLLLLL.LLLLLLLL.LLL
LLLL..LLLL.LLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL
LLL.L.LLLL.LLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLL.L.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLLLLLLL.LL
L.LL..LLLL.L.LLLLLLLLLLLLL.LLLLLLL.LLLLLLLL..LLL.LLL.LLLLLL.LL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LL.L.LLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLL..LLLLLLL.LLLLLLL.LLLLLLLLLLLL
L....L...L..L.L...L....L......L..LLL..L..LL.LL.L.L....L..L.L.LLL.L.LLLL.........L..LL.LLL......L..
LLLLL.LLLL.LLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLL.L
LLLLL.LLL..LLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLLLLLLL.LL
LLLLL.LLLLLL.LL.LLLL.LLLLL.LLLLLLL.L.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLL.LL.LLLLLLLLLLLL.LLLLLLLLLLLL
LLLLL.LLLL.LLLL.LLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL
LLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLL";
        private const string ProblemTestInput = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";
    }
}