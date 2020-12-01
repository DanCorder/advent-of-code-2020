namespace advent_of_code_2020
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day01
    {
        private const string ProblemInput = @"1779
1737
1537
1167
1804
1873
1894
1446
1262
1608
1430
1421
1826
1718
1888
1314
1844
248
1812
1627
1605
1641
1126
1051
1839
1067
1685
1800
1383
1415
1781
1372
1711
1687
1357
1603
1899
1856
1240
1872
1483
1624
1358
1168
1238
1585
1159
1409
1615
1258
1412
1468
1912
1840
1681
1700
1031
1757
1911
1096
1239
1331
1881
1304
1694
1705
1680
820
1744
1245
1922
1545
1335
1852
1318
1565
1505
1535
1536
1758
1508
1453
1957
1375
1647
1531
1261
1202
1701
1562
1933
1293
1828
334
1714
1931
1385
1294
1469
1629
1842
1730
1534
1544
1946
1805
1188
1684
1875
1623
1248
1347
2006
1191
1037
1387
1903
1746
16
952
1246
384
1518
1738
1269
1747
1423
1764
1666
1999
1776
1673
1350
1698
2004
1235
1719
1131
1671
1334
1556
1299
1569
1523
1655
1189
1023
1264
1821
1639
1114
1391
1154
1225
1906
1481
1728
1309
1682
1662
1017
1952
1948
2010
1809
1394
1039
1493
1509
1628
1401
1515
1497
1164
1829
1452
1706
1919
1831
1643
1849
1558
1162
1328
1432
680
1169
1393
1646
1161
1104
1678
1477
1824
1353
1260
1736
1777
1658
1715";
        private const string ProblemTestInput = @"1721
979
366
299
675
1456";

        public static int SolveProblem1()
        {
            var numbers = ProblemInput
                .SplitToLines()
                .Select(Int32.Parse)
                .ToList();

            for (var i = 0; i < numbers.Count; i++)
            {
                for (var j = i+1; j < numbers.Count; j++)
                {
                    if (numbers[i] + numbers[j] == 2020)
                    {
                        return numbers[i] * numbers[j];
                    }
                }
            }
            return -1;
        }

        public static int SolveProblem2()
        {
            var numbers = ProblemInput
                .SplitToLines()
                .Select(Int32.Parse)
                .ToList();

            for (var i = 0; i < numbers.Count; i++)
            {
                for (var j = i+1; j < numbers.Count; j++)
                {
                    for (var k = j+1; k < numbers.Count; k++)
                    {
                        if (numbers[i] + numbers[j] + numbers[k] == 2020)
                        {
                            return numbers[i] * numbers[j] * numbers[k];
                        }
                    }
                }
            }
            return -1;
        }
    }
}