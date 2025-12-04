using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025
{
    internal class MatrixHelper
    {
        public static readonly (int x, int y)[] Directions =
        [
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),  (1, 1)
        ];

        public static Position[] GetAdjacentPositions(Position pos, int maxX, int maxY)
        {
            var positions = new List<Position>();
            foreach (var (dx, dy) in Directions)
            {
                int newX = pos.X + dx;
                int newY = pos.Y + dy;
                if (newX >= 0 && newX < maxX && newY >= 0 && newY < maxY)
                {
                    positions.Add(new Position(newX, newY));
                }
            }
            return [.. positions];
        }
    }

    internal class Position(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }
}
