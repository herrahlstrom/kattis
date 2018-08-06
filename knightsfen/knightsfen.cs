using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnightsFen
{
    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) { X = x; Y = y; }
        public static Point operator +(Point a, Point b) { return new Point(a.X + b.X, a.Y + b.Y); }
    }
    class Program
    {
        const string Goal = "111110111100 110000100000";
        private readonly static IEnumerable<Point> Steps = new[] { new Point(-1, -2), new Point(-1, 2), new Point(-2, -1), new Point(-2, 1), new Point(1, -2), new Point(1, 2), new Point(2, -1), new Point(2, 1) };

        static void Main(string[] args)
        {
            foreach (var map in GetMaps())
            {
                int answer = Solve(map.ToString());
                Console.WriteLine(answer < 11 ? $"Solvable in {answer} move(s)." : "Unsolvable in less than 11 move(s).");
            }
        }
        private static IEnumerable<string> GetMaps()
        {
            int n = int.Parse(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                StringBuilder map = new StringBuilder(25);
                for (int j = 0; j < 5; j++)
                    map.Append(Console.ReadLine());
                yield return map.ToString();
            }
        }
        private static IEnumerable<string> GetMoves(string map)
        {
            int index = map.IndexOf(' ');
            Point current = new Point(index % 5, index / 5);
            foreach (var d in Steps)
            {
                var n = (current + d);
                if (n.X < 0 || n.X > 4 || n.Y < 0 || n.Y > 4)
                    continue;
                var nIndex = n.X + n.Y * 5;
                var mapArr = map.ToCharArray();
                mapArr[index] = mapArr[nIndex];
                mapArr[nIndex] = ' ';
                yield return new string(mapArr);
            }
        }
        private static int Solve(string map)
        {
            if (map == Goal)
                return 0;

            var maps = new HashSet<string>();
            maps.Add(map);

            for (int step = 1; step < 11; step++)
            {
                var newMaps = new List<string>();
                foreach (var cMap in maps)
                {
                    // Check number of incorrect pieces
                    if (cMap.Where((c, i) => c != Goal[i]).Count() > 10)
                        continue;
                    foreach (var newMap in GetMoves(cMap))
                    {
                        if (maps.Contains(newMap))
                            continue;
                        if (newMap == Goal)
                            return step;
                        newMaps.Add(newMap);
                    }
                }
                maps = new HashSet<string>(newMaps);
            }
            return int.MaxValue;
        }
    }
}
