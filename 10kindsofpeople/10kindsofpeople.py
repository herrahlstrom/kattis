"""
https://open.kattis.com/problems/10kindsofpeople
"""

from typing import List

RESULT_NEITHER = "neither"
RESULT_DECIMAL = "decimal"
RESULT_BINARY = "binary"

rows: int
columns: int
map = []
queries: List[tuple] = []


def getNeighbours(index: int) -> List[int]:
    global columns
    yield index - columns
    if index % columns > 0:
        yield index - 1
    if index % columns < (columns-1):
        yield index + 1
    yield index + columns


def getValidNeighbours(index: int) -> List[int]:
    global map
    for n in getNeighbours(index):
        if n >= 0 and n < len(map) and map[n] == map[index]:
            yield n

def solve(map, start: int, end: int):
    if start == end:
        return RESULT_BINARY if map[start] == "0" else RESULT_DECIMAL
    if map[start] != map[end]:
        return RESULT_NEITHER
    
    visited = [False] * len(map)
    q = [start]
    while len(q) > 0:
        current = q.pop()
        if visited[current]:
            continue
        visited[current] = True
        for neighbour in getValidNeighbours(current):
            if neighbour == end:
                return RESULT_BINARY if map[start] == "0" else RESULT_DECIMAL
            if not visited[neighbour]:
                q.append(neighbour)
            
    return RESULT_NEITHER


# Dimension
rows, columns = [int(x) for x in input().split()]

# Map
for x in range(rows):
    map.extend(list(input()))

# Queries
num_queries = int(input())
for i in range(num_queries):
    y1, x1, y2, x2 = [int(x)-1 for x in input().split()]
    start = y1 * columns + x1
    end = y2 * columns + x2
    queries.append((start, end))


for query in queries:
    print(solve(map, query[0], query[1]))
