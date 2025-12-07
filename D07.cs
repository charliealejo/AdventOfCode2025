namespace AdventOfCode2025
{
    internal class D07 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines("D07.txt").ToArray();

            var beamArray = new bool[lines[0].Length];
            beamArray[lines[0].IndexOf('S')] = true;
            var splitterGrid = lines[2..^1].Select(l => l.Select(c => c == '^').ToArray()).ToArray();

            long splitted = 0;
            foreach (var splitterLine in splitterGrid)
            {
                for (int i = 0; i < splitterLine.Length; i++)
                {
                    if (beamArray[i] && splitterLine[i])
                    {
                        beamArray[i - 1] = true;
                        beamArray[i + 1] = true;
                        beamArray[i] = false;
                        splitted++;
                    }
                }
            }

            Console.WriteLine(splitted);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines("D07.txt").ToArray();

            var startPosition = lines[0].IndexOf('S');
            var splitterGrid = lines[2..^1].Select(l => l.Select(c => c == '^').ToArray()).ToArray();

            var answer = Solve(startPosition, 0, splitterGrid);
            Console.WriteLine(answer);
        }

        private static readonly Dictionary<(int position, int line), long> Memo = [];

        private static long Solve(int position, int line, bool[][] splitterGrid)
        {
            if (Memo.ContainsKey((position, line)))
            {
                return Memo[(position, line)];
            }
            if (line >= splitterGrid.Length)
            {
                return 1;
            }
            if (!splitterGrid[line][position])
            {
                long down = Solve(position, line + 2, splitterGrid);
                Memo[(position, line)] = down;
                return down;
            }
            long left = Solve(position - 1, line + 2, splitterGrid);
            long right = Solve(position + 1, line + 2, splitterGrid);
            Memo[(position, line)] = left + right;
            return left + right;
        }
    }
}
