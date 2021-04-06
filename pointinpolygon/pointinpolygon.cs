/*
 * https://open.kattis.com/problems/pointinpolygon
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace pointinpolygon
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return "{" + X + "," + Y + "}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Point other)
            {
                return X == other.X && Y == other.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }
    }

    public class LinearEquation
    {
        public float K { get; set; }
        public float M { get; set; }
    }


    public struct Line
    {
        public Point A { get; set; }
        public Point B { get; set; }

        public bool IsHorizontal => A.Y == B.Y;
        public bool IsVertical => A.X == B.X;

        public Point GetTop()
        {
            return A.Y > B.Y ? A : B;
        }

        public Point GetBottom()
        {
            return A.Y < B.Y ? A : B;
        }

        public Point GetLeft()
        {
            return A.X < B.X ? A : B;
        }

        public LinearEquation GetLinearEquation()
        {
            if (A.X == B.X && A.Y == B.Y)
            {
                throw new NotImplementedException("A line can't start/end on the same point");
            }

            if (A.X == B.X)
            {
                throw new InvalidOperationException("Cant create a linear equation for a vertical line");
            }

            float x1, x2, y1, y2;
            if (A.X < B.X)
            {
                x1 = A.X;
                y1 = A.Y;
                x2 = B.X;
                y2 = B.Y;
            }
            else
            {
                x1 = B.X;
                y1 = B.Y;
                x2 = A.X;
                y2 = A.Y;
            }


            float k = (y2 - y1) / (x2 - x1);

            return new LinearEquation
            {
                K = k,
                M = x1 * -k + y1
            };
        }

        public float GetLinearEquation(float x)
        {
            LinearEquation linearEquation = GetLinearEquation();
            return linearEquation.K * x + linearEquation.M;
        }

        public Point GetRight()
        {
            return A.X > B.X ? A : B;
        }

        public override string ToString()
        {
            return $"{A}-{B}";
        }
    }

    public enum Result
    {
        On,
        In,
        Out
    }

    public class TestCase
    {
        private readonly List<Point> _polygons = new List<Point>();

        private static bool OnSegment(Line vector, Point point)
        {
            if (point.X < vector.GetLeft().X ||
                point.X > vector.GetRight().X ||
                point.Y < vector.GetBottom().Y ||
                point.Y > vector.GetTop().Y)
            {
                return false;
            }

            if (point.Equals(vector.A) || point.Equals(vector.B))
            {
                return true;
            }

            if (vector.IsHorizontal)
            {
                return point.Y == vector.A.Y &&
                       point.X >= vector.GetLeft().X &&
                       point.X <= vector.GetRight().X;
            }

            if (vector.IsVertical)
            {
                return point.X == vector.A.X &&
                       point.Y >= vector.GetBottom().Y &&
                       point.Y <= vector.GetTop().Y;
            }

            Point left = vector.GetLeft();
            Point right = vector.GetRight();

            float dx = Math.Abs((float) point.X - left.X) / Math.Abs(right.X - left.X);
            float dy = Math.Abs((float) point.Y - left.Y) / Math.Abs(right.Y - left.Y);

            return Math.Abs(dx - dy) <= 0.0001;
        }

        public void AddPolygonPoint(Point p)
        {
            _polygons.Add(p);
        }

        public Result Test(Point point)
        {
            int x1 = _polygons.Min(p => p.X);
            int y1 = _polygons.Min(p => p.Y);
            int x2 = _polygons.Max(p => p.X);
            int y2 = _polygons.Max(p => p.Y);
            if (point.X < x1 || point.X > x2 || point.Y < y1 || point.Y > y2)
            {
                return Result.Out;
            }

            var rayCastingLine = new Line
            {
                // A small offset on Y to not get an horizontal line
                A = new Point {X = x1 - 1, Y = point.Y + 1},
                B = point
            };

            int lineCrossCounter = 0;
            List<Line> lines = GetPolygonLines().ToList();
            foreach (Line pLine in lines)
            {
                if (OnSegment(pLine, point))
                {
                    return Result.On;
                }

                if (LineIntesect(pLine, rayCastingLine))
                {
                    lineCrossCounter++;
                }
            }

            return lineCrossCounter % 2 == 0
                ? Result.Out
                : Result.In;
        }

        private IEnumerable<Line> GetPolygonLines()
        {
            for (int i = 1; i < _polygons.Count; i++)
            {
                yield return new Line
                {
                    A = _polygons[i - 1],
                    B = _polygons[i]
                };
            }

            yield return new Line
            {
                A = _polygons.Last(),
                B = _polygons.First()
            };
        }

        private bool LineIntesect(Line line, Line rayCastingLine)
        {
            if (line.IsVertical && rayCastingLine.IsVertical)
            {
                return false;
            }

            if (line.IsVertical || rayCastingLine.IsVertical)
            {
                int x;
                Line vLine;
                Line otherLine;
                if (line.IsVertical)
                {
                    x = line.A.X;
                    vLine = line;
                    otherLine = rayCastingLine;
                }
                else if (rayCastingLine.IsVertical)
                {
                    x = rayCastingLine.A.X;
                    vLine = rayCastingLine;
                    otherLine = line;
                }
                else
                {
                    throw new NotImplementedException();
                }

                float intersectY = otherLine.GetLinearEquation(x);
                return otherLine.GetLeft().X < x &&
                       otherLine.GetRight().X > x &&
                       intersectY <= vLine.GetTop().Y &&
                       intersectY >= vLine.GetBottom().Y;
            }

            LinearEquation lineEq = line.GetLinearEquation();
            LinearEquation rayEq = rayCastingLine.GetLinearEquation();

            if (Math.Abs(rayEq.K - lineEq.K) < 0.001)
            {
                return false;
            }

            float crossX = (lineEq.M - rayEq.M) / (rayEq.K - lineEq.K);

            return crossX >= rayCastingLine.GetLeft().X &&
                   crossX >= line.GetLeft().X &&
                   crossX <= rayCastingLine.GetRight().X &&
                   crossX < line.GetRight().X;
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

                foreach (Point p in ReadPoints(n))
                {
                    testCase.AddPolygonPoint(p);
                }

                int m = int.Parse(Console.ReadLine());
                foreach (Point p in ReadPoints(m))
                {
                    switch (testCase.Test(p))
                    {
                        case Result.In:
                            Console.WriteLine("in");
                            break;
                        case Result.On:
                            Console.WriteLine("on");
                            break;
                        case Result.Out:
                            Console.WriteLine("out");
                            break;
                        default:
                            throw new NotImplementedException();
                    }
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