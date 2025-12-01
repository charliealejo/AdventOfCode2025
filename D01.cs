namespace AdventOfCode2025
{
    internal class D01 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);

            (char d, int n)[] values = [.. lines.Select(line => (d: line[0], n: int.Parse(line[1..])))];

            var p = 50;
            var c = 0;
            for (int i = 0; i < values.Length; i++)
            {
                var (d, n) = values[i];
                if (d == 'R')
                {
                    p += n;
                    p %= 100;
                }
                else if (d == 'L')
                {
                    p -= n;
                    p %= 100;
                }
                if (p == 0) c++;
            }

            Console.WriteLine(c);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName);

            (char d, int n)[] values = [.. lines.Select(line => (d: line[0], n: int.Parse(line[1..])))];

            var p = 50;
            var c = 0;
            for (int i = 0; i < values.Length; i++)
            {
                var (d, n) = values[i];
                if (d == 'R')
                {
                    p += n;
                    c += p / 100;
                    p %= 100;
                }
                else if (d == 'L')
                {
                    if (p == 0) c--;
                    p -= n;
                    if (p < 0)
                    {
                        int w = (-p + 99) / 100;
                        c += w;
                        p += w * 100;
                    }
                    if (p % 100 == 0) c++;
                }
            }

            Console.WriteLine(c);
        }
    }
}
