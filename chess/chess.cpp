/*
https://open.kattis.com/problems/chess
*/

#include <iostream>
#include <string>
#include <vector>
#include <math.h>   // sqrt
#include <stdlib.h> // abs

using namespace std;

struct Point
{
  public:
    Point(char x, char y)
        : x(x), y(y)
    {
    }
    Point()
    {
    }
    char x;
    char y;
};

float GetDistance(Point a, Point b)
{
    int x = abs(((int)a.x - 65) - ((int)b.x - 65));
    int y = abs(((int)a.y - 48) - ((int)b.y - 48));
    return sqrt(x * x + y * y);
}

vector<Point> GetPossibleMoves(Point pos)
{
    vector<Point> candidates, result;
    for (int i = 1; i < 8; i++)
    {
        candidates.push_back(Point((int)pos.x + 1 * i, (int)pos.y + 1 * i));
        candidates.push_back(Point((int)pos.x + 1 * i, (int)pos.y - 1 * i));
        candidates.push_back(Point((int)pos.x - 1 * i, (int)pos.y + 1 * i));
        candidates.push_back(Point((int)pos.x - 1 * i, (int)pos.y - 1 * i));
    }
    for (int i = 0; i < candidates.size(); i++)
    {
        if (candidates[i].x < 'A' || candidates[i].x > 'H')
            continue;
        if (candidates[i].y < '1' || candidates[i].y > '8')
            continue;
        result.push_back(candidates[i]);
    }
    return result;
}

vector<Point> Solve(Point pos, Point goal, vector<Point> path)
{
    if (path.size() > 8)
        return vector<Point>();
    float dist = GetDistance(pos, goal);
    if (dist == 0)
        return path;
    Point bestPos = Point(' ',' ');
    float bestDist = 999;
    vector<Point> candidates = GetPossibleMoves(pos);
    for (int i = 0; i < candidates.size(); i++)
    {
        Point newPos = candidates[i];
        float newDist = GetDistance(newPos, goal);
        if (newDist < bestDist)
        {
            bestPos = newPos;
            bestDist = newDist;
        }
    }
    vector<Point> newPath;
    for (int i = 0; i < path.size(); i++)
        newPath.push_back(path[i]);
    newPath.push_back(bestPos);
    return Solve(bestPos, goal, newPath);
}

int main(int argc, char *argv[])
{
    int n;
    cin >> n;

    for (int i = 0; i < n; i++)
    {
        vector<Point> result;
        vector<Point> path;

        Point a, b;

        cin >> a.x >> a.y;
        cin >> b.x >> b.y;

        path.push_back(a);
        result = Solve(a, b, path);

        if (result.size() == 0)
        {
            cout << "Impossible" << endl;
        }
        else
        {
            cout << result.size() - 1 << " ";
            for (int i = 0; i < result.size(); i++)
                cout << result[i].x << " " << result[i].y << " ";
            cout << endl;
        }
    }
}