
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
            var orderedAreas = areas.OrderByDescending(kv => kv.Value).ToList();

            foreach (var kv in orderedAreas)
            {
                var sortedPair = new[] { new Position(kv.Key.Item1.X, kv.Key.Item1.Y), new Position(kv.Key.Item2.X, kv.Key.Item2.Y) }.OrderBy(p => p.X).ThenBy(p => p.Y).ToArray();
                if (sortedPair[0].Y > sortedPair[1].Y)
                {
                    (sortedPair[1].Y, sortedPair[0].Y) = (sortedPair[0].Y, sortedPair[1].Y);
                }

                if (IsRectangleInPolygon(sortedPair[0], sortedPair[1], points))
                {
                    Console.WriteLine(kv.Value);
                    break;
                }
            }
        }

        private static bool IsRectangleInPolygon(Position pointA, Position pointB, Position[] points)
        {
            return false;
        }
    }
}
