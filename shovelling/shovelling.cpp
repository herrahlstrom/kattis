/*
https://open.kattis.com/problems/shovelling
 */
 
#include <iostream>
#include <string>
#include <vector>
#include <queue>
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

vector<int> GetNeighbours(int index)
{
    vector<int> result = vector<int>();
    int x = index % n;
    int y = index / n;
    int xD [] = {-1, 1, 0, 0};
    int yD [] = {0, 0, -1, 1};
    for(int d = 0; d<4; d++)
    {
        int n_x = x + xD[d];
        int n_y = y + yD[d];
        if(n_x < 0 || n_x >= n || n_y < 0 || n_y >= m)
            continue;
        result.push_back(n_x + n_y * n);
    }
    return result;
}

ShovelResult Shovel(string map, int start, int end, int maxCost)
{
    if(start == end)
    {
        vector<int> path = vector<int>();
        path.push_back(start);
        return ShovelResult(path, map, 0);
    }

    int mapLength = map.length();

    queue<int> q = queue<int>();
    q.push(start);
    
    vector<vector<int> > paths = vector<vector<int> >();
    for(int i=0; i<mapLength; i++)
    {
        vector<int> v = vector<int>();
        if(i == start)
            v.push_back(start);
        paths.push_back(v);
    }

    vector<int> costs = vector<int>();
    for(int i=0; i<mapLength; i++)
    {
        if(i == start)
            costs.push_back(0);
        else
            costs.push_back(9999);
    }

    while(q.size() > 0)
    {
        int c = q.front();
        q.pop();
        vector<int> neighbours = GetNeighbours(c);
        for(int i=0; i<neighbours.size(); i++)
        {
            int neighbour = neighbours[i];

            if(map[neighbour] == Blocked)
                continue;
            int newCost = costs[c];
            if(map[neighbour] == Snow)
                newCost += 1;
            if(newCost > maxCost)
                continue;
            if(newCost < costs[neighbour])
            {
                costs[neighbour] = newCost;

                vector<int> newPath = vector<int>(paths[c]);
                newPath.push_back(neighbour);
                paths[neighbour] = newPath;

                if(neighbour == end)
                    maxCost = newCost;
                else
                    q.push(neighbour);
            }
        }
    }

    int cost = 0;
    string result = string(map);
    for(int i=0; i<paths[end].size(); i++)
    {
        int p = paths[end][i];
        if(result[p] != Snow)
            continue;
        result = result.substr(0,p) + Cleared + result.substr(p+1);
        cost += 1;
    }

    return ShovelResult(paths[end], result, cost);
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

    mapClone = string(map);
    cost = 0;
    for(int i= 1; i<4; i++)
    {
        ShovelResult shRes = Shovel(mapClone, houses[0], houses[i], bestCost);
        mapClone = shRes.GetMap();
        cost += shRes.GetCost();
    }
    if(cost < bestCost)
    {
        bestMap = mapClone;
        bestCost = cost;
    }

    // Calc a simple (fast) solution to get a upper bound of shovelling costs
    mapClone = string(map);
    cost = 0;
    for(int i= 1; i<4; i++)
    {
        ShovelResult shRes = Shovel(mapClone, houses[0], houses[i], bestCost);
        mapClone = shRes.GetMap();
        cost += shRes.GetCost();
    }
    bestMap = mapClone;
    bestCost = cost;

    for (int piv=0; piv<map.size(); piv++)
    {
        if(map[piv] == Blocked)
            continue;
        int x = piv % n;
        int y = piv / n;
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
 