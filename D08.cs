namespace AdventOfCode2025
{
    internal class D08 : Day
    {
        internal override void SolvePart1()
        {
            var points = FileHelper.ReadLinesAs3DPoints("D08.txt", ",");

            var distances = MatrixHelper.GetAllDistances(points);
            var orderedDistances = distances.OrderBy(kv => kv.Value).ToList();
            var circuits = new List<List<Position3D>>();
            for (int i = 0; i < 1000; i++)
            {
                var pair = orderedDistances[i].Key;
                AddToCircuit(circuits, pair);
            }
            // Sort circuits by length descending and multiply lengths
            var result = circuits.OrderByDescending(c => c.Count).Take(3).Select(c => (long)c.Count).Product();
            Console.WriteLine(result);
        }

        internal override void SolvePart2()
        {
            var points = FileHelper.ReadLinesAs3DPoints("D08.txt", ",");

            var distances = MatrixHelper.GetAllDistances(points);
            var orderedDistances = distances.OrderBy(kv => kv.Value).ToList();
            var circuits = new List<List<Position3D>>();
            (Position3D, Position3D) pair = default; // Ensure 'pair' is always assigned
            for (int i = 0; i < orderedDistances.Count; i++)
            {
                pair = orderedDistances[i].Key;
                AddToCircuit(circuits, pair);
                if (circuits.Count == 1 && circuits[0].Count == points.Length)
                {
                    break;
                }
            }
            // Sort circuits by length descending and multiply lengths
            var result = (long)pair.Item1.X * pair.Item2.X;
            Console.WriteLine(result);
        }

        private static void AddToCircuit(List<List<Position3D>> circuits, (Position3D, Position3D) pair)
        {
            // Find if either point is already in a circuit
            var circuit1 = circuits.FirstOrDefault(c => c.Contains(pair.Item1));
            var circuit2 = circuits.FirstOrDefault(c => c.Contains(pair.Item2));
            // If both circuits exist and are different, merge them
            if (circuit1 != null && circuit2 != null && circuit1 != circuit2)
            {
                circuit1.AddRange(circuit2);
                circuits.Remove(circuit2);
                return;
            }
            // If circuit 1 exists, try to add point 2 to it
            if (circuit1 != null)
            {
                if (!circuit1.Contains(pair.Item2))
                {
                    circuit1.Add(pair.Item2);
                }
                return;
            }
            // If circuit 2 exists, try to add point 1 to it
            if (circuit2 != null)
            {
                if (!circuit2.Contains(pair.Item1))
                {
                    circuit2.Add(pair.Item1);
                }
                return;
            }
            // If neither point is in a circuit, create a new circuit
            if (circuit1 == null && circuit2 == null)
            {
                circuits.Add([pair.Item1, pair.Item2]);
            }
        }
    }
}
