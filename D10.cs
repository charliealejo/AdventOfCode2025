using Google.OrTools.LinearSolver;
using System.Data;

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
                var buttons = parts[1..^1].Select(p => p[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray();

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
                var goal = parts[^1][1..^1].Split(',').Select(int.Parse).ToArray();
                var buttons = parts[1..^1].Select(p => p[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray();

                var result = Solve(buttons, goal);
                answer += result.Sum();
            }

            Console.WriteLine(answer);
        }

        public static int[] Solve(int[][] buttons, int[] target)
        {
            var solver = Solver.CreateSolver("SCIP");
            if (solver == null) return null;

            int numButtons = buttons.Length;
            int numPositions = target.Length;

            // Variables: how many times we press each button
            var presses = new Variable[numButtons];
            for (int i = 0; i < numButtons; i++)
            {
                presses[i] = solver.MakeIntVar(0, target.Max(), $"button_{i}");
            }

            // Constraints: each position must reach its target
            for (int pos = 0; pos < numPositions; pos++)
            {
                var constraint = solver.MakeConstraint(target[pos], target[pos]);
                for (int btn = 0; btn < numButtons; btn++)
                {
                    if (buttons[btn].Contains(pos))
                    {
                        constraint.SetCoefficient(presses[btn], 1);
                    }
                }
            }

            // Objective: minimize total button presses
            var objective = solver.Objective();
            for (int i = 0; i < numButtons; i++)
            {
                objective.SetCoefficient(presses[i], 1);
            }
            objective.SetMinimization();

            var resultStatus = solver.Solve();

            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                var result = new int[numButtons];
                for (int i = 0; i < numButtons; i++)
                {
                    result[i] = (int)presses[i].SolutionValue();
                }
                return result;
            }

            return null;
        }
    }
}
