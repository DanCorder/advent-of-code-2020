namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day16
    {
        public static int SolveProblem1()
        {
            var sections = ProblemInput.Split("\n\n");
            var rules = sections[0].SplitToLines()
                .Select(parseRule)
                .ToList();
            var tickets = sections[2].SplitToLines().Skip(1).Select(l => l.Split(",").Select(Int32.Parse).ToList()).ToList();
            var lines = ProblemInput.SplitToLines();

            var badValues = new List<int>();

            foreach (var ticket in tickets)
            {
                foreach (var value in ticket)
                {
                    if (!rules.Any(r => 
                        (value >= r.Item2 && value <= r.Item3) ||
                        (value >= r.Item4 && value <= r.Item5)))
                        badValues.Add(value);
                }
            }
            return badValues.Sum();;
        }

        public static long SolveProblem2()
        {
            var sections = ProblemInput.Split("\n\n");
            var rules = sections[0].SplitToLines()
                .Select(parseRule)
                .ToList();
            var tickets = sections[2].SplitToLines().Skip(1).Select(l => l.Split(",").Select(Int32.Parse).ToList()).ToList();
            var lines = ProblemInput.SplitToLines();
            var myTicket = sections[1].SplitToLines().Skip(1).Single().Split(",").Select(Int32.Parse).ToList();

            var badTickets = new List<List<int>>();

            foreach (var ticket in tickets)
            {
                foreach (var value in ticket)
                {
                    if (!rules.Any(r => 
                        (value >= r.Item2 && value <= r.Item3) ||
                        (value >= r.Item4 && value <= r.Item5)))
                        badTickets.Add(ticket);
                }
            }
            var goodTickets = tickets.Except(badTickets).ToList();
            goodTickets.Add(myTicket);

            var positions = rules.ToDictionary(r => r.Item1, r => new List<int>());

            for (int i = 0; i < rules.Count; i++)
            {
                var possibleValues = goodTickets.Select(t => t[i]);
                var possibleRules = rules.Where(r => possibleValues.All(value =>
                    (value >= r.Item2 && value <= r.Item3) ||
                    (value >= r.Item4 && value <= r.Item5))).ToList();
                
                foreach(var x in possibleRules)
                {
                    positions[x.Item1].Add(i);
                }
            }

            while(positions.Any(p => p.Value.Count > 1))
            {
                var takenPositions = positions.Where(p => p.Value.Count == 1).Select(p => p.Value.Single()).ToList();

                foreach(var p in positions.Where(p => p.Value.Count > 1))
                {
                    positions[p.Key] = p.Value.Except(takenPositions).ToList();
                }
            }
            // foreach (var rule  in rules)
            // {
            //     narrowPositions(rule, positions);

            //     if (positions.Values.Any(v => v.Count > 1))
            //     {
            //         break;
            //     }
            // }
            
            return
                ((long)myTicket[positions["departure location"].Single()]) *
                ((long)myTicket[positions["departure station"].Single()]) *
                ((long)myTicket[positions["departure platform"].Single()]) *
                ((long)myTicket[positions["departure track"].Single()]) *
                ((long)myTicket[positions["departure date"].Single()]) *
                ((long)myTicket[positions["departure time"].Single()]);
        }

        private static void narrowPositions(List<int> ticket, Dictionary<string, List<int>> positions)
        {
            
        }

        private static Tuple<string, int, int, int, int> parseRule(string ruleString)
        {
            var m = rule.Match(ruleString);

            return new Tuple<string, int, int, int, int>(
                m.Groups[1].Value,
                Int32.Parse(m.Groups[2].Value),
                Int32.Parse(m.Groups[3].Value),
                Int32.Parse(m.Groups[4].Value),
                Int32.Parse(m.Groups[5].Value)
            );
        }

        private static Regex rule = new Regex(@"(.*)\: (\d+)\-(\d+) or (\d+)\-(\d+)");

        private const string ProblemInput = @"departure location: 36-363 or 377-962
departure station: 29-221 or 234-953
departure platform: 39-585 or 595-954
departure track: 31-727 or 753-952
departure date: 33-862 or 883-964
departure time: 35-716 or 722-971
arrival location: 32-59 or 74-955
arrival station: 41-330 or 353-963
arrival platform: 28-883 or 894-964
arrival track: 26-669 or 691-974
class: 43-250 or 261-966
duration: 48-521 or 533-974
price: 48-100 or 107-971
route: 47-757 or 777-971
row: 38-629 or 637-961
seat: 43-310 or 330-949
train: 27-560 or 566-957
type: 50-433 or 457-963
wagon: 35-898 or 907-957
zone: 48-354 or 362-961

your ticket:
89,179,173,167,157,127,163,113,137,109,151,131,97,149,107,83,79,139,59,53

nearby tickets:
930,274,273,471,282,613,191,559,820,795,291,215,11,172,813,204,182,218,159,779
81,669,601,495,234,883,59,55,542,924,423,101,610,379,378,135,89,833,514,266
838,481,554,215,129,648,217,990,595,844,100,203,917,417,399,551,715,133,425,897
497,157,897,300,141,622,497,126,755,102,617,269,110,549,843,500,86,659,410,99
423,628,219,414,466,781,425,355,512,700,483,92,542,778,596,160,821,605,296,235
903,620,949,628,285,534,714,619,838,487,834,914,861,815,420,480,854,457,87,301
571,704,828,327,412,930,856,516,648,694,483,399,77,512,568,941,89,182,638,79
91,198,665,617,945,795,799,521,223,432,606,910,602,93,807,848,90,354,237,165
488,135,309,77,857,249,171,11,297,574,712,654,120,240,392,310,823,239,573,798
320,649,362,125,551,408,834,645,98,264,162,309,457,285,644,139,559,666,112,710
280,813,124,91,488,290,836,808,473,533,206,622,389,269,492,908,8,929,603,396
107,288,823,462,215,789,76,408,590,145,381,716,915,693,726,418,781,291,213,141
712,838,172,912,649,57,514,939,923,990,91,783,153,575,382,566,220,385,668,300
121,814,183,380,109,74,654,266,428,777,193,183,533,544,814,120,195,563,216,560
431,172,97,540,310,910,212,697,386,707,671,421,778,484,107,396,208,713,577,74
596,173,850,176,538,533,155,788,125,926,793,720,692,816,651,150,409,482,117,175
213,124,240,714,396,567,366,161,896,172,408,907,723,783,601,935,598,502,656,722
134,124,167,649,856,284,510,400,198,582,521,555,860,127,6,470,657,391,538,583
88,394,912,778,400,717,521,479,913,836,501,820,585,791,170,601,136,625,621,777
416,243,299,194,920,421,843,429,288,76,308,391,292,51,763,786,661,420,832,150
266,585,694,214,515,652,163,406,601,604,291,539,274,167,787,79,897,816,8,486
101,559,123,910,286,553,505,912,556,613,645,922,55,120,931,411,934,289,237,857
841,580,837,493,696,59,154,173,464,460,939,643,865,467,483,408,617,558,606,382
6,638,939,362,270,89,462,533,113,143,858,612,936,663,411,508,271,809,271,203
276,702,653,560,276,567,239,289,127,563,116,567,851,400,461,724,245,132,426,282
299,490,266,129,568,420,817,495,645,423,136,283,50,816,239,695,840,3,946,461
78,283,930,545,191,218,427,830,725,567,329,308,235,600,499,560,792,392,244,306
930,55,659,416,89,913,320,579,660,643,516,949,133,116,389,509,294,827,394,710
562,807,841,494,191,146,289,182,576,50,394,935,600,184,820,298,459,655,123,645
803,89,608,113,679,844,817,617,839,938,946,282,81,266,853,269,89,178,813,151
287,787,513,660,810,580,210,712,646,486,230,165,949,145,791,693,782,862,395,545
157,205,919,142,362,382,922,297,552,382,479,269,666,94,270,184,759,614,791,609
221,180,127,704,500,211,931,320,509,153,469,805,944,189,619,554,497,700,861,107
819,413,710,396,387,919,350,295,727,663,220,708,945,508,154,667,641,546,164,851
684,217,297,177,536,166,164,469,786,600,603,461,77,692,855,211,153,209,838,128
896,244,467,619,498,419,521,798,455,491,119,467,235,310,405,517,515,648,294,700
470,811,605,566,487,870,780,614,261,823,458,419,698,142,502,235,220,188,279,625
615,814,89,467,495,166,53,753,756,476,860,736,552,585,408,516,913,578,605,938
97,691,842,287,201,363,942,509,507,638,779,363,946,930,853,125,97,775,916,813
354,558,542,695,427,132,720,205,618,428,461,112,647,822,846,584,799,272,57,245
476,929,261,694,391,709,533,906,503,709,846,618,235,183,431,462,800,135,725,842
928,197,512,331,568,826,131,406,184,520,283,854,384,896,145,555,468,200,190,617
842,759,579,165,291,479,503,623,500,500,403,617,126,648,307,94,175,405,423,183
131,104,493,620,814,244,543,286,155,579,205,923,171,215,145,143,498,430,381,552
639,898,570,81,363,858,831,546,422,776,300,659,396,170,134,654,817,519,250,852
468,824,469,563,710,85,401,303,412,490,204,459,417,533,363,613,133,537,143,393
122,452,389,831,839,801,556,498,827,211,850,480,856,716,304,89,467,117,267,272
247,546,94,161,428,392,554,403,643,684,508,243,943,755,519,414,113,847,629,473
481,203,304,362,507,163,300,806,482,546,710,875,600,500,426,608,488,241,91,485
220,860,907,789,921,89,387,396,382,784,91,569,528,277,642,109,580,949,81,642
516,895,777,842,166,640,433,147,400,394,897,15,419,861,723,825,691,508,652,148
658,241,203,607,659,759,843,151,616,418,499,825,192,925,196,330,693,848,920,691
128,983,136,521,243,420,172,787,851,809,553,810,198,655,189,77,883,624,572,243
652,843,119,404,537,220,217,195,657,8,925,604,136,704,491,154,572,464,753,819
778,272,56,399,393,858,822,664,796,838,700,313,182,496,302,117,846,624,804,403
93,298,381,161,200,465,825,143,309,571,625,618,949,464,175,768,823,380,598,668
515,211,754,396,239,430,265,831,204,299,948,234,273,57,113,913,125,11,553,694
250,826,779,895,807,704,133,89,622,753,624,372,152,380,149,555,783,627,667,135
426,74,668,697,173,932,784,425,415,598,514,790,655,825,272,855,386,348,584,489
558,580,354,489,669,263,837,618,655,808,583,153,326,84,842,145,111,928,936,661
647,391,551,778,124,381,819,620,368,642,238,491,699,149,715,308,907,613,908,505
756,234,553,131,854,816,496,549,205,783,949,550,741,205,489,573,481,646,287,553
830,191,943,271,157,498,386,266,555,162,259,709,480,489,248,401,911,843,661,779
711,582,387,395,218,108,405,780,124,943,153,556,806,609,77,483,127,584,843,5
707,912,54,393,791,112,707,534,357,665,503,86,811,391,402,414,432,378,214,695
150,78,442,131,149,51,427,131,95,173,130,929,808,268,704,536,238,573,77,714
520,491,896,100,600,631,786,152,927,836,945,465,516,843,937,220,663,202,713,548
51,266,582,497,469,791,11,274,826,654,208,164,946,124,485,846,213,216,479,290
176,830,581,816,363,516,357,695,650,820,246,498,221,803,862,595,778,818,98,856
948,98,698,918,583,701,292,460,221,1,813,661,654,501,462,408,503,382,844,235
421,130,462,983,411,133,949,170,642,206,545,806,171,940,755,144,918,87,556,577
648,215,651,614,463,238,281,653,753,211,293,398,784,919,160,430,929,124,340,96
825,474,380,239,159,943,485,854,166,694,718,114,57,627,799,81,724,823,212,245
167,929,798,579,835,278,591,629,276,919,194,174,221,860,309,280,487,380,79,386
882,182,265,281,384,149,787,603,642,700,723,202,57,652,753,115,221,172,909,919
791,651,330,379,940,806,850,871,482,819,654,945,627,156,156,706,249,813,187,780
213,208,850,263,706,451,804,787,797,819,393,809,836,542,303,246,474,477,651,610
247,485,577,827,615,640,86,365,647,405,928,74,850,207,712,147,613,644,176,660
207,703,486,11,723,416,655,936,807,154,165,503,556,302,934,175,504,282,485,420
640,85,458,490,577,271,202,662,694,293,378,999,485,430,55,428,826,575,193,190
295,820,247,458,205,814,727,658,480,129,451,433,613,644,267,652,553,294,809,285
844,149,416,508,711,397,296,330,216,939,485,645,62,417,161,87,726,160,165,575
279,816,570,240,929,808,851,309,637,268,463,120,661,14,716,272,569,779,585,263
883,502,363,176,221,357,623,834,611,174,202,547,307,581,464,388,474,100,215,663
76,429,157,469,818,512,134,594,403,596,310,938,140,856,491,208,567,840,701,495
491,927,809,195,479,538,932,554,934,911,423,716,154,814,637,97,97,0,168,397
481,51,769,810,848,119,797,554,89,53,155,629,509,578,664,108,275,622,457,854
603,250,550,192,785,89,143,145,900,571,546,429,841,77,158,578,654,726,839,478
144,927,802,765,793,77,553,459,549,388,427,608,149,202,167,395,554,754,938,647
358,521,143,854,151,86,709,940,716,637,693,167,639,945,855,458,97,796,161,419
791,927,220,667,757,949,263,122,815,581,404,509,369,793,580,149,809,487,646,584
176,753,918,58,918,566,646,239,615,816,993,383,135,534,95,81,627,663,815,502
307,493,249,627,512,537,462,836,286,19,79,819,501,94,659,856,243,267,94,221
295,896,554,57,620,409,542,545,340,782,566,833,402,384,539,600,780,597,498,802
306,76,727,695,713,427,215,562,123,610,269,95,186,423,84,657,710,428,200,291
220,140,723,913,779,909,621,941,883,660,415,291,390,910,563,301,520,159,848,540
459,166,653,74,849,110,276,945,856,396,369,709,178,134,161,575,243,463,558,727
852,793,267,713,662,184,656,235,488,945,795,388,717,639,616,710,244,606,854,295
699,541,839,161,418,902,306,281,707,940,219,479,849,199,461,383,777,94,279,582
51,240,607,819,392,501,299,392,710,535,938,118,677,75,691,928,287,79,661,936
194,420,84,828,781,942,924,569,667,723,984,707,916,462,948,839,911,150,269,860
363,333,646,428,568,538,582,691,912,394,580,377,848,363,286,147,841,908,141,382
850,627,550,472,177,112,602,714,319,785,498,160,942,805,472,571,610,402,427,826
95,533,203,214,399,224,895,430,692,858,84,584,165,550,538,96,429,846,464,280
460,209,152,380,437,643,175,416,945,411,712,385,144,568,669,726,788,846,212,57
139,286,291,406,521,426,694,184,910,851,897,112,668,392,521,106,78,897,199,188
81,942,201,569,579,142,112,836,558,165,624,816,803,151,671,112,274,798,459,382
691,472,477,715,115,591,144,803,139,488,249,664,398,188,204,176,911,544,494,846
432,480,822,693,240,476,753,744,810,520,495,411,463,655,168,796,921,722,843,938
838,943,579,629,126,465,78,388,564,795,554,98,201,806,398,804,204,919,239,602
423,557,706,140,489,213,429,585,538,574,558,692,634,603,814,219,115,273,51,138
836,711,115,693,487,117,694,926,609,939,413,198,404,647,485,978,577,291,845,911
159,417,599,786,716,933,418,280,852,734,622,704,467,709,395,650,390,509,546,519
78,693,911,939,104,755,264,577,296,709,483,537,782,894,832,218,780,211,605,55
159,425,946,599,917,309,691,97,704,269,132,562,507,330,791,853,757,406,217,176
20,432,170,916,935,653,942,781,661,273,555,131,399,379,189,80,393,665,922,473
136,584,190,130,52,562,707,599,176,803,264,930,433,484,644,930,421,497,396,267
200,714,90,264,363,431,597,20,126,431,132,622,281,520,133,128,354,285,755,535
759,509,650,942,543,638,839,617,190,725,279,276,432,155,647,851,171,141,936,757
126,724,396,786,916,184,159,581,646,264,264,841,922,928,399,383,641,706,693,17
148,810,202,304,774,303,304,911,264,127,460,579,516,779,401,406,831,310,722,220
400,502,680,585,617,217,249,601,194,580,663,856,622,664,57,915,301,557,669,611
420,576,166,403,158,9,788,569,414,430,930,818,280,51,643,89,427,754,506,849
216,266,695,195,50,485,110,694,460,843,176,280,105,583,535,552,600,940,502,269
799,842,841,714,908,537,692,806,608,839,562,823,810,457,577,218,240,934,396,391
715,395,855,669,509,556,292,86,707,498,239,697,76,12,784,248,286,193,299,622
618,418,723,894,638,169,481,494,421,601,545,829,398,790,458,722,515,697,675,115
600,690,912,306,265,86,276,429,155,79,826,147,392,692,558,465,811,645,491,942
932,792,778,640,100,168,268,830,982,269,198,81,517,421,296,237,284,462,201,429
497,533,275,536,562,659,159,820,936,122,656,559,789,248,534,144,628,798,692,665
170,610,992,138,780,395,267,567,214,156,417,143,192,619,195,710,915,502,245,642
75,476,204,461,240,610,84,267,661,123,175,904,606,276,221,280,280,277,184,895
434,603,604,262,77,288,816,268,270,267,389,476,168,178,75,497,575,462,382,663
13,98,568,638,413,520,519,108,558,294,855,599,595,574,274,941,612,948,284,428
578,433,414,487,390,219,700,218,898,572,121,485,465,545,570,690,250,546,214,142
367,796,281,782,210,291,160,402,295,238,911,783,283,171,848,552,430,915,503,202
945,212,262,118,484,940,396,815,139,866,239,649,619,800,781,213,156,494,578,467
91,202,925,645,621,57,852,709,869,184,645,415,509,712,379,293,618,136,466,136
855,656,470,99,600,862,445,201,154,711,400,143,544,400,496,151,566,120,845,664
819,647,800,799,281,89,232,175,490,165,191,89,109,713,234,153,911,408,387,138
197,547,895,145,148,908,578,350,667,143,459,142,107,666,248,140,663,647,927,828
610,506,122,146,725,575,214,936,217,468,637,994,552,394,109,307,637,755,691,205
111,214,398,860,938,728,582,668,485,944,158,285,200,77,835,390,828,203,575,538
724,925,570,96,920,581,193,826,852,87,155,186,468,938,650,378,183,906,58,213
518,832,628,545,265,576,813,605,192,806,533,629,358,399,838,918,203,508,179,396
181,399,841,421,619,920,74,431,142,493,866,95,608,613,94,384,461,378,779,171
699,564,50,147,713,547,810,133,797,430,502,244,212,945,629,414,502,198,847,932
754,585,521,267,297,820,167,677,95,789,697,426,157,691,664,615,59,261,856,122
206,621,858,643,53,350,810,216,811,931,781,814,895,848,266,396,494,145,420,187
606,640,388,99,302,492,539,123,382,351,465,656,246,484,156,155,607,378,197,920
79,498,611,190,193,412,52,58,552,441,206,613,50,797,824,821,214,58,919,568
94,615,390,595,350,898,566,598,519,795,539,790,50,175,422,246,147,295,912,218
405,854,179,504,178,660,660,174,414,157,387,187,56,516,268,235,573,502,772,787
642,822,296,755,247,472,546,821,695,262,840,987,202,115,282,169,571,95,834,288
468,361,204,477,378,919,91,354,150,521,837,288,836,710,918,517,77,131,77,614
397,290,285,663,267,928,501,816,701,603,588,149,520,646,581,472,623,812,135,275
658,480,715,901,556,918,935,712,600,779,848,798,571,936,82,936,638,50,195,580
930,859,648,213,668,830,581,463,859,859,789,267,399,852,507,85,617,102,810,265
277,134,280,261,183,297,158,477,157,554,830,592,668,192,490,579,277,577,268,421
801,265,220,466,605,482,755,388,392,147,113,182,479,848,102,644,414,141,134,280
659,218,940,135,513,509,865,701,250,566,842,175,151,110,655,829,214,705,813,596
856,250,53,282,509,693,900,204,845,188,941,814,669,920,85,624,824,140,839,109
797,669,693,579,629,985,597,161,194,569,199,187,797,503,860,301,666,815,552,184
131,236,195,238,830,613,780,245,146,600,201,143,512,549,511,498,167,354,441,460
292,794,895,304,118,949,163,783,768,141,270,471,612,724,301,383,172,912,268,835
534,268,208,726,83,177,412,78,918,376,396,426,403,694,658,598,614,843,657,694
283,571,897,367,541,911,830,693,188,818,623,80,698,165,108,778,91,122,209,51
571,937,210,567,515,497,401,293,103,158,542,189,795,466,144,362,295,599,295,805
781,107,103,212,287,188,815,666,804,624,423,82,142,248,713,695,176,242,926,850
93,3,362,654,835,284,210,382,512,484,472,385,165,125,283,560,250,811,782,496
779,912,80,394,614,292,257,859,568,642,113,584,898,240,400,791,171,115,787,93
800,236,814,924,909,568,106,786,642,388,784,514,457,843,133,475,117,859,245,132
621,489,236,359,504,398,310,264,569,827,883,786,159,919,580,242,238,705,662,115
354,790,858,852,323,646,465,907,697,245,300,151,389,790,99,804,805,489,156,637
620,488,624,816,548,237,478,813,53,11,511,200,610,84,500,926,894,645,208,546
193,389,423,138,610,944,59,238,475,202,348,125,479,175,919,642,157,294,492,426
82,190,898,201,497,421,261,596,263,140,84,174,702,237,131,795,607,171,246,0
836,534,303,74,923,944,862,104,820,201,600,207,405,133,93,661,849,397,153,398
234,413,84,931,646,649,569,249,247,482,521,550,87,627,576,570,471,895,22,709
697,380,95,413,211,158,173,753,478,279,660,855,898,946,212,992,541,666,854,861
785,597,384,945,205,829,430,330,281,294,298,416,57,768,800,273,822,703,508,833
306,939,521,829,144,629,827,447,140,598,175,710,544,150,819,407,407,703,236,819
295,460,520,898,469,241,640,413,745,915,133,809,658,827,549,154,702,826,310,171
700,566,788,214,240,815,724,638,109,193,377,770,597,917,598,385,180,263,554,416
551,397,847,417,426,298,919,639,932,566,566,444,930,551,243,620,470,100,847,911
596,182,651,573,407,24,276,939,273,608,136,130,420,279,389,431,427,597,186,541
545,279,478,820,272,192,565,507,777,56,709,237,93,77,51,278,932,491,693,851
848,458,79,281,663,508,786,929,297,597,98,612,588,461,538,858,486,649,848,668
834,706,703,175,120,603,389,381,387,246,492,671,835,111,306,597,214,802,828,596
696,124,818,128,152,302,262,596,978,128,710,398,55,309,167,144,808,117,620,396
90,160,56,77,923,933,432,411,642,311,520,852,186,575,175,120,576,432,706,381
399,89,284,53,182,428,650,767,797,502,396,272,703,53,669,151,601,533,239,124
939,87,569,106,392,939,287,706,493,74,821,247,131,214,262,192,925,941,611,182
504,467,387,787,703,796,584,547,780,617,941,602,785,492,582,910,670,214,857,622
639,935,478,294,396,127,769,620,463,237,218,830,823,931,303,413,74,583,502,174
652,578,196,153,913,293,646,164,675,490,123,483,512,595,597,805,516,924,570,940
553,407,831,310,90,809,90,936,942,709,937,795,258,493,829,836,289,386,80,94
841,176,306,238,478,554,123,410,726,390,943,547,765,753,159,923,657,468,725,576
469,76,362,627,551,327,192,945,941,666,249,304,132,559,158,129,390,615,554,269
171,692,571,128,163,214,408,289,282,497,146,519,345,203,838,567,461,612,581,625
392,164,564,79,536,278,401,466,287,638,571,595,824,611,426,858,81,812,855,835
239,901,238,295,300,484,822,109,197,279,407,555,518,301,808,506,170,640,142,408
605,419,379,518,176,427,191,298,647,199,693,828,829,466,2,486,142,77,393,187
486,479,100,132,460,935,733,116,429,514,168,401,133,89,663,913,691,603,723,59
147,990,816,700,246,585,704,432,508,914,378,508,546,661,854,178,567,923,293,839
389,166,694,560,133,675,388,795,51,298,793,147,468,833,581,80,910,243,699,781
213,425,691,74,485,375,405,624,579,245,936,703,580,133,806,267,262,78,828,401
351,116,894,644,285,548,810,930,409,114,298,843,378,205,205,849,298,597,387,166
796,496,599,627,85,149,76,171,499,797,545,126,203,695,150,484,533,996,860,471
807,647,643,859,903,292,568,484,197,489,381,247,940,926,854,854,117,568,521,88
309,824,57,289,96,104,200,602,625,516,470,58,108,629,88,114,306,664,74,493
824,59,155,686,786,139,517,714,654,712,151,818,378,191,780,799,712,76,792,152
126,839,911,459,600,392,181,546,416,404,987,511,653,418,426,576,792,780,81,272
462,519,489,926,172,794,381,781,121,623,359,713,295,397,304,491,846,833,393,612
662,601,214,475,117,207,820,832,407,234,834,404,437,310,267,307,428,613,278,500
803,511,291,175,699,116,614,541,604,795,632,896,292,214,606,848,294,490,264,78
181,936,705,544,667,542,559,154,113,814,830,278,390,77,550,908,429,414,562,418
518,473,303,605,355,625,547,136,275,549,786,126,933,544,653,646,639,947,57,304
422,666,425,649,521,216,611,539,717,210,610,605,547,156,391,394,923,125,292,128
486,480,548,145,397,363,207,98,543,581,623,133,611,197,701,142,309,677,604,492
182,790,933,247,429,136,930,652,158,558,124,187,903,99,648,533,789,467,362,709
605,836,117,932,610,283,215,302,2,701,490,204,810,459,814,724,210,53,641,930
782,424,792,895,175,567,212,237,557,698,585,83,227,820,485,483,457,160,724,272
134,555,291,581,600,704,614,543,607,814,102,894,616,553,859,501,580,656,275,781
377,152,117,451,708,291,485,649,306,919,198,803,300,516,495,163,193,354,290,781
247,492,805,119,783,516,295,917,708,55,235,811,565,281,618,512,853,215,789,203
782,667,805,83,204,515,415,127,155,857,13,516,409,921,394,52,401,469,705,949
659,169,845,504,831,609,676,308,584,646,199,924,575,538,390,940,692,929,422,560
620,148,89,942,850,167,833,599,290,411,531,847,611,921,91,211,295,788,170,219
115,363,539,788,144,500,613,262,898,93,501,53,976,389,585,917,509,309,812,392
650,148,460,264,120,661,932,99,171,900,693,551,883,858,696,457,856,664,910,241
696,128,942,81,215,716,292,517,937,91,769,133,56,93,543,88,501,568,289,790
574,271,857,937,138,12,88,894,94,612,138,714,511,206,126,276,824,235,815,574
220,404,242,118,883,387,942,794,499,110,269,348,944,204,793,292,537,691,553,481
162,462,836,795,142,910,129,389,82,361,353,145,399,515,583,607,476,169,166,74
495,551,948,517,482,947,408,486,726,469,729,583,88,937,698,173,408,171,216,75
431,938,207,307,555,266,533,511,827,923,393,608,130,916,917,996,131,118,724,638
777,573,475,623,149,76,704,197,391,755,992,639,486,245,205,501,785,813,265,573
640,150,287,627,205,52,159,781,392,519,832,52,571,379,412,397,662,450,509,806
549,483,80,433,511,470,426,655,57,791,901,927,839,911,288,568,119,120,180,619
610,433,466,134,895,658,278,626,550,282,944,622,468,86,851,405,108,544,101,234
197,664,426,568,583,829,636,158,854,396,211,642,57,78,808,619,858,709,940,637
820,219,182,271,99,280,602,817,79,99,248,358,896,433,598,281,578,498,297,826
895,847,201,980,911,75,133,667,390,512,804,94,861,707,918,193,471,795,262,907
652,478,535,849,804,502,491,110,806,810,149,517,827,834,235,124,265,709,845,989";
        private const string ProblemTestInput = @"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12";

        private const string ProblemTestInput2 = @"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9";
    }
}