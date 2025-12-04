namespace AdventOfCode2025
{
    internal class D04 : Day
    {
        internal override void SolvePart1()
        {
            var map = FileHelper.ReadLinesAsCharMap("D04.txt");

            long result = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] != '@')
                    {
                        continue;
                    }
                    var p = new Position(j, i);
                    var adjacentPositions = MatrixHelper.GetAdjacentPositions(p, map[i].Length, map.Length);
                    var adjacentRolls = adjacentPositions.Where(pos => map[pos.Y][pos.X] == '@').Count();
                    if (adjacentRolls < 4)
                    {
                        result++;
                    }
                }
            }

            Console.WriteLine(result);
        }

        internal override void SolvePart2()
        {
            var map = FileHelper.ReadLinesAsCharMap("D04.txt");

            long total = 0;
            long removedRolls;
            do
            {
                removedRolls = 0;
                var rollPositions = new List<Position>();
                for (int i = 0; i < map.Length; i++)
                {
                    for (int j = 0; j < map[i].Length; j++)
                    {
                        if (map[i][j] != '@')
                        {
                            continue;
                        }
                        var p = new Position(j, i);
                        var adjacentPositions = MatrixHelper.GetAdjacentPositions(p, map[i].Length, map.Length);
                        var adjacentRolls = adjacentPositions.Where(pos => map[pos.Y][pos.X] == '@').Count();
                        if (adjacentRolls < 4)
                        {
                            rollPositions.Add(p);
                            removedRolls++;
                        }
                    }
                }
                if (removedRolls > 0)
                {
                    total += removedRolls;
                    foreach (var p in rollPositions)
                    {
                        map[p.Y][p.X] = '.';
                    }
                }
            } while (removedRolls > 0);

            Console.WriteLine(total);
        }
    }
}
