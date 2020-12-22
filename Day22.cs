namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day22
    {
        public static long SolveProblem1()
        {
            var decks = ProblemInput.Split("Player 2:");
            var deck1 = decks[0].SplitToLines().Skip(1).SkipLast(1).Select(Int64.Parse).ToList();
            var deck2 = decks[1].SplitToLines().Skip(1).Select(Int64.Parse).ToList();

            while (deck1.Count > 0 && deck2.Count > 0)
            {
                var c1 = deck1[0];
                var c2 = deck2[0];
                deck1.RemoveAt(0);
                deck2.RemoveAt(0);

                if (c1 > c2)
                {
                    deck1.Add(c1);
                    deck1.Add(c2);
                }
                else
                {
                    deck2.Add(c2);
                    deck2.Add(c1);
                }
            }

            var winner = deck1.Count > 0 ? deck1 : deck2;
            winner.Reverse();

            return winner.Select((c, i) => c*(i+1)).Sum();
        }

        public static long SolveProblem2()
        {
            var decks = ProblemInput.Split("Player 2:");
            var deck1 = decks[0].SplitToLines().Skip(1).SkipLast(1).Select(Int64.Parse).ToList();
            var deck2 = decks[1].SplitToLines().Skip(1).Select(Int64.Parse).ToList();

            var winner = Player1WinsGame(deck1, deck2) ? deck1 : deck2;

            winner.Reverse();

            return winner.Select((c, i) => c*(i+1)).Sum();
        }

        private static bool Player1WinsGame(List<long> deck1, List<long> deck2)
        {
            var prev = new HashSet<Tuple<List<long>, List<long>>>();
            while (deck1.Count > 0 && deck2.Count > 0)
            {
                if (prev.Any(p => p.Item1.SequenceEqual(deck1) && p.Item2.SequenceEqual(deck2)))
                    return true;
                
                prev.Add(new Tuple<List<long>, List<long>>(new List<long>(deck1), new List<long>(deck2)));
                var c1 = deck1[0];
                var c2 = deck2[0];
                deck1.RemoveAt(0);
                deck2.RemoveAt(0);

                if (c1 <= deck1.Count && c2 <= deck2.Count)
                {
                    var newDeck1 = deck1.Take((int)c1).ToList();
                    var newDeck2 = deck2.Take((int)c2).ToList();

                    if (Player1WinsGame(newDeck1, newDeck2))
                    {
                        deck1.Add(c1);
                        deck1.Add(c2);
                    }
                    else
                    {
                        deck2.Add(c2);
                        deck2.Add(c1);
                    }
                }
                else if (c1 > c2)
                {
                    deck1.Add(c1);
                    deck1.Add(c2);
                }
                else
                {
                    deck2.Add(c2);
                    deck2.Add(c1);
                }
            }

            return deck1.Count > 0;
        }

        private const string ProblemInput = @"Player 1:
21
22
33
29
43
35
8
30
50
44
9
42
45
16
12
4
15
27
20
31
25
47
5
24
19

Player 2:
3
40
37
14
1
13
49
41
28
48
18
7
23
38
32
34
46
39
17
2
11
6
10
36
26";
        private const string ProblemTestInput = @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";
    }
}