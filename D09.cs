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

            var areas = MatrixHelper.GetAllAreas(points);
            var orderedAreas = areas.OrderByDescending(kv => kv.Value).ToArray();

            foreach (var kv in orderedAreas)
            {
                int minX = Math.Min(kv.Key.Item1.X, kv.Key.Item2.X);
                int maxX = Math.Max(kv.Key.Item1.X, kv.Key.Item2.X);
                int minY = Math.Min(kv.Key.Item1.Y, kv.Key.Item2.Y);
                int maxY = Math.Max(kv.Key.Item1.Y, kv.Key.Item2.Y);

                // Check all 4 corners are inside the polygon
                bool cornersInside =
                    IsInside(new Position(minX, minY), points) &&
                    IsInside(new Position(maxX, minY), points) &&
                    IsInside(new Position(maxX, maxY), points) &&
                    IsInside(new Position(minX, maxY), points);

                if (!cornersInside)
                    continue;

                // Check no polygon edge crosses any rectangle edge
                bool noIntersection = !PolygonIntersectsRectangle(points, minX, maxX, minY, maxY);

                if (noIntersection)
                {
                    Console.WriteLine(kv.Value);
                    break;
                }
            }
        }

        private static bool PolygonIntersectsRectangle(Position[] polygon, int minX, int maxX, int minY, int maxY)
        {
            int n = polygon.Length;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                var a = polygon[j];
                var b = polygon[i];

                // Check if this polygon edge crosses any of the 4 rectangle edges
                // Top edge (y = minY, x from minX to maxX)
                if (SegmentsIntersect(a, b, minX, maxX, minY, maxY, isTopOrBottom: true, y: minY))
                    return true;
                // Bottom edge (y = maxY)
                if (SegmentsIntersect(a, b, minX, maxX, minY, maxY, isTopOrBottom: true, y: maxY))
                    return true;
                // Left edge (x = minX, y from minY to maxY)
                if (SegmentsIntersect(a, b, minX, maxX, minY, maxY, isTopOrBottom: false, x: minX))
                    return true;
                // Right edge (x = maxX)
                if (SegmentsIntersect(a, b, minX, maxX, minY, maxY, isTopOrBottom: false, x: maxX))
                    return true;
            }
            return false;
        }

        private static bool SegmentsIntersect(Position a, Position b, int minX, int maxX, int minY, int maxY,
            bool isTopOrBottom, int x = 0, int y = 0)
        {
            if (isTopOrBottom)
            {
                // Horizontal rectangle edge at y, from minX to maxX
                // Check if polygon segment (a,b) crosses it (strictly inside, not at corners)
                if ((a.Y < y && b.Y > y) || (a.Y > y && b.Y < y))
                {
                    // Find x intersection point
                    double xIntersect = a.X + (double)(b.X - a.X) * (y - a.Y) / (b.Y - a.Y);
                    return xIntersect > minX && xIntersect < maxX;
                }
            }
            else
            {
                // Vertical rectangle edge at x, from minY to maxY
                if ((a.X < x && b.X > x) || (a.X > x && b.X < x))
                {
                    double yIntersect = a.Y + (double)(b.Y - a.Y) * (x - a.X) / (b.X - a.X);
                    return yIntersect > minY && yIntersect < maxY;
                }
            }
            return false;
        }

        private static bool IsInside(Position point, Position[] polygon)
        {
            int n = polygon.Length;
            bool inside = false;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                // Check if on edge
                if (IsOnSegment(point, polygon[i], polygon[j]))
                    return true;

                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                    (point.X < (double)(polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        private static bool IsOnSegment(Position point, Position a, Position b)
        {
            if (a.Y == b.Y && point.Y == a.Y)
                return point.X >= Math.Min(a.X, b.X) && point.X <= Math.Max(a.X, b.X);
            if (a.X == b.X && point.X == a.X)
                return point.Y >= Math.Min(a.Y, b.Y) && point.Y <= Math.Max(a.Y, b.Y);
            return false;
        }
    }
}
