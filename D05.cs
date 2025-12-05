namespace AdventOfCode2025
{
    internal class D05 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines("D05.txt");
            var ranges = lines.Where(l => l.Contains('-'))
                              .Select(l => l.Split('-'))
                              .Select(parts => (long.Parse(parts[0]), long.Parse(parts[1])))
                              .ToArray();
            var ingredients = lines.Where(l => !string.IsNullOrEmpty(l) && !l.Contains('-'))
                                   .Select(long.Parse)
                                   .ToArray();

            long count = 0;
            foreach (var ingredient in ingredients)
            {
                foreach (var (min, max) in ranges)
                {
                    if (ingredient >= min && ingredient <= max)
                    {
                        count++;
                        break;
                    }
                }
            }

            Console.WriteLine(count);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines("D05.txt");
            var ranges = lines.Where(l => l.Contains('-'))
                              .Select(l => l.Split('-'))
                              .Select(parts => (long.Parse(parts[0]), long.Parse(parts[1])))
                              .ToArray();

            // Merge overlapping ranges
            Array.Sort(ranges, (a, b) => a.Item1.CompareTo(b.Item1));
            var mergedRanges = new List<(long, long)>();
            foreach (var range in ranges)
            {
                if (mergedRanges.Count == 0 || mergedRanges.Last().Item2 < range.Item1)
                {
                    mergedRanges.Add(range);
                }
                else
                {
                    var last = mergedRanges.Last();
                    mergedRanges[^1] = (last.Item1, Math.Max(last.Item2, range.Item2));
                }
            }

            // Calculate total covered numbers
            long totalCovered = 0;
            foreach (var (min, max) in mergedRanges)
            {
                totalCovered += (max - min + 1);
            }

            Console.WriteLine(totalCovered);
        }
    }
}
