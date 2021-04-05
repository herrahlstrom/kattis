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
    }

    public class VerticalLine : ILinearEquation
    {
        public float X { get; set; }
    }

    public class HorizontalLine : ILinearEquation
    {
        public float Y { get; set; }
    }

    public class LinearEquation : ILinearEquation
    {
        public float K { get; set; }
        public float M { get; set; }
    }

    public class SinglePointLine : ILinearEquation
    {
        public Point Point { get; set; }
    }

    public interface ILinearEquation
    {
    }

    public struct Line
    {
        public Point A { get; set; }
        public Point B { get; set; }

        public Point GetTop()
        {
            return A.Y < B.Y ? A : B;
        }

        public Point GetBottom()
        {
            return A.Y > B.Y ? A : B;
        }

        public Point GetLeft()
        {
            return A.X < B.X ? A : B;
        }

        public ILinearEquation GetLinearEquation()
        {
            if (A.X == B.X && A.Y == B.Y)
            {
                return new SinglePointLine {Point = A};
            }

            if (A.X == B.X)
            {
                return new VerticalLine {X = A.X};
            }

            if (A.Y == B.Y)
            {
                return new HorizontalLine {Y = A.Y};
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

        public Point GetRight()
        {
            return A.X > B.X ? A : B;
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
            int x1 = Math.Min(vector.A.X, vector.B.X);
            int y1 = Math.Min(vector.A.Y, vector.B.Y);
            int x2 = Math.Max(vector.A.X, vector.B.X);
            int y2 = Math.Max(vector.A.Y, vector.B.Y);
            if (point.X < x1 || point.X > x2 || point.Y < y1 || point.Y > y2)
            {
                return false;
            }

            float dx = x2 == x1
                ? float.MaxValue
                : ((float) point.X - x1) / (x2 - x1);

            float dy = y2 == y1
                ? float.MaxValue
                : ((float) point.Y - y1) / (y2 - y1);

            return dx.Equals(dy);
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
                A = new Point {X = x1 - 1, Y = point.Y},
                B = point
            };

            int lineCrossCounter = 0;
            foreach (Line pLine in GetPolygonLines())
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
            ILinearEquation lineLinearEquation = line.GetLinearEquation();
            
            if (lineLinearEquation is SinglePointLine || lineLinearEquation is HorizontalLine)
            {
                return false;
            }

            if (lineLinearEquation is VerticalLine vLine)
            {
                // Assumption: rayCastingLine is Horizontal
                return rayCastingLine.GetLeft().X < vLine.X &&
                       rayCastingLine.GetRight().X > vLine.X &&
                       rayCastingLine.A.Y > line.GetTop().Y &&
                       rayCastingLine.A.Y < line.GetBottom().Y;
            }

            if (lineLinearEquation is LinearEquation regLine)
            {
                if (regLine.K == 0)
                {
                    return false;
                }

                // Assumption: rayCastingLine is Horizontal
                float crossX = (rayCastingLine.A.Y - regLine.M) / regLine.K;
                return crossX >= line.GetLeft().X && crossX <= line.GetRight().X &&
                       crossX >= rayCastingLine.GetLeft().X && crossX <= rayCastingLine.GetRight().X;
            }

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