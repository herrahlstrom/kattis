"""
https://open.kattis.com/problems/shovelling
"""

import sys
import time
import random

# Constants
BLOCKED = "#"
SNOW = "o"
CLEAR = "."

# Map data
n = 0
m = 0
houses = []
map = ""
neighbours = []

# DEBUG
print_debug = True

def get_heuristicscore(a, b):
    global n
    return abs((a % n) - (b % n)) + abs((a // n) - (b // n))

def read_mapdata():
    global n, m, map, houses, neighbours

    dim = sys.stdin.readline().split()
    n = int(dim[0])
    m = int(dim[1])
    map = ""
    if n == 0 or m == 0:
        return False
    for i in range(m):
        map += sys.stdin.readline()[:n]
    houses = [map.find("A"), map.find("B"), map.find("C"), map.find("D")]
    sys.stdin.readline() # Empty line
    #cache-data
    neighbours = [None] * len(map)
    return True

def get_neighbours(index):
    global neighbours
    if not neighbours[index] is None:
        return neighbours[index]
    neighbours[index] = []
    x,y = index % n, index // n
    for neighbour in [(x-1, y), (x+1, y), (x, y-1), (x, y+1)]:
        if neighbour[0] < 0 or neighbour[0] >= n: continue
        if neighbour[1] < 0 or neighbour[1] >= m: continue
        neighbours[index].append(neighbour[0] + (neighbour[1] * n))
    return neighbours[index]


def get_path(map, start, end, maxcost):
    if start == end:
        return [start]
    open = [start]
    closed = []
    source = [-1] * len(map)
    g_score = [9999] * len(map)
    g_score[start] = 0
    f_score = [9999] * len(map)
    f_score[start] = get_heuristicscore(start, end)
    while open:
        c = open[0]
        for o in open:
            if f_score[o] < f_score[c]:
                c = o
        if c == end:
            path = [c]
            tmp = c
            while source[tmp] != -1:
                tmp = source[tmp]
                path.append(tmp)
            path.reverse()
            return path
        if g_score[c] > maxcost:
            return None
        open.remove(c)
        closed.append(c)
        for neighbour in get_neighbours(c):
            if map[neighbour] == BLOCKED:
                continue
            if neighbour in closed:
                continue
            newscore = g_score[c]
            if map[neighbour] == SNOW:
                newscore += 1
            if not neighbour in open:
                open.append(neighbour)
            elif newscore >= g_score[neighbour]:
                continue
            source[neighbour] = c
            g_score[neighbour] = newscore
            f_score[neighbour] = newscore + get_heuristicscore(neighbour, end)
    return None

def shovel(map, path):
    cost = 0
    maplist = list(map)
    for p in [p for p in path if maplist[p] == SNOW]:
        maplist[p] = CLEAR
        cost += 1
    map = "".join(maplist)
    return (map, cost)

def solve():
    best_cost = 9999
    best_map = map

    # Find a solution for every point on the map
    for piv in range(len(map)):
        if map[piv] == BLOCKED: continue
        map_clone = map
        cost = 0
        for h in houses:
            path = get_path(map_clone, piv, h, best_cost)
            if path is None:
                cost += 99999
            else:
                (map_clone, pathcost) = shovel(map_clone, path)
                cost += pathcost
            if cost >= best_cost:
                break
        if cost < best_cost:
            best_cost = cost
            best_map = map_clone
    return best_map

def shovelling():
    while read_mapdata():
        start = time.time()
        result_map = solve()
        print("{} {}".format(n, m))
        for y in range(m):
            if print_debug:
                print(map[y * n : (y + 1) * n], result_map[y * n : (y + 1) * n], sep="  ->  ")
            else:
                print(result_map[y * n : (y + 1) * n])
        print()
        if print_debug:
            print("{:.2f} sec".format(time.time() - start))
    print("0 0")

def shovelling_cost():
    while read_mapdata():
        start = time.time()
        result_cost = solve().count(".") - map.count(".")
        print(result_cost)
        if print_debug:
            print("{:.2f} sec".format(time.time() - start))

shovelling_cost()