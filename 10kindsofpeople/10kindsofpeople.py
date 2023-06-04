"""
https://open.kattis.com/problems/10kindsofpeople
"""

from typing import List

rows: int
columns: int
map = []


def getNeighbours(index: int) -> List[int]:
    global columns, map
    if index >= columns:
        yield index - columns
    if index % columns > 0:
        yield index - 1
    if index % columns < (columns-1):
        yield index + 1
    if(index + columns < len(map)):
        yield index + columns


def build_zones():
    global map
    zone_queue = []
    next_even = 2
    next_odd = 3
    for i in range(len(map)):
        if map[i] > 1:
            continue
        current_value = map[i]
        zone_value:int
        if map[i] % 2 == 0:
            zone_value = next_even
            next_even += 2
        else:
            zone_value = next_odd
            next_odd += 2
        zone_queue.append(i)
        while len(zone_queue) > 0:
            current = zone_queue.pop()
            map[current] = zone_value
            for neighbour in getNeighbours(current):
                if map[neighbour] == current_value:
                    zone_queue.append(neighbour)


# Dimension
rows, columns = [int(x) for x in input().split()]

# Map
for x in range(rows):
    map.extend([int(x) for x in input()])

build_zones()

# Queries
for i in range(int(input())):
    y1, x1, y2, x2 = [int(x)-1 for x in input().split()]
    start = y1 * columns + x1
    end = y2 * columns + x2
    if map[start] == map[end]:
        if map[start] % 2 == 0:
            print("binary")
        else:
            print("decimal")
    else:
        print("neither")
