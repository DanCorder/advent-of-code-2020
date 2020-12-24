namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day24
    {
        public static int SolveProblem1()
        {
            var directions = ProblemInput.SplitToLines().Select(ToDirections).ToList();
            return directions.GroupBy(d => d)
                .Where(g => (g.Count() % 2) == 1)
                .Count();
        }

        private static Tuple<int, int> ToDirections(string directions)
        {
            var e = 0;
            var se = 0;
            var sw = 0;

            var i = 0;
            while (i < directions.Length)
            {
                switch (directions[i])
                {
                    case 'e':
                        e++;
                        i++;
                        break;
                    case 'w':
                        e--;
                        i++;
                        break;
                    case 's':
                        if (directions[i+1] == 'e')
                            se++;
                        else
                            sw++;
                        i += 2;
                        break;
                    case 'n':
                        if (directions[i+1] == 'e')
                            sw--;
                        else
                            se--;
                        i += 2;
                        break;
                }
            }

            // e, se, sw
            // sw == se - e
            se = se + sw;
            e = e - sw;

            return new Tuple<int, int>(e, se);
        }

        public static int SolveProblem2()
        {
            var directions = ProblemInput.SplitToLines().Select(ToDirections).ToList();
            var blackTiles = directions.GroupBy(d => d)
                .Where(g => (g.Count() % 2) == 1)
                .Select(g => g.Key)
                .ToHashSet();

            for (var i = 0; i < 100; i++)
            {
                blackTiles = flipTiles(blackTiles);
            }
            return blackTiles.Count();
        }

        private static HashSet<Tuple<int, int>> flipTiles(HashSet<Tuple<int, int>> currentTiles)
        {
            var checkedTiles = new HashSet<Tuple<int, int>>();
            var newTiles = new HashSet<Tuple<int, int>>();

            foreach (var currentTile in currentTiles)
            {
                var tilesToCheck = getNeighbours(currentTile);
                tilesToCheck.Add(currentTile);

                foreach(var tileToCheck in tilesToCheck)
                {
                    if (checkedTiles.Contains(tileToCheck))
                        continue;

                    var neighbours = getNeighbours(tileToCheck);
                    var blackNeighbours = currentTiles.Intersect(neighbours).Count();
                    var isBlack = currentTiles.Contains(tileToCheck);
                    if ((isBlack && (blackNeighbours == 1 || blackNeighbours == 2))
                        || (!isBlack && blackNeighbours == 2))
                        newTiles.Add(tileToCheck);
                }
            }

            return newTiles;
        }

        private static HashSet<Tuple<int, int>> getNeighbours(Tuple<int, int> tile)
        {
            var n = new HashSet<Tuple<int, int>>();
            n.Add(new Tuple<int, int>(tile.Item1, tile.Item2 - 1));
            n.Add(new Tuple<int, int>(tile.Item1, tile.Item2 + 1));
            n.Add(new Tuple<int, int>(tile.Item1 - 1, tile.Item2));
            n.Add(new Tuple<int, int>(tile.Item1 + 1, tile.Item2));
            n.Add(new Tuple<int, int>(tile.Item1 + 1, tile.Item2 - 1));
            n.Add(new Tuple<int, int>(tile.Item1 - 1, tile.Item2 + 1));

            return n;
        }

        private const string ProblemInput = @"enwwsenweswweseseswnenewwswseswenw
nwnwneswwnenwnwnwenenwwnesenenwsenenee
eneswwenwneeswneenenw
swwswswwwswswsweeswww
swewnwneeewnewwwswwnwwswswnwww
seswnwenesenwsenenwwenwsenwswnwseswnw
nwswwenwwwswwnwnewswnwnwwwswnee
seswseswseseseseeseseseswnwsesw
swswseswseswswnweneswswseswse
wenwswwswwwswwnwsesewwenwseswwne
nenewswsenenenenenwnenesewswnenenewe
eeneneswswnenwnenenwnwnwnwnwnwswenwnesw
nwsweneeeeweenenwneeseeesenesw
nwnewsesesesesesesewsweseenwsesesese
nenwneenenwneneneswsenenenenewnenwnenewse
wswnwseseneswswswenewswneswenw
wwswsewswswwwnewnwwswswewsewwnw
nwnwnwsenenwnwnwneenwwnwnwswwnwnwenwnw
seenweeswenwwseeeeeesweesese
neswwewnewwwnewseswwwwwnwseswnew
swseseesweenwswnwswswnwsesewseswene
nwsenenenwsenenenesenewneswwnenenenesenene
ewnenwneswsenenwnwsenewsenwwnenwnesesene
sesenenwswsesesweseseswnwsesenesenewe
wwenwnewewswwswwswwsewwenww
neweeseseswswwweenwnenwweeneenew
neseneseneneneneneswnenwswnewnenenenene
wswwwnwewwwnwneswswsewewwweww
swseseswwnwswswswnwnew
eseseesewnwseseseewseneseesesenwsw
swsweswswnesenwswsesw
seeeswnenweenewseeesweeenenewsw
swnwsewesewnenwswneweneswseenwsew
wsesweswnwswswnesweswwwswswswswsww
wseenwnwswneewnwswsweenenwnwswneew
seswnwseeeseseseseseswswneswseesenwsew
nwnwnenwnwwnwsenwenwneneneswnwnwnw
swseeseswswnwswsesesenwsesenwswnwnw
nwsewnwwnwwewwneswnwenenwseneswsw
wwsewswenwwwnweenwnwnenwswswwnw
wneeneeneeeeseenesewneneeswee
wnwenwswneneenwswnenesenwnenwwneswnwse
sewsenwseswsewewswneswnwneseseswseee
wnwwwswnwnwnwwsenwneww
swswnwweseeswneswswswswswnwswswswswswne
neeneneeeneeeswenwnenenwswneneswe
wneeseeeseeseswsenwweeeeseee
nenweesenesenewwwsweswnwnwweswne
senewseswsewswneseesenwsenenwsesenwsw
ewenwnwseeeenwesenwnewswwswe
nwnwnesesewnwnwnewneenwnwnwnwnwswnwne
enwwewnwnesenenenwnwnenenweneseswwswne
nwswnenwswnwwnwseswnwnwewnwwwnenew
wweeswweneseewnewwwewwewse
swseseswswseseneseswneswseswneswseswnwsw
wnwnwnesenwwnwnwnwwsewnwswnenwwnw
sewsenwswsenewneneewnwswwnesw
wswseneeswsenwsesenwwneswnenwewsesesene
nwnwnwwnwnwnwenwsenwwnwenwnww
esenweeswseseenwe
nwnenwwnwnwneweswswwnwsweesewsew
swwswseeseseneseseeswswwswnesesenwwswsw
seseesewseseeeswnwseseenwseesesenwsese
wwnwwwswwwwwnewww
swseenewseneneeeswswnwswsenwswenwsene
swwswwseewnwnenwwnewwnenwsewwnw
nwnwenwswnwnwnenwnwwnenwsenenenwnwwsenw
nwenwwnwnwnwnenwnwneswnwnwnwseenwnwnw
seswnenenewwneeneneesesewenwenwse
nenenenewwnenesee
senwwwwenwwnwwwnesenwnwsenwnwnwnw
senwneswwewwseswwnwswesewnenwwwww
nenenenenenwneneewneeneswneswsewene
swseneswswwswswnwswnewswseseseswswswswswne
wwenwewnwwnwnwnwwwwswneswsewnww
swnwnwwswswseswsenweneseswswswwswsee
wnwswsenwwwswsweswwswswswewwww
neeeswneeeeeeeeneeneeswwenesw
newwwnewwwnwnwsewnwswsew
enwswnwnesenewwneenenwnesw
eneewwneswnwneeneeswe
seseesesesesesesesenwsesenwesewswnenew
wseswswseswnesweswswswsw
seeeeeenweeeeweseenenenewwe
nwsenewsenweswsewwnwenwwenwse
nwwnwnwwnenwwswsewsenenwnwnwewwse
eeneeweeeeeenee
seswneneewswswswswswswswswneswwwwsw
senwnewnwnwenwnwnwnwnwnwnwnwnw
wwsenwnwwenwwwneewwnwswseneswe
nenenenenenwnenenesenewneneewnwseewsw
wsenwswswnwnwnwnwnenwwnwnwwnwneeenw
neeenwsewsesweeeenweneswenenwswwe
nwwnwenwwnwnenweswwenwesenwnwwnw
seneswnwwswwswwswwwwwnewswneswe
eeneeweweeeenee
wneseseweneseeewsesenesenwwwwnwse
swwnweseswneesesenewseseeswswnwwswne
swnwsenwsewseseseswnwseneseswswswswene
swwseswswseseneseswseswseswswneneswnwsw
sesesenweswswsweswsewseseseseswsw
seswnwewwwsewnwwnenwwnwwnwnwnwnw
enweewseeseenweneseseseenweeee
wwwwwwswwwwneswswsww
sweswnwnwsewswsewnwswwwwneseneesw
nwseswnwnenwsewnwenw
nwnesenewnesweenenenwneneneneenewsene
swswnwwswswseswenwswswwswswe
nenwnenwnwnwnwwnwenenwswnesewnenwnwe
swseeseswweswswswseneswswswswswswww
neswnwnenenwnwneneneneneeswnwnenenwneesw
nenwnwesewnwsenenenwnwswwnwnwnesene
seeeseswewnwneseseenwweeeseneesw
swswswnenwswnwneswseswesewnwseswsenwswsw
senesewseseseseseseeswesenwsewsenesese
nwnwnwwnwnwnwnwsenwnewnwnwww
neswneseeseneswwwnewswwwwwwswwsww
nweesesenwesesesenweeeseseeseswse
neneeneswneswneenesenwwnwsenwneswwnene
enewewswnwnweesweswseeeeeenwe
eswseeseneeseseseseeee
sesesewsesewseesenwsesesenwswesenweswse
nwnwswnesweenwswneseeseswneewnenenenwsw
nwseswswswseeewswswwenwne
swneewswnwswswswseswwseeeswswswneswwsw
eneneeseneewneneenenesenwnewnenene
nwsweseswswswswswswswsw
nwnenwnwwwnwseneswnwenwnwswswwweswnee
nenwnwwnwwesenwseswenenwwnesewnww
sesenwseeweeenweseeeeeenwesee
eeswwesewseseeeesewnw
nwenwnwenwnwnwnwswnwwnwewnwnwswswnw
neweswnwsweneneweneeneneswnenesenwsw
seseseswswsesenesewsesese
seeeeeewesenwenenwewsenwseee
eneneweewwneeneneesene
swnwnweeswswsweesenwsewseswswnwswsewse
neneseneswneneneneswneneswneneneenenwnee
nenewnenewwneneneneneeseneneneseneswse
nwswsweeeseeewwnwswnwswnwne
newwnwsewswwwnewwwsewwwewww
wwwwwnwwwwswwenwnwnwnwwesweew
nwneneneseneeeneswswneweseeeneene
enesesenwnwseseewneswswseesweseseene
wwnwenewwwnwnwnwsenwnwwwnenwnwsw
swwwewwweww
wnewwwseeesewsenwnwnwnwswnwewwnw
eswnwnesenwnwnwseseswnwswseseewswsew
swneneseswnenwnenwenenweswneenwnenwne
wwnewnewsewsww
nwwenwnwneseseewwwnesenwswswswnenw
seenwnwswwsenwseswwsewneswseseesee
eseeseeswseeseeesenenewese
eseseseseeseeseseswesesenenwnewesw
neeswnwswsweeewnweesweswnewnwene
newwnwnwneswwwenwwwsewwwsenww
nenwswnwnwnwswnwneseewswnwnwnee
sesewswwswwsewswnwnwnew
sweswswnwnwnweswswsesw
senesenweneneswsewnenwnenenenenenwenee
enwwnwswnwneenwnwnwnwswnwnwseswnw
newswnwnwswewneneneneswwwseneneesene
seswsesesewswneseseseseewswswsenwsese
newnwnenwnenenwsenwseneneneswnwnenwnene
nwnwnenwnwnwnwnwswnwnwnwwwewnweswnw
nwnenwnwnenenwnwwsenenenwnweswnene
nwwnenwnenenesenewseneneneswnwenwnwnenw
nwnwnwnenwnwwnwsenewnenwsenwnwnwnenwnwsw
swsenwswswswswneswswswsweswswswsewswnw
wwswwswsweswnwswwnwewwsewwwew
sewwnenwnwswwwnwwewnwwnwnenwneswnw
ewneeenweneeseeeswenw
nwnwneeswneseewneneneseeeeee
wsweswewswwnewswnwwwnwnwswwesewe
enwnwnwsenwnwnenwneswnwsenenwneswnwnwnw
nenesenewnenwswnewwsesewswsenewsww
wnenenenenenenenenenenenesenwnesesewnene
wswnwwwwnwnwewnwsew
nesenwwswseenwseswnesesesenweseseseese
nwewnenenwswsenwnewnwneneesenwnwwnwnwne
nwnwnwwnwneneswenenwnwnwsenwneswneeswne
seseseseeeseswnwsenwswneseeeewsese
sewnenweswneeswenesenwenwwswwneswnw
wsesesesewsesesesesesesenwseseeeew
seseeeesesesweewewwesewwnew
nwnwswswenwnenenenwnwnenwnwweneswnenwe
neeneneenwneneneneesweswneseneneew
weewwwwwnewwwwwsewewswnw
swseseenweeeeeeee
seseseeseseseneseseeswseee
nwewnwneenenenwnwnwwswsewnwsenenwne
nenwnesenewnenenenenenenenesenenw
seeesweenwsesese
nenwnenwsesenenenenenwwnene
eseneseswnwsewesesenwseseeeeswsese
swseseesweseneswneesesesenwsee
swnenewwswwwwwnwwewnewswwsesw
eswnwsesenwsesenwenwnesesesweswwnwnenw
nesenwneseseswswseseswswseweswneswswsese
eseswsewnwnewenwseneeeseenesw
eneeeneeneweneewneneeneewsee
sewesesewseswseseseseseswnesenwneseswsw
eneesenwneeseenenwwseneeneeee
neweneswnenweeneeseeneewnesenenwe
wsesewswwwwnwenweswnwneneswnww
eswnwenwwweswwsewswswnwsenwwsww
wnewnwwsenenwnwnwnwnesenenwsesewnene
seswnwwwseneswnenewwnwesesewne
eenwesweweeneeeeweeee
neneenewswwseenenwnenenwnwnesenenenene
nwsenwnwsesenwseseseeseswseseseseseswsese
sesenwseswewswswsenweswnwswswseenesese
swsewswsenwswwnwsweenwnwseseseeswwe
seeseswswneseseswwsesenwswsesesenesese
sweneswnenwswswseswseswswseewnwnenwesw
wswseswwnewwwswswswswsewswswnwwenw
seneenweswnwswneeseseeswsesenwesesee
wwenwsewwwwwww
swseswswwseseseseeswsw
wswneneswswnwwseswwwswswewwsewesw
esesweeeesenenenwnwnew
sesesenweseseeseseseeesee
esewseseseseswsesenesewsewswsesesee
swwswwwswwnwwswwsenewneswswsenwwe
seseswsesesesesewsesenesenesesenwseswsenw
senwwwwenwseneswseneewnwnwenewww
seswsenwsweseswseseneeseeseenwseww
seseswwneneswswwwswnwneswswswsewnwswwsw
eweewneeenewwseewewseene
neeneswnenenenenenene
wnenwwwwnenwswwwswsenwwenwweww
wnenwnenenesweeneneenenenwneenwsese
neewsenenesesenweseseswsweswne
neneeneneeeeneesw
eseneswseenwswneesweseeseeesenwnwee
wwsesenwneswneseseneswnwseseneseswsene
wewsenwewwwnwwwwwwwwesene
wsewnwnwwseeswwsw
swwswwswswswwswsweswwsw
eeeseeneswnwseseneseenwesweswnwswswne
eseswwsenenesesewsesesesesese
eseeswesesesesewseenwsesewnwsesesese
newwwwwsewswnwnwneseswwnwswwwwne
nwesenwnwwswwweweewwwneswwsw
eeeeeeeeeweese
eeneewweswweeneenwseseseeswnenw
swewswwwwnwsweswswswneewseswwww
swwswwsweswwswwnesw
seeseseneesewseseswnwswsesweseneseese
swswswewneseseseseseswnwswnesesesenwse
weseeeeeeeee
swswsewseseeswsesw
seseseswnweneswsenenenwwwswswsenweeese
swnenwneswenwnwsenwenenenenenenewswnene
nenewwwwwswweswewewnwwsewnwse
senwewswswnwenwswseneeswswse
eseeeweweeeeenwsweneeeneseee
enwseeeseeneeweseeseeeeswenwe
nwnwsenwnwnwnwsenwnw
swswwswswweewwwsww
seeeeeeewsee
ewwwnwwnwwsenesewneswswswswswseew
nesenwwnwnwseswswnwneswnwnwnwnwnwwnenw
nenwsweseswseewneswwseeenwnenese
wswswswswswswswwswnese
swnenwswswnwswnesenwnwseewseneseenwnene
nwsesenesenwnwswesenwnenenesewwnwnwnw
nenwswswwewnwseswwwswneewnewese
enenenenewwneneesesenenewenenenesew
nwnwsenwsenwnenwnesenenenwwesenwwsenesww
nwsenwswswsesesenesewswsesesesesesenene
eeneweenwenwenwsesesweseeswsenw
seseseswswswswswnwwesweswneswnw
seseswsesesenwseswswsee
nwwnwnewwwwwwwewsewnwswnww
seswnewswnwseneseneswseseeswswsenesewse
sewswneswnwswswswswneweswswswswswswswsw
wenesenenenwnwnwnew
nenwnwnwnwnwwweswwenwnwnwnwswnw
seneneswneneneswnwsenwnesenw
wswswnwsweswnwsesewsenwnenewsewsene
eswnwseeseewseenwe
neenwsweswnenewsenwnwswne
wnwewnwwnwswwnwwseewsenwnesenwnw
nwneneenewsesweseneeneneewsenwnenene
sesewsesenewswseseenwseneswswsesesese
wwwsewwnwwnesenwwnwnesewwwnwnww
seseseseswnewseswnewne
neswsweneeneneeneswenwnenenewneenene
wsesesesesesesenwseeseseeseseneswwne
swwenwnwnwenwsenwwwne
swnenenenenenewnenenwneesenwswnenenenene
wswneseneswenewswswnwswneewenwweswe
nenewseenwsenesenwswwsewwnwewsewnw
wseeseeweneswsenweseneesenenwswwsw
neswswneswsewwswwsewsweeneswwnwsw
sesenwswnwseswseeswswseswneseseseswwswsw
seswneseneswwnwnwnesenwseenw
eesesewsewnwnenenenwnweseswwwnee
wneenewnenenwneneseneesenesenwwwsenese
seseseseseeweseswsesewseseesesenwe
nwenwwenwnwnwwnenwnwnwsenw
eseeeneeseneswewseseeesesenweesw
swsewswswnwsewnene
nwnenwneswnwnwnenenenwnwnenenw
eeeseeewseeeseeesee
swnenwswnwnwnenwnenwnwnwnwnwenwnwenwnesw
eeseseeswwneeeesesesesesenewsee
swnenenenesenenwnesenenwnenesenenenenewne
neewwwwewwesewwswwnwwwne
nwsewnwwswwnwwwwswneneswneswnwnwnew
wswswseeneswwwwsweswswswwnenwesw
nwswewnwsenwswnenweewnwnwenwsw
seeenwweseeeeenenweeesenw
swnwwswswswswwswswneswswneesw
sweseneneswwswswewswwwnenwseneswnese
eneenwewseeeeesenwseeeseseswsee
nwnewnwnwsenwnwwneesenwnwnenewnw
newswswwswwsewsewwnene
eneseswenewwneseneneneneswneneneswnene
nweneeseneeeesesweeneeenwewnee
seswsewnesenenwnenwswwwwnweweswnw
nwneseneswswseseseswnewneswseseseesene
eeewseswweswenwesenwsewswswnenwne
nwnwnwnwwnewnwswnwnwsenwnwnwnewenwnw
seseswswsweswwseswswsweswwsesesenwnesw
neeswnwswswswseswswwneswswswseseswswsesw
swswseeneswswswwswswswnwnwseswenwew
swswswseswsweswswewswnwseseswswswnwnee
nwwwnwnenwneneeeenwswnenw
senenwswnwneewsewswwweneeeeesesw
nwswenwsweseneesesewnwwnwseweenenese
swesenwseeneseeseswswseswseeseenwnwse
wneseweweneneswsenwnenenenenwnenwnew
swswswswswswswswnwswwwwswse
neeseseenwsweneneeesenwweewwew
swswswsweeswewnwwewsweswnenwswswnw
enwwneneswnenenwnwnenenenwwneenenenese
nwwwenwwenwwwnwnwnwnwseswnwwnww
sweswwwsweweswswweswwnwswswnenwese
neseseseseseseseseseseseswsese
nwsesenwneseseseseswseese
swneswnwwwwenwwwe
nwnwnwnwnewnwwwnwnwnenenwsenwsesenwnww
nwwswneseseswsenweswswswneswwswwswswsw
neenwsenenenenenenwswswnenenwsenenene
newnwnwnwnwwnweswwewsewenwwnwnwnwnw";
        private const string ProblemTestInput = @"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew";
    }
}