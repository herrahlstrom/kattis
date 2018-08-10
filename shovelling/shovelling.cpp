/*
https://open.kattis.com/problems/shovelling
 */
 
#include <iostream>
#include <string>
#include <vector>
#include <algorithm>
#include <math.h>
#include <stdlib.h>
#include <algorithm>

using namespace std;

const char Blocked = '#';
const char Snow = 'o';
const char Cleared = '.';

// Map data
int n, m;
int houses [4];
string map;

struct ShovelResult
{
    private:
        vector<int> _path;
        string _map;
        int _cost;
    public:
        ShovelResult(vector<int> path, string map, int cost)
        {
            _path = path;
            _map = map;
            _cost = cost;
        }
        vector<int> GetPath() { return _path; }
        string GetMap() { return _map; }
        int GetCost() { return _cost; }
};

float GetDist(int a, int b)
{
    int aX = (a % n);
    int aY = (a / n);
    int bX = (b % n);
    int bY = (b / n);
    return sqrt(abs(aX-bX) + abs(aY-bY));
}

ShovelResult Shovel(string map, int start, int end, int maxCost)
{
    if(start == end)
    {
        vector<int> path = vector<int>();
        path.push_back(start);
        return ShovelResult(path, map, 0);
    }

    vector<int> open = vector<int>();
    open.push_back(start);

    vector<int> closed = vector<int>();

    vector<int> comeFrom = vector<int>();
    for(int i=0; i<map.size(); i++)
        comeFrom.push_back(-1);

    vector<int> gScore = vector<int>();
    for(int i=0; i<map.size(); i++)
        gScore.push_back(9999);
    gScore[start] = 0;
        
    vector<float> fScore = vector<float>();
    for(int i=0; i<map.size(); i++)
        fScore.push_back(9999);
    fScore[start] = GetDist(start, end);

    while (open.size() > 0)
    {
        int c = open[0];
        int cIndex = 0;
        for(int i=1; i<open.size(); i++)
        {
            if(fScore[open[i]] < fScore[c])
            {
                c = open[i];
                cIndex = i;
            }
        }
        if(c == end)
        {
            vector<int> path = vector<int>();
            int tmp = c;
            path.push_back(c);
            while(comeFrom[tmp] != -1)
            {
                tmp = comeFrom[tmp];
                path.push_back(tmp);
            }

            int cost = 0;
            string result = string(map);
            for(int i=0; i<path.size(); i++)
            {
                int p = path[i];
                if(result[p] != Snow)
                    continue;
                result = result.substr(0,p) + Cleared + result.substr(p+1);
                cost += 1;
            }

            return ShovelResult(path, result, cost);
        }

        if(gScore[c] > maxCost)
        {
            vector<int> p = vector<int>();
            return ShovelResult(p, map, 9999);
        }

        open.erase(open.begin() + cIndex);
        closed.push_back(c);

        int xD [] = {-1, 1, 0, 0};
        int yD [] = {0, 0, -1, 1};
        for(int d = 0; d<4; d++)
        {
            int n_x = (c % n) + xD[d];
            int n_y = (c / n) + yD[d];
            if(n_x < 0 || n_x >= n || n_y < 0 || n_y >= m)
                continue;

            int neighbour = n_y * n + n_x;
       
            if(map[neighbour] == Blocked)
                continue;
            
            bool foundInClosed = false;
            for(int i=0; i<closed.size() && !foundInClosed; i++)
            {
                if(closed[i] == neighbour)
                    foundInClosed = true;
            }
            if(foundInClosed)
                continue;
            
            float newScore = gScore[c];
            if(map[neighbour] == Snow)
                newScore += 1;
            
            bool foundInOpen = false;
            for(int i=0; i<open.size() && !foundInOpen; i++)
            {
                if(open[i] == neighbour)
                    foundInOpen = true;
            }
            if(!foundInOpen)
            {
                open.push_back(neighbour);
            }
            else if(newScore >= gScore[neighbour])
            {
                continue;
            }

            comeFrom[neighbour] = c;
            gScore[neighbour] = newScore;
            fScore[neighbour] = newScore + GetDist(neighbour, end);
        }
    }

    return ShovelResult(vector<int>(), map, 9999);
}

string GetInputLine()
{
    string line;
    getline(cin, line);
    return line;
}

bool ReadMapData()
{
    map = "";
    cin >> n >> m;
    GetInputLine(); // Clear line

    if (n == 0 || m == 0)
        return false;
    for(int y=0; y<m; y++)
        map += GetInputLine();
    houses[0] = map.find('A');
    houses[1] = map.find('B');
    houses[2] = map.find('C');
    houses[3] = map.find('D');
    
    // Clear empty line
    string empty;
    getline(cin, empty);
}

string Solve()
{
    string bestMap = map;
    int bestCost = 9999;

    string mapClone;
    int cost;

    vector<int> permHouses = vector<int>();
    permHouses.push_back(houses[0]);
    permHouses.push_back(houses[1]);
    permHouses.push_back(houses[2]);
    permHouses.push_back(houses[3]);
    sort(permHouses.begin(), permHouses.end());
    do {
        mapClone = string(map);
        cost = 0;
        for(int i= 1; i<4; i++)
        {
            ShovelResult shRes = Shovel(mapClone, permHouses[0], permHouses[i], bestCost);
            mapClone = shRes.GetMap();
            cost += shRes.GetCost();
        }
        if(cost < bestCost)
        {
            bestMap = mapClone;
            bestCost = cost;
        }
    } while(next_permutation(permHouses.begin(), permHouses.end()));

    return bestMap;

    for (int piv=0; piv<map.size(); piv++)
    {
        if(map[piv] == Blocked)
            continue;
        mapClone = string(map);
        cost = 0;
        for(int i=0; i<4; i++)
        {
            if(piv == houses[i])
                continue;
            ShovelResult shRes = Shovel(mapClone, piv, houses[i], bestCost);
            if(shRes.GetPath().size() < 2)
            {
                cost += 99999;
            }
            else
            {
                mapClone = shRes.GetMap();
                cost += shRes.GetCost();
            }
            if(cost > bestCost)
                break;
        }
        if(cost < bestCost)
        {
            bestMap = mapClone;
            bestCost = cost;
        }
    }

    return bestMap;
}

int main(int argc, char *argv[])
{
    while (ReadMapData())
    {
        string result = Solve();
        cout << n << " " << m << endl;
        for(int y=0; y<m; y++)
            cout << result.substr(y*n, n) << endl;
        cout << endl;
    }

    cout << "0 0" << endl;

    return 0;
}
 