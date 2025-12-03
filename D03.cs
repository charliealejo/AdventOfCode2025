namespace AdventOfCode2025
{
    internal class D03 : Day
    {
        /// <summary>
        /// Part 1 solution.
        /// 
        /// Shows how to solve the problem using a manual approach. We first find the top two highest
        /// values in the line. Then we manually check the conditions to calculate the result.
        /// 
        /// We could use the approach shown in Part 2 to solve this as well, but I wanted to demonstrate
        /// the manual approach.
        /// </summary>
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLinesAsIntMap("D03.txt");

            long sum = 0;
            foreach (var l in lines)
            {
                var topTwo = l.Select((v, i) => (v, i)).OrderByDescending(t => t.v).Take(2).ToArray();
                if (topTwo[0].i == l.Length - 1)
                {
                    var v = topTwo[1].v * 10 + topTwo[0].v;
                    sum += v;
                }
                else if (topTwo[0].v > topTwo[1].v && topTwo[0].i > topTwo[1].i)
                {
                    var v = topTwo[0].v * 10 + l.Skip(topTwo[0].i + 1).Max();
                    sum += v;
                }
                else
                {
                    var v = topTwo[0].v * 10 + topTwo[1].v;
                    sum += v;
                }
            }

            Console.WriteLine(sum);
        }

        /// <summary>
        /// Part 2 solution.
        /// 
        /// As we could not use a manual check for 12 batteries, I implemented a sliding window approach.
        /// For each battery to turn on, we find the maximum value in the allowed range, add it to the result,
        /// and then update the range for the next battery.
        /// </summary>
        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLinesAsIntMap("D03.txt");

            long sum = 0;
            foreach (var l in lines)
            {
                var batteryValues = new List<int>();
                var length = l.Length;
                var batteriesToTurn = 12;
                var window = length - batteriesToTurn + 1;
                var startPosition = 0;
                for (int i = 0; i < batteriesToTurn; i++)
                {
                    // Find the maximum value in the allowed range and add it to the battery values
                    var w = l[startPosition..(startPosition + window)];
                    var maxInRange = w.Max();
                    var maxIndex = Array.IndexOf(w, maxInRange);
                    var maxPosition = startPosition + maxIndex;
                    batteryValues.Add(maxInRange);
                    // Update the start position and window size for the next iteration
                    window -= maxPosition - startPosition;
                    startPosition = maxPosition + 1;
                }
                sum += batteryValues.Select((v, i) => v * (long)Math.Pow(10, batteriesToTurn - i - 1)).Sum();
            }

            Console.WriteLine(sum);
        }
    }
}
