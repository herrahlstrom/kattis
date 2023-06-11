"""
https://open.kattis.com/problems/paths
"""

# n: number of vertices
# m: number of edges
# k: number of different colors
n, m, k = [int(x) for x in input().split()]

colors = [int(x) for x in input().split()]

edges = [[] for x in range(n)]

# Read all edges
for i in range(m):
    a, b = [int(x) for x in input().split()]
    edges[a - 1].append(b)
    edges[b - 1].append(a)


result = 0
queue = []
for i in range(n):
    queue.append((i + 1, 1 << colors[i]))
    while len(queue) > 0:
        vertex, passed_colors = queue.pop()
        for next in edges[vertex - 1]:
            next_color_bit = 1 << colors[next - 1]
            if passed_colors & next_color_bit != next_color_bit:
                queue.append((next, passed_colors | next_color_bit))
                result += 1

print(result)
