using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CoastLength
{
    struct Point
    {
        public Point(int x, int y) { X = x; Y = y; }
        public int X;
        public int Y;
    }
    public class Program
    {
        const char Water = '0';
        const char Land = '1';
        static bool InMap(char[,] map, Point pos)
        {
            return pos.X >= 0 && pos.X < map.GetLength(0) && pos.Y >= 0 && pos.Y < map.GetLength(1);
        }
        static IEnumerable<Point> GetNeighbors(Point point)
        {
            yield return new Point(point.X - 1, point.Y);
            yield return new Point(point.X + 1, point.Y);
            yield return new Point(point.X, point.Y - 1);
            yield return new Point(point.X, point.Y + 1);
        }
        static void Main(string[] args)
        {
            int[] nm = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
            int n = nm[0];
            int m = nm[1];

            char[,] map = GetMap(n, m);
            bool[,] cWater = GetCoastWater(map);

            int coast = 0;
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < m; x++)
                {
                    if (map[x, y] == Water)
                        continue;
                    foreach (var neighbor in GetNeighbors(new Point(x, y)))
                    {
                        if (!InMap(map, neighbor))
                        {
                            coast++;
                        }
                        else if (cWater[neighbor.X, neighbor.Y])
                        {
                            coast++;
                        }
                    }
                }
            }

            Console.WriteLine(coast);
        }

        private static char[,] GetMap(int n, int m)
        {
            char[,] map = new char[m, n];
            for (int y = 0; y < n; y++)
            {
                string line = Console.ReadLine();
                for (int x = 0; x < m; x++)
                {
                    map[x, y] = line[x];
                }
            }
            return map;
        }

        private static bool[,] GetCoastWater(char[,] map)
        {
            bool[,] result = new bool[map.GetLength(0), map.GetLength(1)];
            bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];
            var queue = new Queue<Point>();

            var border = new List<Point>();

            // Översta raden
            for (int x = 0, y = 0; x < map.GetLength(0); x++)
                border.Add(new Point(x, y));

            // Nedersta raden
            for (int x = 0, y = map.GetLength(1) - 1; x < map.GetLength(0); x++)
                border.Add(new Point(x, y));

            // Vänstra raden
            for (int x = 0, y = 0; y < map.GetLength(1); y++)
                border.Add(new Point(x, y));

            // Hgra raden
            for (int x = map.GetLength(0) - 1, y = 0; y < map.GetLength(1); y++)
                border.Add(new Point(x, y));

            foreach (var pos in border)
            {
                if (map[pos.X, pos.Y] == Water)
                {
                    result[pos.X, pos.Y] = true;
                    visited[pos.X, pos.Y] = true;
                    queue.Enqueue(pos);
                }
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var newNeighbors = from neighbor in GetNeighbors(current)
                                   where InMap(map, neighbor)
                                   where map[neighbor.X, neighbor.Y] == Water
                                   where !visited[neighbor.X, neighbor.Y]
                                   select neighbor;
                foreach (var neighbor in newNeighbors)
                {
                    result[neighbor.X, neighbor.Y] = true;
                    visited[neighbor.X, neighbor.Y] = true;
                    queue.Enqueue(neighbor);
                }
            }

            return result;
        }
    }
}