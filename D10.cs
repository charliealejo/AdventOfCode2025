namespace AdventOfCode2025
{
    internal class D10 : Day
    {
        internal override void SolvePart1()
        {
            var machines = FileHelper.ReadLines("D10.txt");

            var answer = 0;
            foreach (var machine in machines)
            {
                var parts = machine.Split(' ');
                var goal = parts[0][1..^1].Select(c => c == '#').ToArray();
                var buttons = parts[1..^1].Select(p => p[1..^1].Split(',').Select(long.Parse).ToArray()).ToArray();

                bool found = false;

                for (int i = 1; i < buttons.Length && !found; i++)
                {
                    var combinations = buttons.GetCombinations(i);
                    foreach (var combination in combinations)
                    {
                        var state = new bool[goal.Length];
                        foreach (var button in combination)
                        {
                            for (int b = 0; b < button.Length; b++)
                            {
                                state[button[b]] = !state[button[b]];
                            }
                        }
                        if (state.SequenceEqual(goal))
                        {
                            answer += i;
                            found = true;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine(answer);
        }

        internal override void SolvePart2()
        {
            var machines = FileHelper.ReadLines("D10.txt");

            var answer = 0L;
            foreach (var machine in machines)
            {
                var parts = machine.Split(' ');
                var goal = parts[^1][1..^1].Split(',').Select(long.Parse).ToArray();
                var buttons = parts[1..^1].Select(p => p[1..^1].Split(',').Select(long.Parse).ToArray()).ToArray();

                var result = FindShortestPath(buttons, goal);
                answer += result;
            }

            Console.WriteLine(answer);
        }

        private static long FindShortestPath(long[][] buttons, long[] goal)
        {
            int numPositions = goal.Length;
            int numButtons = buttons.Length;
            
            // Build coefficient array: coeffs[button][position] = how much button adds to position
            var coeffs = new int[numButtons][];
            for (int b = 0; b < numButtons; b++)
            {
                coeffs[b] = new int[numPositions];
                foreach (var pos in buttons[b])
                {
                    if (pos < numPositions)
                        coeffs[b][pos]++;
                }
            }

            // Use recursive backtracking with greedy selection
            var bestResult = long.MaxValue;
            var currentPresses = new int[numButtons];
            var currentState = new long[numPositions];
            
            Solve(0);
            
            return bestResult == long.MaxValue ? -1 : bestResult;

            void Solve(int totalPresses)
            {
                // Pruning: if we already exceed best, stop
                if (totalPresses >= bestResult)
                    return;

                // Check if we reached the goal
                bool isGoal = true;
                for (int p = 0; p < numPositions; p++)
                {
                    if (currentState[p] != goal[p])
                    {
                        isGoal = false;
                        break;
                    }
                }
                
                if (isGoal)
                {
                    bestResult = totalPresses;
                    return;
                }

                // Find the first position that needs more presses
                int targetPos = -1;
                long maxDeficit = 0;
                for (int p = 0; p < numPositions; p++)
                {
                    var deficit = goal[p] - currentState[p];
                    if (deficit > maxDeficit)
                    {
                        maxDeficit = deficit;
                        targetPos = p;
                    }
                }

                if (targetPos == -1) return;

                // Find buttons that can help this position, sorted by efficiency
                var candidates = new List<(int button, int contribution, int maxPresses)>();
                for (int b = 0; b < numButtons; b++)
                {
                    if (coeffs[b][targetPos] > 0)
                    {
                        // Calculate max times we can press this button without exceeding any goal
                        int maxPresses = int.MaxValue;
                        for (int p = 0; p < numPositions; p++)
                        {
                            if (coeffs[b][p] > 0)
                            {
                                int remaining = (int)(goal[p] - currentState[p]);
                                maxPresses = Math.Min(maxPresses, remaining / coeffs[b][p]);
                            }
                        }
                        
                        if (maxPresses > 0)
                        {
                            candidates.Add((b, coeffs[b][targetPos], maxPresses));
                        }
                    }
                }

                // Sort by contribution descending (greedy: use most impactful button first)
                candidates.Sort((a, b) => b.contribution.CompareTo(a.contribution));

                // Try each candidate button
                foreach (var (button, contribution, maxPresses) in candidates)
                {
                    // Greedy: try pressing as many times as possible first, then fewer
                    for (int presses = maxPresses; presses >= 1; presses--)
                    {
                        // Apply presses
                        for (int p = 0; p < numPositions; p++)
                            currentState[p] += coeffs[button][p] * presses;
                        currentPresses[button] += presses;

                        // Check validity (no position exceeds goal)
                        bool valid = true;
                        for (int p = 0; p < numPositions; p++)
                        {
                            if (currentState[p] > goal[p])
                            {
                                valid = false;
                                break;
                            }
                        }

                        if (valid)
                        {
                            Solve(totalPresses + presses);
                        }

                        // Undo presses
                        for (int p = 0; p < numPositions; p++)
                            currentState[p] -= coeffs[button][p] * presses;
                        currentPresses[button] -= presses;

                        // Early exit if we found an optimal solution
                        if (bestResult <= totalPresses + 1)
                            return;
                    }
                }
            }
        }
    }
}
