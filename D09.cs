namespace AdventOfCode2025
{
    internal class D09 : Day
    {
        internal override void SolvePart1()
        {
            var points = FileHelper.ReadLinesAs2DPoints("D09.txt", ",");

            var areas = MatrixHelper.GetAllAreas(points);
            var orderedAreas = areas.OrderByDescending(kv => kv.Value).ToList();

            Console.WriteLine(orderedAreas[0].Value);
        }

        internal override void SolvePart2()
        {
            var points = FileHelper.ReadLinesAs2DPoints("D09.txt", ",");

            var allowedX = points.Select(p => p.X).Distinct().Order().ToArray();
            var allowedY = points.Select(p => p.Y).Distinct().Order().ToArray();

            var compressedPoints = points
                .Select(p => new Position(Array.IndexOf(allowedX, p.X) * 2 + 1, Array.IndexOf(allowedY, p.Y) * 2 + 1))
                .ToArray();

            var map = BuildMap(compressedPoints, allowedX.Length * 2 + 1, allowedY.Length * 2 + 1);
            FillInterior(map);

            var prefixSum = BuildPrefixSum(map);
            var bestRectangle = FindLargestFilledRectangle(compressedPoints, prefixSum);

            long realArea = CalculateRealArea(bestRectangle, allowedX, allowedY);
            Console.WriteLine(realArea);
        }

        private static bool[,] BuildMap(Position[] compressedPoints, int width, int height)
        {
            var map = new bool[width, height];
            for (int i = 0; i < compressedPoints.Length; i++)
            {
                for (int j = i + 1; j < compressedPoints.Length; j++)
                {
                    var (p1, p2) = (compressedPoints[i], compressedPoints[j]);
                    if (p1.X == p2.X)
                        for (int y = Math.Min(p1.Y, p2.Y); y <= Math.Max(p1.Y, p2.Y); y++)
                            map[p1.X, y] = true;
                    else if (p1.Y == p2.Y)
                        for (int x = Math.Min(p1.X, p2.X); x <= Math.Max(p1.X, p2.X); x++)
                            map[x, p1.Y] = true;
                }
            }
            return map;
        }

        private static void FillInterior(bool[,] map)
        {
            var outsideArea = FloodFill(map, new Position(0, 0));
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    if (!map[x, y] && !outsideArea.Contains(new Position(x, y)))
                        map[x, y] = true;
        }

        private static int[,] BuildPrefixSum(bool[,] map)
        {
            var prefixSum = new int[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    prefixSum[x, y] = (x > 0 ? prefixSum[x - 1, y] : 0)
                                    + (y > 0 ? prefixSum[x, y - 1] : 0)
                                    - (x > 0 && y > 0 ? prefixSum[x - 1, y - 1] : 0)
                                    + (map[x, y] ? 1 : 0);
                }
            }
            return prefixSum;
        }

        private static (Position, Position) FindLargestFilledRectangle(Position[] compressedPoints, int[,] prefixSum)
        {
            long maxArea = 0;
            (Position, Position) best = (new(0, 0), new(0, 0));

            for (int i = 0; i < compressedPoints.Length; i++)
            {
                for (int j = i + 1; j < compressedPoints.Length; j++)
                {
                    var (p1, p2) = (compressedPoints[i], compressedPoints[j]);
                    int x1 = Math.Min(p1.X, p2.X), y1 = Math.Min(p1.Y, p2.Y);
                    int x2 = Math.Max(p1.X, p2.X), y2 = Math.Max(p1.Y, p2.Y);

                    long areaInside = QueryPrefixSum(prefixSum, x1, y1, x2, y2);
                    long area = (long)(x2 - x1 + 1) * (y2 - y1 + 1);

                    if (areaInside == area && area > maxArea)
                    {
                        maxArea = area;
                        best = (p1, p2);
                    }
                }
            }
            return best;
        }

        private static long QueryPrefixSum(int[,] ps, int x1, int y1, int x2, int y2) =>
            ps[x2, y2]
            - (x1 > 0 ? ps[x1 - 1, y2] : 0)
            - (y1 > 0 ? ps[x2, y1 - 1] : 0)
            + (x1 > 0 && y1 > 0 ? ps[x1 - 1, y1 - 1] : 0);

        private static long CalculateRealArea((Position p1, Position p2) rect, int[] allowedX, int[] allowedY)
        {
            int realWidth = Math.Abs(allowedX[rect.p1.X / 2] - allowedX[rect.p2.X / 2]) + 1;
            int realHeight = Math.Abs(allowedY[rect.p1.Y / 2] - allowedY[rect.p2.Y / 2]) + 1;
            return (long)realWidth * realHeight;
        }

        private static HashSet<Position> FloodFill(bool[,] map, Position start)
        {
            var visited = new HashSet<Position>();
            var toVisit = new Queue<Position>();
            toVisit.Enqueue(start);
            int width = map.GetLength(0), height = map.GetLength(1);
            Position[] directions = [new(1, 0), new(-1, 0), new(0, 1), new(0, -1)];

            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                if (!visited.Add(current)) continue;

                foreach (var dir in directions)
                {
                    var neighbor = new Position(current.X + dir.X, current.Y + dir.Y);
                    if (neighbor.X >= 0 && neighbor.X < width && neighbor.Y >= 0 && neighbor.Y < height
                        && !map[neighbor.X, neighbor.Y] && !visited.Contains(neighbor))
                    {
                        toVisit.Enqueue(neighbor);
                    }
                }
            }
            return visited;
        }
    }
}
