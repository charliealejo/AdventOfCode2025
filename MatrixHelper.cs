using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025
{
    internal class MatrixHelper
    {
        internal static readonly (int x, int y)[] Directions =
        [
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),  (1, 1)
        ];

        internal static Position[] GetAdjacentPositions(Position pos, int maxX, int maxY)
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

        internal static Dictionary<(Position3D, Position3D), double> GetAllDistances(Position3D[] pointList) { 
            var distances = new Dictionary<(Position3D, Position3D), double>();
            for (int i = 0; i < pointList.Length; i++)
            {
                for (int j = i + 1; j < pointList.Length; j++)
                {
                    var p1 = pointList[i];
                    var p2 = pointList[j];
                    double distance = Math.Sqrt(
                        Math.Pow(p1.X - p2.X, 2) +
                        Math.Pow(p1.Y - p2.Y, 2) +
                        Math.Pow(p1.Z - p2.Z, 2)
                    );
                    distances[(p1, p2)] = distance;
                }
            }
            return distances;
        }

        internal static Dictionary<(Position, Position), double> GetAllAreas(Position[] pointList)
        {
            var areas = new Dictionary<(Position, Position), double>();
            for (int i = 0; i < pointList.Length; i++)
            {
                for (int j = i + 1; j < pointList.Length; j++)
                {
                    var p1 = pointList[i];
                    var p2 = pointList[j];
                    double area = (long)(Math.Abs(p1.X - p2.X) + 1) * (Math.Abs(p1.Y - p2.Y) + 1);
                    areas[(p1, p2)] = area;
                }
            }
            return areas;
        }

        internal static Dictionary<(Position, Position), double> GetAllDistances(Position[] pointList)
        {
            var distances = new Dictionary<(Position, Position), double>();
            for (int i = 0; i < pointList.Length; i++)
            {
                for (int j = i + 1; j < pointList.Length; j++)
                {
                    var p1 = pointList[i];
                    var p2 = pointList[j];
                    double distance = Math.Sqrt(
                        Math.Pow(p1.X - p2.X, 2) +
                        Math.Pow(p1.Y - p2.Y, 2)
                    );
                    distances[(p1, p2)] = distance;
                }
            }
            return distances;
        }
    }

    internal class Position(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public override string ToString() => $"({X}, {Y})";
    }

    internal class Position3D(int x, int y, int z)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int Z { get; set; } = z;
        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}
