namespace AdventOfCode2025
{
    internal class D11 : Day
    {
        internal override void SolvePart1()
        {
            var circuits = FileHelper.ReadLines("D11.txt");
            var nodes = CreateNodes(circuits);

            var answer = 0;

            var stack = new Stack<string>();
            stack.Push("you");
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                foreach (var neighbor in nodes[current])
                {
                    if (neighbor == "out")
                    {
                        answer++;
                    }
                    else
                    {
                        stack.Push(neighbor);
                    }
                }
            }

            Console.WriteLine(answer);
        }

        internal override void SolvePart2()
        {
            var circuits = FileHelper.ReadLines("D11.txt");

            var nodes = CreateNodes(circuits);

            var cache = new Dictionary<(string node, bool hasFft, bool hasDac), long>();

            long countPaths(string current, bool hasFft, bool hasDac)
            {
                hasFft = hasFft || current == "fft";
                hasDac = hasDac || current == "dac";

                if (current == "out")
                {
                    return (hasFft && hasDac) ? 1 : 0;
                }

                var key = (current, hasFft, hasDac);
                if (cache.TryGetValue(key, out var cachedValue))
                {
                    return cachedValue;
                }

                long count = 0;
                if (nodes.TryGetValue(current, out var neighbors))
                {
                    foreach (var neighbor in neighbors)
                    {
                        count += countPaths(neighbor, hasFft, hasDac);
                    }
                }

                cache[key] = count;
                return count;
            }

            var validPaths = countPaths("svr", false, false);

            Console.WriteLine(validPaths);
        }

        private static Dictionary<string, string[]> CreateNodes(IEnumerable<string> circuits)
        {
            var nodes = new Dictionary<string, string[]>();

            foreach (var circuit in circuits)
            {
                var label = circuit[0..3];
                var outputs = circuit[5..].Split(" ");
                nodes[label] = outputs;
            }

            return nodes;
        }
    }
}
