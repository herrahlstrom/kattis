// https://open.kattis.com/problems/shovelling

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace kattis
{
    public class Program
    {
        private const string Friends = "ABCD";
        private const char Blocked = '#';
        private const char Snow = 'o';
        private const char Cleared = '.';
        private const char Shovelled = Cleared;

        private static void Main(string[] args)
        {
            while (ReadTestCase() is { } test)
            {
                var solution = Solve(test);

                PrintMap(test.Map, p =>
                {
                    if (test.Map[p.X, p.Y] == Snow && solution.Contains(p))
                        return Shovelled;
                    else
                        return test.Map[p.X, p.Y];
                });
            }

            Console.WriteLine("0 0");
        }

        private static TestCase? ReadTestCase()
        {
            string[] dim = Console.ReadLine()!.Split();
            int width = int.Parse(dim[0]);
            int height = int.Parse(dim[1]);

            if (width == 0 && height == 0)
            {
                return null;
            }

            var map = new char[width, height];
            Point[] friends = new Point[4];

            for (int y = 0; y < height; y++)
            {
                string line = Console.ReadLine()!;
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = line[x];

                    if (Friends.IndexOf(line[x]) is { } p and >= 0)
                        friends[p] = new(x, y);
                }
            }

            // Read expected empty line
            Console.ReadLine();

            return new TestCase(map, height, width, friends);
        }

        private static ICollection<Point> Solve(TestCase test)
        {
            PathNode?[][,] dists = Enumerable.Range(0, 4)
                .Select(i => CalculateDistances(test.Map, test.Friends[i]))
                .ToArray();

            int bestCaseCost = 1_000_000;
            List<Point> bestCase = new List<Point>();
            HashSet<Point> current = new HashSet<Point>();

            for (int i = 0; i < test.Size; i++)
            {
                var p1 = test.GetPoint(i);
                if (test.Map[p1.X, p1.Y] == Blocked)
                    continue;

                if (dists.First()[p1.X, p1.Y] is null)
                    continue;

                var p1Distances = CalculateDistances(test.Map, p1);

                for (int j = i; j < test.Size; j++)
                {
                    var p2 = test.GetPoint(j);
                    if (test.Map[p2.X, p2.Y] == Blocked)
                        continue;

                    if (dists.First()[p2.X, p2.Y] is null)
                        continue;

                    bool stillPossible = true;

                    current.Clear();
                    foreach (var friendDistance in dists)
                    {
                        var p1Dist = friendDistance[p1.X, p1.Y];
                        var p2Dist = friendDistance[p2.X, p2.Y];

                        if (p1Dist is null || p2Dist is null)
                        {
                            stillPossible = false;
                            continue;
                        }

                        PathNode? pDist = p1Dist.Cost < p2Dist.Cost ? p1Dist : p2Dist;
                        foreach (var node in pDist.GetPath())
                        {
                            if (test.Map[node.X, node.Y] == Snow)
                            {
                                current.Add(node);
                            }
                            if (current.Count > bestCaseCost)
                            {
                                stillPossible = false;
                                break;
                            }
                        }

                        if (!stillPossible)
                            break;
                    }

                    if (!stillPossible)
                        continue;

                    // Add path between Steiner points
                    if (i != j)
                    {
                        var betweenSteinerPoints = p1Distances[p2.X, p2.Y];
                        if (betweenSteinerPoints is null)
                        {
                            stillPossible = false;
                            continue;
                        }
                        foreach (var node in betweenSteinerPoints.GetPath())
                        {
                            if (test.Map[node.X, node.Y] == Snow)
                            {
                                current.Add(node);
                            }
                        }
                    }

                    if (current.Count < bestCaseCost)
                    {
                        bestCase.Clear();
                        bestCase.AddRange(current);
                        bestCaseCost = current.Count;
                    }
                }
            }

            return bestCase;
        }

        private static void PrintMap<T, U>(T[,] map, Func<Point, U> projection)
        {
            StringBuilder output = new StringBuilder(1024);

            output.AppendFormat("{0} {1}", map.GetLength(0), map.GetLength(1)).AppendLine();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    output.Append(projection(new Point(x, y)));
                }
                output.AppendLine();
            }

            output.AppendLine();
            Console.Write(output);
        }

        private static PathNode?[,] CalculateDistances(char[,] map, Point start)
        {
            PathNode?[,] result = new PathNode[map.GetLength(0), map.GetLength(1)];
            var startCost = 0;
            if (map[start.X, start.Y] == Snow)
                startCost = 1;

            result[start.X, start.Y] = new PathNode(start, startCost, null);

            Queue<Point> q = new Queue<Point>([start]);
            while (q.TryDequeue(out var p))
            {
                var currentPath = result[p.X, p.Y]!;

                Point[] nexts = [p.Left(), p.Right(), p.Up(), p.Down()];
                foreach (var next in nexts.Where(x => x.InMap(map)))
                {
                    var c = map[next.X, next.Y];
                    if (c == Blocked)
                        continue;

                    int newCost = currentPath.Cost;
                    if (c == Snow)
                        newCost += 1;
                    if (result[next.X, next.Y] is { } last && last.Cost <= newCost)
                        continue;

                    result[next.X, next.Y] = new PathNode(next, newCost, currentPath);
                    q.Enqueue(next);
                }
            }

            return result;
        }
    }

    internal record TestCase(char[,] Map, int Height, int Width, Point[] Friends)
    {
        public int Size { get; } = Height * Width;
        public int GetIndex(Point point) => point.Y * Width + point.X;
        public Point GetPoint(int index) => new Point(index % Width, index / Width);
    }
    internal record struct Point(int X, int Y)
    {
        public bool InMap(char[,] map) => X >= 0 && X < map.GetLength(0) && Y >= 0 && Y < map.GetLength(1);
        public Point Left() => new Point(X - 1, Y);
        public Point Right() => new Point(X + 1, Y);
        public Point Up() => new Point(X, Y - 1);
        public Point Down() => new Point(X, Y + 1);
        public override string ToString()
        {
            return $"{X}x{Y}";
        }
    }
    internal record class PathNode(Point Point, int Cost, PathNode? Previous)
    {
        public IEnumerable<Point> GetPath()
        {
            PathNode? node = this;
            while (node != null)
            {
                yield return node.Point;
                node = node.Previous;
            }
        }
    }
}