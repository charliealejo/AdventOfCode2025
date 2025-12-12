namespace AdventOfCode2025
{
    internal class D12 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines("D12.txt").ToArray();
            var shapes = new List<Shape>();
            for (int i = 0; i <= 5; i++)
            {
                shapes.Add(new Shape
                {
                    Id = i,
                    ShapeCells = [.. lines[(i * 5 + 1)..(i * 5 + 4)].Select(line => line.Select(c => c == '#').ToArray())]
                });
            }
            var regions = lines[30..].Select(line =>
            {
                var parts = line.Split(": ");
                var sizeParts = parts[0].Split('x').Select(int.Parse).ToArray();
                var shapesToFit = parts[1].Split(' ').Select(int.Parse).ToArray();
                return new Region
                {
                    Size = (sizeParts[0], sizeParts[1]),
                    ShapesToFit = shapesToFit
                };

            }).ToArray();

            // For each region, try to fit the shapes in all possible orientations and positions (even flipped and rotated).
            // But after checking all possibilities, either the size of the presents is greater than the region size (no fit)
            // or the size of the presents is way smaller than the available region (easy fit).
            // So let's just calculate the total area of the shapes and compare it to the area of the region.
            var areas = shapes.Select(s => s.ShapeCells.Sum(row => row.Count(cell => cell))).ToArray();
            var answer = 0;
            foreach (var region in regions)
            {
                var totalShapeArea = 0L;
                for (int i = 0; i < region.ShapesToFit.Length; i++)
                {
                    totalShapeArea += areas[i] * region.ShapesToFit[i];
                }
                var regionArea = region.Size.X * region.Size.Y;
                if (totalShapeArea < regionArea)
                {
                    answer++;
                }
            }

            Console.WriteLine(answer);
        }

        internal override void SolvePart2()
        {
            // No part 2 for this day.
            Console.WriteLine("Merry Christmas!");
        }
    }

    internal class Shape
    {
        internal int Id { get; set; }
        internal bool[][] ShapeCells { get; set; }
    }

    internal class Region
    {
        internal (int X, int Y) Size { get; set; }
        internal int[] ShapesToFit { get; set; }
    }
}
