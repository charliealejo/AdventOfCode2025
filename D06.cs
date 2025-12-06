namespace AdventOfCode2025
{
    internal class D06 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines("D06.txt").ToArray();

            long answer = 0;
            var data = lines[..^1]
                .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => long.Parse(s)).ToArray())
                .ToArray();
            var operations = lines[^1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            for (int i = 0; i < data[0].Length; i++)
            {
                switch (operations[i])
                {
                    case "+":
                        answer += data.Select(d => d[i]).Sum();
                        break;
                    case "*":
                        answer += data.Select(d => d[i]).Product();
                        break;
                }
            }

            Console.WriteLine(answer);
        }

        internal override void SolvePart2()
        {
            var charMap = FileHelper.ReadLinesAsCharMap("D06.txt");

            // Rotate 90 degrees anticlockwise
            int rows = charMap.Length;
            int cols = charMap[0].Length;
            var rotated = Enumerable.Range(0, cols)
                .Select(c => new string([.. Enumerable.Range(0, rows).Select(r => charMap[r][cols - 1 - c])]))
                .ToArray();

            // Split into chunks by empty lines and solve
            long answer = rotated
                .ChunkBy(line => !string.IsNullOrWhiteSpace(line))
                .Sum(chunk =>
                {
                    var numbers = chunk.Select(s => long.Parse(s[..^1])).ToArray();
                    return chunk.Last().Last() switch
                    {
                        '+' => numbers.Sum(),
                        '*' => numbers.Product(),
                        _ => 0
                    };
                });

            Console.WriteLine(answer);
        }
    }
}
