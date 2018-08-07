/*
https://open.kattis.com/problems/shovelling
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shovelling
{
    class Program
    {
        const char Blocked = '#';
        const char Snow = 'o';
        const char Cleared = '.';
        
        // Map data
        int n;
        int m;
        int[] houses;
        string map;

        bool ReadMapData()
        {
            var dim = Console.ReadLine().Split();
            n = int.Parse(dim[0]);
            m = int.Parse(dim[1]);
            map = "";
            if (n==0 && m==0)
                return false;
            for(int i=0;i<m;i++)
                map += Console.ReadLine().Substring(0, n);
            houses = new[] { map.IndexOf('A'), map.IndexOf('B'), map.IndexOf('C'), map.IndexOf('D') };
            Console.ReadLine();
            return true;
        }

        IEnumerable<int> GetNeighbours(int index)
        {
            var x = index % n;
            var y = index / n;
            var xD = new[] {-1, 1, 0, 0};
            var yD = new[] {0, 0, -1, 1};
            for(int d = 0; d<4; d++)
            {
                var neighbour = Tuple.Create(x + xD[d], y + yD[d]);
                if(neighbour.Item1 < 0 || neighbour.Item1 >= n)
                    continue;
                if(neighbour.Item2 < 0 || neighbour.Item2 >= m)
                    continue;
                yield return neighbour.Item1 + neighbour.Item2 * n;
            }
        }

        IList<int> GetPath(string map, int start, int end, int maxCost)
        {
            if(start == end)
                return new[]{start};
            List<int> queue = new List<int>{start};
            var paths = new IList<int>[map.Length];
            var costs = Enumerable.Repeat(9999, map.Length).ToArray();
            paths[start] = new[]{start};
            costs[start] = 0;
            while(queue.Any())
            {
                var current = queue[0];
                queue.RemoveAt(0);
                foreach(var neighbour in GetNeighbours(current))
                {
                    if (map[neighbour] == Blocked)
                        continue;
                    int newCost = costs[current] + (map[neighbour] == Snow ? 1 : 0);
                    if (newCost > maxCost)
                        continue;
                    if (newCost < costs[neighbour])
                    {
                        costs[neighbour] = newCost;
                        paths[neighbour] = paths[current].Concat(new[] {neighbour}).ToList();
                        if (neighbour == end)
                            maxCost = newCost;
                        else
                            queue.Add(neighbour);
                    }
                }
            }
            return paths[end];
        }

        static Tuple<string, int> Shovel(string map, IEnumerable<int> path)
        {
            int cost = 0;
            var result = map.ToCharArray();
            foreach(int p in path)
            {
                if (result[p] != Snow)
                    continue;
                result[p] = Cleared;
                cost += 1;
            }
            return Tuple.Create(new string(result), cost);
        }

        Tuple<string, int> Solve()
        {
            string bestMap = map;
            int bestCost = 9999;

            // Calc a simple (fast) solution to get a upper bound of shovelling costs
            var mapClone = map;
            int cost = 0;
            foreach(var h in houses.Skip(1))
            {
                var path = GetPath(mapClone, houses[0], h, bestCost);
                if (path == null)
                    throw new Exception($"House {houses[0]} can't reach house {h}");
                var result = Shovel(mapClone, path);
                mapClone = result.Item1;
                cost += result.Item2;
            }
            bestMap = mapClone;
            bestCost = cost;

            // todo: sätt min/max bouderies
            int minX = 20;
            int minY = 20;
            int maxX = 0;
            int maxY = 0;
            foreach(var h in houses)
            {
                int x = h % n;
                int y = h / n;
                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
            }


            // Find a solution for every point on the map
            for(int piv = 0; piv < map.Length; piv++)
            {
                if (map[piv] == Blocked)
                    continue;
                int x = piv % n;
                int y = piv / n;
                if (x < minX || x > maxX)
                    continue;
                if (y < minY || y > maxY)
                    continue;
                mapClone = map;
                cost = 0;
                foreach(var h in houses)
                {
                    var path = GetPath(mapClone, piv, h, bestCost);
                    if (path == null)
                        cost += 9999;
                    else
                    {
                        var result = Shovel(mapClone, path);
                        mapClone = result.Item1;
                        cost += result.Item2;
                    }
                    if (cost > bestCost)
                        break;
                }
                if (cost < bestCost)
                {
                    bestCost = cost;
                    bestMap = mapClone;
                }
            }
            return Tuple.Create(bestMap, bestCost);
        }

        static void Main(string[] args)
        {
            var p = new Program();
            while(p.ReadMapData())
            {
                var result = p.Solve();
                Console.WriteLine($"{p.n} {p.m}");
                for(int y=0; y<p.m; y++)
                    Console.WriteLine(result.Item1.Substring(y * p.n, p.n));
                Console.WriteLine();
            }

            Console.WriteLine("0 0");
        }
    }
}
