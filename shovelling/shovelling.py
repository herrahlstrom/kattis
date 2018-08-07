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

#Debug
show_debug = False
fake_maps = 1 if show_debug else 0

def gen_randomdata(n, m):
    chars = ["A", "B", "C", "D"]
    chars += list("." * int(m*n*0.1))
    chars += list("#" * int(m*n*0.1))
    chars += list("o" * (m*n - len(chars)))
    for i in range(m*n):
        r = random.randint(0, m*n-1)
        chars[i], chars[r] = chars[r], chars[i]
    return "".join(chars)

def read_mapdata():
    global n, m, map, houses
    global fake_maps

    if fake_maps > 0:
        n = 20
        m = 20
        map = gen_randomdata(n, m)
        houses = [map.find("A"), map.find("B"), map.find("C"), map.find("D")]
        fake_maps -= 1
        return True

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
    return True

def get_neighbours(index):
    pos = (index % n, index // n)
    for neighbour in [(pos[0]-1, pos[1]), (pos[0]+1, pos[1]), (pos[0], pos[1]-1), (pos[0], pos[1]+1)]:
        if neighbour[0] < 0 or neighbour[0] >= n: continue
        if neighbour[1] < 0 or neighbour[1] >= m: continue
        yield neighbour[0] + (neighbour[1] * n)

def get_path(map, start, end, maxcost):
    if start == end:
        return [start]
    q = [start]
    paths = [None] * len(map)
    cost = [99999] * len(map)
    paths[start] = [start]
    cost[start] = 0
    while q:
        c = q[0]
        q = q[1:]
        for neighbour in get_neighbours(c):
            if map[neighbour] == BLOCKED:
                continue
            new_cost = cost[c]
            if map[neighbour] == SNOW:
                new_cost += 1
            if new_cost > maxcost:
                continue
            if new_cost < cost[neighbour]:
                cost[neighbour] = new_cost
                paths[neighbour] = paths[c] + [neighbour]
                if neighbour == end:
                    maxcost = new_cost
                else:
                    q.append(neighbour)
    if paths[end] is None:
        return None
    return paths[end]

def shovel(map, path):
    cost = 0
    maplist = list(map)
    for p in [p for p in path if maplist[p] == SNOW]:
        maplist[p] = CLEAR
        cost += 1
    map = "".join(maplist)
    return (map, cost)

def solve():
    best_cost = 99999
    best_map = map

    # Calc a simple (fast) solution to get a upper bound of shovelling costs
    map_clone = map
    cost = 0
    for h in houses[1:]:
        path = get_path(map_clone, houses[0], h, best_cost)
        if path is None:
            raise Exception("House {} can't reach house {}".format(houses[0], h))
        (map_clone, pathcost) = shovel(map_clone, path)
        cost += pathcost
    best_cost = cost
    best_map = map_clone

    min_x = 20
    min_y = 20
    max_x = 0
    max_y = 0
    for h in houses:
        (x, y) =  (h % n, h // n)
        if x < min_x: min_x = x
        if y < min_y: min_y = y
        if x > max_x: max_x = x
        if y > max_y: max_y = y

    # Find a solution for every point on the map
    for piv in range(len(map)):
        if map[piv] == BLOCKED: continue
        (x, y) =  (piv % n, piv // n)
        if x < min_x or x > max_x: continue
        if y < min_y or y > max_y: continue
        map_clone = map
        cost = 0
        for h in houses:
            path = get_path(map_clone, piv, h, best_cost)
            if path is None:
                pathcost += 9999
            else:
                (map_clone, pathcost) = shovel(map_clone, path)
            cost += pathcost
            if cost > best_cost:
                break
        if cost < best_cost:
            best_cost = cost
            best_map = map_clone
    return (best_map, best_cost)

while read_mapdata():

    start = time.time()
    (result_map, result_cost) = solve()
    elapsed = time.time() - start

    if show_debug:
        print("{} {} (cost: {}, {:.3f}s)".format(n, m, result_cost, elapsed))
        for y in range(m):
            print(map[y * n : (y + 1) * n], result_map[y * n : (y + 1) * n], sep="  ->  ")
    else:
        print("{} {}".format(n, m))
        for y in range(m):
            print(result_map[y * n : (y + 1) * n])
    print()

print("0 0")
