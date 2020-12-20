namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day20
    {
        public static long SolveProblem1()
        {
            var tiles = ProblemInput.Split("\n\n").Select(t => new Tile(t)).ToList();

            var possibleCorners = tiles.Where(t => {
                var others = tiles.Except(new Tile[] { t });
                var matches = t.edges.Where(te => others.Any(o => o.edges.Any(oe => oe.Item1 == te.Item1 || oe.Item2 == te.Item1)));
                return matches.Count() == 2;
            });

            return possibleCorners.Aggregate(1L, (acc, t) => acc * t.id);
        }

        private class Tile
        {
            public long id;
            public HashSet<Tuple<string, string>> edges = new HashSet<Tuple<string, string>>();
            public Tile(string tile)
            {
                var lines = tile.SplitToLines();
                id = long.Parse(lines.First().Substring(5, 4));
                var image = lines.Skip(1).ToRectangularCharArray();
                var left = new string(lines.Skip(1).Select(l => l[0]).ToArray());
                var right = new string(lines.Skip(1).Select(l => l[9]).ToArray());
                edges.Add(new Tuple<string, string>(lines.Skip(1).First(), new string(lines.Skip(1).First().Reverse().ToArray())));
                edges.Add(new Tuple<string, string>(right, new string(right.Reverse().ToArray())));
                edges.Add(new Tuple<string, string>(lines.Last(), new string(lines.Last().Reverse().ToArray())));
                edges.Add(new Tuple<string, string>(left, new string(left.Reverse().ToArray())));
            }
        }

        private class Tile2
        {
            public long id;
            public char[,] grid;
            public List<string> clockwiseEdges;
            public List<string> anticlockwiseEdges;
            public Tile2(string tile)
            {
                var lines = tile.SplitToLines().ToList();
                grid = lines.Skip(1).ToRectangularCharArray();
                id = long.Parse(lines.First().Substring(5, 4));
                clockwiseEdges = new List<string>();
                clockwiseEdges.Add(new string(grid.GetRow(0)));
                clockwiseEdges.Add(new string(grid.GetColumn(9)));
                clockwiseEdges.Add(new string(grid.GetRow(9).Reverse().ToArray()));
                clockwiseEdges.Add(new string(grid.GetColumn(0).Reverse().ToArray()));
                anticlockwiseEdges = clockwiseEdges.Select(e => new string(e.Reverse().ToArray())).ToList();
            }
        }

        public static int SolveProblem2()
        {
            var tiles = ProblemInput.Split("\n\n").Select(t => new Tile2(t)).ToList();
            var tileSquareSideLength = (int)Math.Sqrt(tiles.Count);

            var possibleCorners = tiles.Where(t => {
                var others = tiles.Except(new Tile2[] { t });
                var matches = t.clockwiseEdges.Where(te => others.Any(o => o.clockwiseEdges.Contains(te) || o.anticlockwiseEdges.Contains(te)));
                return matches.Count() == 2;
            });

            var corner = possibleCorners.First();
            var others = tiles.Except(new Tile2[] { corner });
            var matchingEdges = corner.clockwiseEdges.Select(e => others.Any(t => t.clockwiseEdges.Contains(e) || t.anticlockwiseEdges.Contains(e))).ToList();
            var clockwiseRotations = 0;
            if (matchingEdges[0] && matchingEdges[1])
                clockwiseRotations = 1;
            if (matchingEdges[1] && matchingEdges[2])
                clockwiseRotations = 0;
            if (matchingEdges[2] && matchingEdges[3])
                clockwiseRotations = 3;
            if (matchingEdges[3] && matchingEdges[0])
                clockwiseRotations = 2;

            var tileArrangement = new Arrangement[tileSquareSideLength,tileSquareSideLength];

            tileArrangement[0,0] = new Arrangement(corner, false, clockwiseRotations);
            for (var x = 1; x < tileSquareSideLength; x++)
            {
                tileArrangement[x, 0] = FindRight(tileArrangement[x-1, 0], tiles);
            }

            for (var y = 1; y < tileSquareSideLength; y++)
            {
                for (var x = 0; x < tileSquareSideLength; x++)
                {
                    tileArrangement[x, y] = FindBelow(tileArrangement[x, y-1], tiles);
                }
            }
            
            // var pictureWithBorders = new char[tileSquareSideLength * 10, tileSquareSideLength * 10];
            // for (var y = 0; y < tileSquareSideLength; y++)
            // {
            //     for (var x = 0; x < tileSquareSideLength; x++)
            //     {
            //         var transformedTile = Transform(tileArrangement[x,y]);
            //         for (var y2 = 0; y2 < transformedTile.GetLength(1); y2++)
            //         {
            //             for (var x2 = 0; x2 < transformedTile.GetLength(0); x2++)
            //             {
            //                 pictureWithBorders[x*10 + x2, y*10 + y2] = transformedTile[x2,y2];
            //             }
            //         }
            //     }
            // }
            // pictureWithBorders.DebugPrint();

            var pictureWithoutBorders = new char[tileSquareSideLength * 8, tileSquareSideLength * 8];
            for (var y = 0; y < tileSquareSideLength; y++)
            {
                for (var x = 0; x < tileSquareSideLength; x++)
                {
                    var transformedTile = Transform(tileArrangement[x,y]);
                    for (var y2 = 0; y2 < transformedTile.GetLength(1) - 2; y2++)
                    {
                        for (var x2 = 0; x2 < transformedTile.GetLength(0) - 2; x2++)
                        {
                            pictureWithoutBorders[x*8 + x2, y*8 + y2] = transformedTile[x2+1,y2+1];
                        }
                    }
                }
            }
            pictureWithoutBorders.DebugPrint();

            var amountOfSea = pictureWithoutBorders.ElementsWhere(e => e == '#').Count();
            var monsters = countMonsters(pictureWithoutBorders);

            return amountOfSea - (monsters*15);
        }

        private static int countMonsters(char[,] picture)
        {
            for (int i = 0; i<3; i++)
            {
                var monsterCount = countMonstersInternal(picture);
                if (monsterCount > 0)
                    return monsterCount;
                picture = picture.RotateClockwise();
            }

            picture = picture.FlipHorizontally();

            for (int i = 0; i<3; i++)
            {
                var monsterCount = countMonstersInternal(picture);
                if (monsterCount > 0)
                    return monsterCount;
                picture = picture.RotateClockwise();
            }
            return -1;
        }

        private static int countMonstersInternal(char[,] picture)
        {
            //   01234567890123456789
            // 0                   # 
            // 1 #    ##    ##    ###
            // 2  #  #  #  #  #  #   
            var monsterOffsets = new List<Tuple<int, int>>();
            monsterOffsets.Add(new Tuple<int, int>(0, 1));
            monsterOffsets.Add(new Tuple<int, int>(1, 2));
            monsterOffsets.Add(new Tuple<int, int>(4, 2));
            monsterOffsets.Add(new Tuple<int, int>(5, 1));
            monsterOffsets.Add(new Tuple<int, int>(6, 1));
            monsterOffsets.Add(new Tuple<int, int>(7, 2));
            monsterOffsets.Add(new Tuple<int, int>(10, 2));
            monsterOffsets.Add(new Tuple<int, int>(11, 1));
            monsterOffsets.Add(new Tuple<int, int>(12, 1));
            monsterOffsets.Add(new Tuple<int, int>(13, 2));
            monsterOffsets.Add(new Tuple<int, int>(16, 2));
            monsterOffsets.Add(new Tuple<int, int>(17, 1));
            monsterOffsets.Add(new Tuple<int, int>(18, 1));
            monsterOffsets.Add(new Tuple<int, int>(18, 0));
            monsterOffsets.Add(new Tuple<int, int>(19, 1));
            var monsterWidth = 20;
            var monsterHeight = 3;

            var count = 0;
            for (var y = 0; y < picture.GetLength(1) - monsterHeight; y++)
            {
                for (var x = 0; x < picture.GetLength(0) - monsterWidth; x++)
                {
                    if (monsterOffsets.All(mo => picture[x+mo.Item1, y+mo.Item2] == '#'))
                        count++;
                }
            }

            return count;
        }

        private static char[,] Transform(Arrangement arrangement)
        {
            char[,] ret = arrangement.Tile.grid;
            if (arrangement.Flipped)
            {
                ret = ret.FlipHorizontally();
            }
            for (int i=0; i<arrangement.ClockwiseRotations; i++)
            {
                ret = ret.RotateClockwise();
            }
            return ret;
        }
        
        private static Arrangement FindRight(Arrangement left, List<Tile2> tiles)
        {
            string topDownEdgeToMatch = "";
            var others = tiles.Except(new Tile2[] { left.Tile });
            if (left.Flipped)
            {
                switch(left.ClockwiseRotations)
                {
                    case 0:
                        topDownEdgeToMatch = left.Tile.anticlockwiseEdges[3];
                        break;
                    case 1:
                        topDownEdgeToMatch = left.Tile.anticlockwiseEdges[0];
                        break;
                    case 2:
                        topDownEdgeToMatch = left.Tile.anticlockwiseEdges[1];
                        break;
                    case 3:
                        topDownEdgeToMatch = left.Tile.anticlockwiseEdges[2];
                        break;
                }
            }
            else
            {
                switch(left.ClockwiseRotations)
                {
                    case 0:
                        topDownEdgeToMatch = left.Tile.clockwiseEdges[1];
                        break;
                    case 1:
                        topDownEdgeToMatch = left.Tile.clockwiseEdges[0];
                        break;
                    case 2:
                        topDownEdgeToMatch = left.Tile.clockwiseEdges[3];
                        break;
                    case 3:
                        topDownEdgeToMatch = left.Tile.clockwiseEdges[2];
                        break;
                }
            }

            var nextTile = others.Single(t => t.anticlockwiseEdges.Contains(topDownEdgeToMatch) || t.clockwiseEdges.Contains(topDownEdgeToMatch));
            if (nextTile.clockwiseEdges.Contains(topDownEdgeToMatch))
            {
                var edgeIndex = nextTile.clockwiseEdges.IndexOf(topDownEdgeToMatch);
                switch (edgeIndex)
                {
                    case 0:
                        return new Arrangement(nextTile, true, 3);
                    case 1:
                        return new Arrangement(nextTile, true, 0);
                    case 2:
                        return new Arrangement(nextTile, true, 1);
                    case 3:
                        return new Arrangement(nextTile, true, 2);
                }
            }
            else
            {
                var edgeIndex = nextTile.anticlockwiseEdges.IndexOf(topDownEdgeToMatch);
                switch (edgeIndex)
                {
                    case 0:
                        return new Arrangement(nextTile, false, 3);
                    case 1:
                        return new Arrangement(nextTile, false, 2);
                    case 2:
                        return new Arrangement(nextTile, false, 1);
                    case 3:
                        return new Arrangement(nextTile, false, 0);
                }
            }
            throw new Exception();
        }

        private static Arrangement FindBelow(Arrangement above, List<Tile2> tiles)
        {
            string leftToRightEdgeToMatch = "";
            var others = tiles.Except(new Tile2[] { above.Tile });
            if (above.Flipped)
            {
                switch(above.ClockwiseRotations)
                {
                    case 0:
                        leftToRightEdgeToMatch = above.Tile.clockwiseEdges[2];
                        break;
                    case 1:
                        leftToRightEdgeToMatch = above.Tile.clockwiseEdges[3];
                        break;
                    case 2:
                        leftToRightEdgeToMatch = above.Tile.clockwiseEdges[0];
                        break;
                    case 3:
                        leftToRightEdgeToMatch = above.Tile.clockwiseEdges[1];
                        break;
                }
            }
            else
            {
                switch(above.ClockwiseRotations)
                {
                    case 0:
                        leftToRightEdgeToMatch = above.Tile.anticlockwiseEdges[2];
                        break;
                    case 1:
                        leftToRightEdgeToMatch = above.Tile.anticlockwiseEdges[1];
                        break;
                    case 2:
                        leftToRightEdgeToMatch = above.Tile.anticlockwiseEdges[0];
                        break;
                    case 3:
                        leftToRightEdgeToMatch = above.Tile.anticlockwiseEdges[3];
                        break;
                }
            }

            var nextTile = others.Single(t => t.anticlockwiseEdges.Contains(leftToRightEdgeToMatch) || t.clockwiseEdges.Contains(leftToRightEdgeToMatch));
            if (nextTile.clockwiseEdges.Contains(leftToRightEdgeToMatch))
            {
                var edgeIndex = nextTile.clockwiseEdges.IndexOf(leftToRightEdgeToMatch);
                switch (edgeIndex)
                {
                    case 0:
                        return new Arrangement(nextTile, false, 0);
                    case 1:
                        return new Arrangement(nextTile, false, 3);
                    case 2:
                        return new Arrangement(nextTile, false, 2);
                    case 3:
                        return new Arrangement(nextTile, false, 1);
                }
            }
            else
            {
                var edgeIndex = nextTile.anticlockwiseEdges.IndexOf(leftToRightEdgeToMatch);
                switch (edgeIndex)
                {
                    case 0:
                        return new Arrangement(nextTile, true, 0);
                    case 1:
                        return new Arrangement(nextTile, true, 1);
                    case 2:
                        return new Arrangement(nextTile, true, 2);
                    case 3:
                        return new Arrangement(nextTile, true, 3);
                }
            }
            throw new Exception();
        }

        private struct Arrangement
        {
            public Tile2 Tile;
            public bool Flipped;
            public int ClockwiseRotations;

            public Arrangement(Tile2 tile, bool flipped, int clockwiseRotations)
            {
                Tile = tile;
                Flipped = flipped;
                ClockwiseRotations = clockwiseRotations;
            }
        }

        private const string ProblemInput = @"Tile 2131:
###..#...#
##..#.##.#
..###...#.
......#..#
##.#.....#
..#..#...#
......#...
......#...
.........#
#.##..#.##

Tile 2441:
..#.#.....
#.#.......
..........
...#.#....
....###.#.
#....#...#
#.#...#...
#...#.....
#...#....#
.##.#.#..#

Tile 1571:
....#....#
...#...##.
.#.....#..
....#..#..
#....##.##
###..#....
....#.#.#.
..#....#..
#...###..#
#.########

Tile 2179:
#####..#.#
##......##
#.....#.#.
#...#....#
.....#.#..
####.....#
.#..#.#..#
##..###...
##.....##.
..#.####.#

Tile 1433:
#....#..##
#.##...#..
.........#
#..#....#.
#......#..
.#.....###
#......###
#..#..#...
.........#
.#.#.#....

Tile 1283:
...#.##...
##.......#
#..#.....#
.####.##..
..#..#.###
#.....#...
.##..##.##
#........#
..........
#.##......

Tile 1667:
.#####.#.#
...#.....#
##...#...#
#..#....#.
.........#
...##..#.#
...#..#..#
#.#.....##
#.........
#..#..####

Tile 1657:
##.#.#..##
#.....#.#.
#........#
....#....#
......#...
......#..#
#..#.....#
.##.......
#...#.##.#
.#.##..##.

Tile 1259:
..#.....##
##.......#
.......#.#
#..#.....#
.#..#.....
#.#..##..#
.##.....##
##.......#
##...#....
########..

Tile 2161:
.#.#.###.#
....##.#..
..##.#...#
....#.#..#
#.........
.#.#......
#.#.......
#.##.....#
##.##....#
.#######.#

Tile 3793:
.##.##..##
#.........
........#.
.#.......#
.......#.#
.#.......#
#.......##
....#....#
#.#.......
####..##.#

Tile 2953:
....####..
#..#...##.
.........#
.#...#..##
..#..#.#..
..........
#.........
....##.#..
##..###.#.
...####.##

Tile 1979:
.####.###.
.....#.##.
##..#.#..#
.##..#.###
#..#..#..#
.#...####.
#....##..#
###.#...##
.#...##..#
.##.#..##.

Tile 3061:
...####..#
#...#....#
..#...#...
#.##......
.........#
#.#.......
#........#
.....#....
#.#......#
.##....#..

Tile 2731:
..#...##.#
#......#.#
.....##...
.....##..#
#.##.#..#.
##....##..
..........
#........#
.........#
##..#...#.

Tile 1439:
..###.####
....##...#
........##
.........#
#.......##
..##...#.#
....#.#..#
......#...
#.##.#.#.#
##.##...##

Tile 3347:
..#.##.###
...#...#.#
#.#.#...#.
#..#...###
.##....#..
##........
##.#..#..#
..#....#..
..##..#.#.
..#######.

Tile 1559:
###.#.####
#.......#.
#.......#.
..#.##..#.
..#.....#.
#...#..#.#
#..#....##
#.##......
#.#..#...#
.#..#####.

Tile 3559:
...##.##..
#........#
..##......
#...#.....
##.#..##..
#.........
#.#....#.#
#.........
....#....#
#...#.###.

Tile 3109:
.##..#....
#........#
#.....##.#
.....##.#.
#.#......#
#.##......
#......#.#
#....##..#
.#.##..###
.#.....#..

Tile 3701:
#..#..#...
..#..#...#
#.........
#.........
.....#.###
.....#....
.#..#...##
#.........
#.#.#....#
#..###..##

Tile 1597:
.##.#.#.#.
..#.#.#..#
#.......##
...#.....#
#........#
.#.......#
..#.#....#
..........
#.#.##...#
.....####.

Tile 1231:
.#####....
#....##...
##..#..#.#
...#......
#.........
#..#......
#.#...#..#
..#..#..##
#..##..#..
##.###....

Tile 3389:
.###....#.
#.......#.
#....#....
#...#....#
..#.##...#
.....##.##
..........
###...#..#
#..#.#.#..
#####..##.

Tile 3461:
....#.###.
..........
..........
...#...#.#
#....##...
..#..#....
##.#..#..#
#....#...#
...#.#...#
....######

Tile 2837:
##..#...##
#....##..#
...#....##
.#...#....
#.....#...
#.#...##.#
##..###..#
#........#
#.........
..###..#..

Tile 1423:
####.#...#
#.#......#
...#.....#
#####.#...
#..#..##..
.....##...
##.##.....
#.#....#.#
...#.....#
#.#.#..#..

Tile 3943:
#.....#..#
...##..#..
...#......
.#.#......
#...#.....
#.#.#..#..
........##
.........#
##..##.#.#
..#.#.##..

Tile 3779:
..#.#..#.#
...##...##
.......#.#
..#.....#.
..##......
#.#....###
.#.#.....#
#.........
##...#...#
#.#.##.###

Tile 1583:
##..#.##.#
..#.....#.
#..#...##.
#..#.#.#.#
.......#.#
#.##..###.
#######.#.
..#.....##
#.##....##
#..##.#.##

Tile 3041:
...#.###..
#......#..
..#....#..
.##.....##
#.#...##.#
......#.#.
..........
.#...#.#..
..#..#...#
#.##.##.#.

Tile 1889:
...#.##.#.
.#..#.....
#..#.....#
........#.
.........#
..#...#..#
###....###
#.#.......
#..#.....#
...##..#..

Tile 1493:
....##..#.
#........#
#..#......
.....#..#.
#.........
...#.....#
.........#
.#.#.....#
..........
...#######

Tile 1987:
#..#####..
#........#
###..##..#
.#....#...
......#...
#...##..##
........#.
.##.....#.
##...###..
##..##.#..

Tile 2003:
..#.#.####
##..######
#.#..#.#.#
...#.....#
#.........
......#..#
.......#.#
##........
##....#..#
..#..#.##.

Tile 1297:
.....###..
...#...#.#
...#.#...#
..........
.....#...#
#.......#.
..#.......
..#.......
....#....#
#.#######.

Tile 3049:
#..###..#.
..###...##
##........
#........#
.....#.#..
##..##..#.
#..#.#....
#.....#...
###.#.....
#..#....##

Tile 2143:
#...#.#.#.
#......#.#
#.#..#####
###..#.#..
#.##...###
....#.#.#.
#........#
..........
#...#...#.
.##.##..##

Tile 1753:
#.#.#..##.
....#.....
#...###...
#.....#.#.
#.#..###..
......#...
...#.....#
..#....#.#
#...##....
..#.#..##.

Tile 3407:
..###.##..
....#.#..#
.#...#....
....##....
#..##....#
....#.....
.....#....
.#.......#
#.....#..#
.##...#.##

Tile 1783:
.#..####..
##........
...###..##
#......#.#
#.........
..........
##........
........#.
##.####.##
.####.#.#.

Tile 3593:
#.#.....##
......#..#
##...#.#.#
#.#.#...#.
..........
.....###..
#...#.#...
###...##..
#.#...#...
..#.....##

Tile 1249:
###....##.
##..#..#.#
.#.#.##..#
..#.......
.........#
#....#.#.#
..##..#..#
###.####.#
.#.#.....#
....##...#

Tile 2963:
..##.#..#.
#..##....#
#.#..#..##
.#...####.
..#......#
##...##.#.
.........#
#....#...#
##.....#.#
####.#....

Tile 1289:
.#....#...
.##.#.....
.##.#.....
#.........
....#...##
##.......#
...#.....#
..........
..........
.#.##.#...

Tile 2053:
#.#.#...#.
....#.##.#
.........#
#.....#..#
....#.....
.##......#
#...#.#...
#...#.#...
...#..#...
#..#.#.###

Tile 3001:
.#...#...#
....##..#.
....##...#
...###...#
#..#....##
#.......#.
#.##.....#
....#.#.##
.#..##.#..
.##.....##

Tile 2591:
.####.##..
......#.##
..#......#
....#....#
..#....###
..##......
#........#
.......#.#
#....#....
##.###...#

Tile 1931:
.##..#.#..
#.........
..#......#
.#.......#
....#.....
..#.......
##...##...
#...###..#
#.#.....#.
#.#..#...#

Tile 3203:
.###.....#
...#..#..#
##........
.........#
#.#....#..
.#........
#.......##
#....####.
#.........
##....#...

Tile 1747:
#...##..##
...#.#...#
#.#......#
#....#..##
.##.......
##..#.##..
##....#..#
#...#.....
#.#....#..
.##.#.#.##

Tile 3557:
#.##..#...
#.......##
.#.#.#....
#...#.#...
#.#.##....
#...#....#
..........
#....#....
#...#.....
####..###.

Tile 3329:
#..##.....
..........
..#.......
.....##.##
....#.....
.##....#..
.....#....
..........
#####.#..#
#..#.##...

Tile 3067:
#..##....#
.#.#..#..#
..#......#
#...#....#
#.....#...
..#.#....#
#.#...#..#
.....#....
..#......#
.#.##.....

Tile 3371:
#.###..#..
#.#....#..
#....#...#
#.....#..#
..#..##..#
#.#.....#.
.#.#..####
#.#..#....
..#..#....
##......#.

Tile 3467:
#.#....#..
##....##.#
...#..####
...#.#...#
.........#
..#.......
...#...#.#
...#......
.####....#
..#.....#.

Tile 3923:
...#...##.
#........#
.#........
#.......##
#.#.#.....
.........#
....#.....
#.........
...#.#..##
#.######..

Tile 1453:
#.#...##..
##....#..#
#......#..
..........
#.#.##...#
#........#
.....#....
.##....#..
..........
#.###.###.

Tile 3331:
.##.#..#.#
.......#.#
#......#..
..#......#
#..#.....#
#..#.....#
#.#.....##
#.........
..........
####.#...#

Tile 2633:
..#.#....#
###......#
.......#.#
....#....#
###.#...#.
.#....#..#
##.....#.#
.##..#....
#...#...#.
..#...##..

Tile 1811:
#..#######
.##.....#.
#.....##.#
#..#...#..
#...#.....
##..#..#..
#.##.#..#.
.#........
##...#.#.#
.####.#..#

Tile 1777:
.####...#.
.#....#.#.
###..#....
..#..#...#
#.#.#.##..
#...#..###
#....#...#
......#...
.....#...#
####.#..##

Tile 1697:
#.##..#..#
#........#
##.####...
##......#.
#.##....#.
##..#..#..
#..#..#..#
#.....####
...#.#....
##.###.#.#

Tile 2551:
#..###..##
..#...#...
..........
.#..#.....
##.....#..
...#.#.#..
..#..##..#
.##.......
.###......
...###.##.

Tile 2351:
#...#####.
......##.#
....##...#
.........#
#.........
#.....#..#
#........#
..##.....#
....#..#..
.#.#....##

Tile 2689:
..###..###
.........#
#...#.....
##..#..#..
#.#.###...
....#....#
#........#
......#.#.
#.#..####.
####.###.#

Tile 3253:
..##......
..#.#....#
..#.#...#.
#...#.....
#..#.#....
.#....#...
....#.....
#.........
#......#..
#..#.#####

Tile 2843:
####..#...
#.....##.#
##....#...
.##..#.#..
..###.#...
...#...###
#..#..#...
.....#..##
#....#..##
...#..#.#.

Tile 1187:
.##.####.#
......#..#
...#.....#
.....#...#
.......#.#
.........#
#..#......
..#......#
#...#....#
#####...##

Tile 3529:
.....#####
.#...#....
#..#.#....
#........#
.#....#.##
##..##.#..
..##.#..##
.#........
......#..#
###..#.##.

Tile 1489:
.##.#.####
#.#..#.#..
#....#....
#.#..#....
.#...#....
..........
#.#..##..#
.........#
.......#..
.....###.#

Tile 1097:
###.###.#.
.#...#....
.......#..
.#....#.#.
.....#...#
#.#.....##
####..##.#
###......#
#........#
##..#.#.##

Tile 3491:
#...#.##..
...##....#
..#.....#.
#.#.#....#
#..#.#...#
.....#...#
....#.....
....#.#..#
........##
##....#...

Tile 3947:
######.#.#
#.#.#....#
...#.#..#.
...#.#....
#...#...##
....#.....
.......#..
##..#....#
..#..#....
.###..####

Tile 3313:
.#....#.##
.##.......
#.##......
##........
#.#.......
...#.....#
...#......
......#..#
##......#.
####.#..#.

Tile 1277:
#..###..#.
#....#...#
.##....###
#........#
#....##...
#.#...#..#
#...#....#
...#...#..
...##....#
.#...#.#.#

Tile 3121:
.#...#..##
........#.
..#..#####
#.....#.##
..#.....##
..........
#.........
#..###...#
.........#
###.#.#.#.

Tile 2287:
#....#.#.#
#.#..#....
#.....#...
..........
##.#......
##..#..###
..##....##
.....#....
#........#
..#.#...#.

Tile 1171:
#..#.#...#
..#...#...
##.......#
####...#.#
#.#.#.#...
.#.......#
##....####
#...##..#.
........#.
##.###.##.

Tile 2503:
#...#.#..#
..#..##.##
...#...###
.##.#...##
#.........
#........#
##.......#
....#.#...
...#.....#
..#...#.##

Tile 2579:
#.#..#..##
.........#
####......
..#....#..
#.#..##..#
#.....##.#
....#....#
.........#
..#..#..##
#..##.####

Tile 1867:
.##.....#.
.........#
.#....#.#.
#....#..##
##........
...#......
#....##..#
##..##.#.#
#.........
####..##.#

Tile 2081:
.##..#...#
......###.
..##.#..##
##..##....
#..#.#...#
....#...#.
##.....#.#
..#....#..
#...#...##
.####..#.#

Tile 3319:
..#...#.#.
.....#....
.#........
#.......#.
#.........
.....#...#
#.....###.
#........#
........##
#...#####.

Tile 3727:
##.#.#...#
#.........
..##.....#
#...#.#...
#.#.#...#.
.###.#....
......#.##
#...#....#
...#.#...#
#..#.#..##

Tile 1103:
###.##.#..
.#..#.##..
.#..###.#.
#.#..##.#.
.##...##.#
....#....#
....#..#.#
...#...###
...#..#.#.
###.#..##.

Tile 3463:
##...#.#.#
....#..##.
........##
.#.......#
..#..#....
#.#....#..
##....#..#
...#..#..#
#.#......#
#.###..##.

Tile 3449:
..###.##..
..#..#....
#...##....
.####..###
.#..###.##
#.#....##.
......#..#
..........
##........
#.......#.

Tile 2207:
#...#.#.#.
.##.....#.
#####....#
..#.#.....
#..#....##
.#....##.#
#....#.#.#
...#....#.
..#.#.....
##..#.##.#

Tile 2647:
.#####..#.
.#..#....#
....#.#..#
.##.......
...#..###.
####...###
##.....#.#
#..#.....#
#.#.....##
..##.##..#

Tile 2539:
##..#..###
...#..#..#
.#....#...
###....#..
.#...#....
...##.....
..#..##.#.
...#......
#...#....#
#.#.##....

Tile 2927:
.#.....##.
#.......#.
#..##..##.
#...#.....
....#...##
.#.#.#..##
........##
###.#...#.
...#.....#
.##.....##

Tile 2153:
#.#..##..#
...#.####.
#..##.....
.........#
#.....#..#
...##.....
...#...###
..##..####
...#...###
##.#.##.##

Tile 3083:
###.#..#.#
...#.....#
#.......##
..#......#
...#.....#
..........
..#......#
.##....#..
#......###
..#......#

Tile 1237:
.#....###.
#.........
#...#...##
.....#.#..
#.........
....#..#..
........##
##......#.
#.........
#..##.#...

Tile 2239:
#.....##.#
##........
....##....
#.##....##
###.#..###
#........#
.........#
#..#.#.#..
.........#
.#..#.....

Tile 1093:
.####..#..
##....#...
..#.#...##
...#..#...
#........#
#...#.#..#
#.#####..#
....#.#...
#.#.#..#.#
#.#..#####

Tile 2221:
......#.#.
....#..#.#
#..#...#.#
.#........
#........#
##....##.#
##.....#.#
##...#...#
#..#..#...
..##....#.

Tile 1523:
##.###.###
###..#..#.
#..##...##
#..#.#....
#..#.....#
#.#......#
#.......#.
#.........
.#.###.##.
...#.#...#

Tile 3673:
###.##..#.
..........
.#.......#
#..#.##...
......#..#
#.....##..
##....#..#
..#...#...
#...#...#.
#####...##

Tile 2063:
####.....#
#........#
......#..#
#.#..#....
#.#.....#.
...#.....#
#...##..#.
#.....#...
......#.##
..##.#.##.

Tile 3631:
#.#.#.####
...#.#....
...#.#...#
#....#.#..
#..#...###
#.##......
#..#...###
#.#.#..#..
.......##.
.#.#......

Tile 2693:
###.####.#
...#......
....#.#..#
#.#..#....
.....###.#
#....#....
##.###....
#.....#..#
#.....#...
######.#.#

Tile 2593:
###.....##
.....#....
..#...#...
.....#...#
..##......
###.......
.##......#
#....#...#
#..#..#..#
#.###.#.#.

Tile 3607:
.##.##.#..
#...#..##.
....#..#..
#......#.#
..........
..........
.......#..
#....##..#
.......#.#
.#####....

Tile 1223:
.####..#.#
......##..
.........#
...#....#.
#....#.#..
##.#...#..
.#......##
.........#
#.#.#....#
##.#..#..#

Tile 3433:
##.###..##
#..#.##...
##....#..#
#.........
###.##.#.#
....##....
#...#.#..#
...#......
##....#..#
##.#.#...#

Tile 1217:
#..###.###
#.#...#...
..#.##....
#.......#.
#.......#.
##...#..##
#..#..#...
#..####..#
#...##...#
.####.###.

Tile 2861:
#..##.#.#.
#...#.##..
.#...####.
.......#.#
..#...#..#
#...##...#
#.....#.#.
......##.#
##..#.##..
###..####.

Tile 1327:
....#.####
...####...
....#.#...
..#...#.#.
....#....#
..#..#...#
....##....
......#...
#....##.##
##.######.

Tile 3613:
#.##.###..
#..#..#..#
..##.##...
..#..#####
...###..##
#...#.#.##
#....#....
...#.....#
.###.#..##
####...###

Tile 2909:
#####..#..
#...#..#..
#.#....#..
..#.......
...##...#.
.#.##..###
...#.....#
.........#
...#.##..#
#######.##

Tile 1709:
#.#.##....
#......#.#
.#........
#......#..
.........#
......#.#.
..##......
.....#....
..#.##....
#....#####

Tile 2857:
###.#...##
#.#....#..
........##
#.#...#..#
#......#..
.....####.
......#..#
..........
#.#.....##
..#..##...

Tile 3541:
#.#.##.###
.....##..#
..#.#.....
#..#.....#
##....#...
#.#...#...
....##....
.........#
.##....#.#
...#.....#

Tile 2459:
##..#.#..#
...#.#.##.
##...#....
.##.#.#...
#.....#..#
...#.....#
......#...
.....##...
...##...#.
..#..###..

Tile 1621:
#.##.###..
##..#.#...
.#.##...##
.###.#..##
.#..##.#..
#.......##
.....##...
..#.......
#.........
#..#..###.

Tile 3833:
#..#...###
.........#
#.....#...
#..#...#.#
#......#..
.#........
..#.......
.........#
......##.#
.#..#.#.##

Tile 1117:
..##.....#
..##......
#..#.#...#
#...##..##
....#...#.
#...#....#
....#..#.#
...#.....#
#..#.....#
..#.##..##

Tile 3413:
#.#..#.###
....##.#.#
...#.....#
.........#
#.........
.....##.##
..#....#..
#.....##..
#........#
#.######..

Tile 2141:
.##.#..#.#
#.###.....
#..#....#.
#..#....#.
..#.......
.#.....#.#
##......#.
..#..#.###
...#.....#
...#..#..#

Tile 3547:
####.#..##
...#.#...#
......#...
##.#......
#........#
.##...#..#
.#...#..##
#..#..#...
#.##......
##.###..#.

Tile 1069:
##.#..##.#
.#....#..#
.##......#
.#....#.##
#..#..#...
..#.#.....
..........
#....#..#.
##........
..#...##.#

Tile 3169:
##.##.#..#
#.....####
.##...##..
#....#.#.#
#.#..##...
#........#
.#.......#
.#........
.........#
#..#...#.#

Tile 1307:
..##..#.##
###.#...##
..##.....#
#.##.....#
.....#.#..
..#....#..
....#...#.
.....#..##
.#.....#.#
##.#.....#

Tile 3359:
#.#.#..#..
........#.
.#...#...#
#....##..#
.#.#......
#...#.#.#.
.#...###.#
#........#
.#.#.....#
###.#..#..

Tile 2713:
#...#...##
##.###.#..
..##....#.
...#.....#
..#.....#.
#....#...#
....#.....
#..##....#
.........#
#######..#

Tile 2897:
...####..#
...#...#.#
#.......#.
......#...
.....#...#
.........#
.#.#......
######..##
...#..#..#
#..###.###

Tile 3929:
....#.#.#.
.#...#....
#.##..###.
..##......
#.##...#..
#...#.....
.......###
#.##.....#
#...#.....
...#######

Tile 1031:
#####.#.#.
#....##.#.
#..#..#..#
.........#
##.#...#.#
....#...#.
.......#..
#......#..
......#..#
.#.....#.#

Tile 1361:
#.#.#.....
.#........
..##...#.#
#..##.#..#
.....#####
.......##.
#........#
..........
#..#...#.#
##.###.#.#

Tile 3853:
#..##.....
.........#
#..#......
#.........
###.....#.
...#.#...#
#.........
..........
..........
##.##.####

Tile 1009:
...#.#...#
..##....#.
.......#..
.....#...#
#..#...#.#
.#..#..#..
..##.#..#.
....#.#..#
#..#.#....
###.#.####

Tile 3761:
.##.#.#.#.
..#......#
#....##...
.#........
#...#..#.#
.........#
#.........
.........#
.#.......#
#..#......

Tile 2521:
.#.....###
#........#
...##....#
###.......
#....#....
###..#...#
##....#..#
.#........
.........#
#..#...###

Tile 1907:
.####.#..#
...#..#..#
#.#..##.##
##....##.#
#........#
#...#.....
#.........
#.#..#...#
##....###.
.##.###.#.

Tile 2039:
...##..###
###...#...
.#.......#
...##.#...
.........#
......#..#
#........#
###....#.#
#.#......#
#..#...#..

Tile 2389:
##.##.###.
#.#.......
........##
.........#
...#......
..........
#...#..#.#
#.......#.
....#..###
####.###.#

Tile 2089:
#.#..#.#.#
#........#
.#.....#..
..........
#....#...#
#........#
#.........
#.........
#...#.....
#.#...#..#

Tile 2137:
.#####..##
..#......#
#.#..#....
.#......#.
#...#.....
......#...
....##..#.
.##....#.#
#..#..#..#
#..####.##

Tile 3191:
..###....#
.........#
........##
....#..#..
.........#
#.........
..#..#..#.
.#....###.
#.....#..#
#..#####.#

Tile 2789:
#.#.#.##.#
...#.....#
#..###...#
#........#
##.......#
#..##....#
.#........
.........#
..#......#
......#..#

Tile 1567:
....#....#
#..##..##.
.....#....
#.##.##...
....##....
##...#...#
#.#.#....#
......###.
..#.#..#.#
.#..##.#.#

Tile 1163:
.###..##..
.#.....#.#
#...###..#
.....#....
.........#
..#...#..#
..#..##.##
.##..###.#
.........#
##.##..#.#";
        private const string ProblemTestInput = @"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...";
    }
}