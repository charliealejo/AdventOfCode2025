namespace AdventOfCode2025
{
    internal class D02 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadAll(FileName);
            var ranges = lines.Split(",").ToArray();

            long c = 0;

            foreach (var r in ranges)
            {
                var bounds = r.Split("-").Select(long.Parse).ToArray();
                for (long i = bounds[0]; i <= bounds[1]; i++)
                {
                    var s = i.ToString();
                    if (s[0..(s.Length / 2)] == s[(s.Length / 2)..])
                    {
                        c += i;
                    }
                }
            }

            Console.WriteLine(c);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadAll(FileName);
            var ranges = lines.Split(",").ToArray();

            long c = 0;

            foreach (var r in ranges)
            {
                var bounds = r.Split("-").Select(long.Parse).ToArray();
                for (long i = bounds[0]; i <= bounds[1]; i++)
                {
                    var s = i.ToString();
                    if (IsRepetitive(s))
                    {
                        c += i;
                    }
                }
            }

            Console.WriteLine(c);
        }

        /// <summary>
        /// Knuth-Morris-Pratt (KMP) algorithm to speed up the search for repetitive patterns.
        /// </summary>
        private static bool IsRepetitive(string s)
        {
            int l = s.Length;
            if (l <= 1) return false;
            int[] pi = new int[l];
            for (int i = 1, j = 0; i < l; i++)
            {
                while (j > 0 && s[i] != s[j])
                {
                    j = pi[j - 1];
                }
                if (s[i] == s[j])
                {
                    j++;
                }
                pi[i] = j;
            }
            int d = l - pi[l - 1];
            return d > 0 && l % d == 0 && d <= l / 2;
        }
    }
}
