/*
 * https://open.kattis.com/problems/pointinpolygon
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace pointinpolygon
{
    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    internal struct Line
    {
        public Point A { get; set; }
        public Point B { get; set; }
    }
    
    internal class TestCase
    {
        public List<Point> Polygons = new List<Point>();
        
        private static bool OnSegment(Line vector, Point point)
        {
            int x1 = Math.Min(vector.A.X, vector.B.X);
            int y1 = Math.Min(vector.A.Y, vector.B.Y);
            int x2 = Math.Max(vector.A.X, vector.B.X);
            int y2 = Math.Max(vector.A.Y, vector.B.Y);

            if (point.X <= x2 && point.X >= x1 && point.Y <= y2 && point.Y >= y1)
            {
                float dx = ((float) point.X - x1) / (x2 - x1);
                float dy = ((float) point.Y - y1) / (y2 - y1);
                return dx.Equals(dy);
            }

            return false;
        }

        public string Test(Point point)
        {
            Line[] polygonLines = GetPolygonLines().ToArray();

            if (polygonLines.Any(x => OnSegment(x, point)))
            {
                return "on";
            }

            return InPolygon() ? "in" : "out";
        }

        private IEnumerable<Line> GetPolygonLines()
        {
            for (int i = 1; i < Polygons.Count; i++)
            {
                yield return new Line
                {
                    A = Polygons[i - 1],
                    B = Polygons[i]
                };
            }

            yield return new Line
            {
                A = Polygons[^1],
                //A = Polygons[Polygons.Count - 1],
                B = Polygons[0]
            };
        }

        private bool InPolygon()
        {
            throw new NotImplementedException();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                var testCase = new TestCase();

                int n = int.Parse(Console.ReadLine());
                if (n == 0)
                {
                    break;
                }

                testCase.Polygons.AddRange(ReadPoints(n));

                int m = int.Parse(Console.ReadLine());
                foreach (Point p in ReadPoints(m))
                {
                    Console.WriteLine(testCase.Test(p));
                }
            }
        }

        private static IEnumerable<Point> ReadPoints(int n)
        {
            for (int i = 0; i < n; i++)
            {
                string[] ps = Console.ReadLine().Split(' ');
                yield return new Point
                {
                    X = int.Parse(ps[0]),
                    Y = int.Parse(ps[1])
                };
            }
        }
    }
}