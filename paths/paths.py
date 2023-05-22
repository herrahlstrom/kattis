"""
https://open.kattis.com/problems/paths
"""

import queue
import time
from dataclasses import dataclass
from typing import List

# number of vertices
n = 0

# number of edges
m = 0

# number of different colors
k = 0

colors = []
edges = []

@dataclass
class QItem:
    vertex: int
    colors: List[bool]

def readInput():
    global n, m, k, colors, edges

    nmk = input().split()

    n = int(nmk[0])
    m = int(nmk[1])
    k = int(nmk[2])

    colors = [0]*n

    color_input = input().split()
    for i in range(len(color_input)):
        colors[i] = int(color_input[i])

    edges = [None] * n
    for i in range(n):
        edges[i] = []

    # Read all edges
    for i in range(m):
        line = input().split()
        a = int(line[0])
        b = int(line[1])

        edges[a-1].append(b)
        edges[b-1].append(a)

readInput()

result = 0

time_start = time.time()

q = queue.Queue()
for i in range(0, n):
    color_map = [False] * 5
    color_map[colors[i] - 1] = True
    item = QItem(i + 1, color_map)
    q.put_nowait(item)

time_init = time.time()

while not q.empty():
    current = q.get()
    for next in edges[current.vertex - 1]:
        next_color = colors[next - 1]
        if current.colors[next_color - 1]:
            continue
        color_map = current.colors.copy()
        color_map[next_color - 1] = True
        if not all(color_map):
            q.put_nowait(QItem(next, color_map))
        result += 1

time_completed = time.time()

print(result)

print( time_init - time_start, time_completed - time_init )
