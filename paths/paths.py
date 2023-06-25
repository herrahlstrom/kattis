"""
https://open.kattis.com/problems/paths
"""

# n: number of vertices
# m: number of edges
# k: number of different colors
n, m, k = [int(x) for x in input().split()]

colors = [1 << (int(x) - 1) for x in input().split()]
max_color_value = (1 << k) - 1

edges = [[] for x in range(n)]

# Read all edges
for i in range(m):
    a, b = [int(x) for x in input().split()]
    edges[a - 1].append(b)
    edges[b - 1].append(a)


result = 0
queue = []
for i in range(n):
    queue.append((i + 1, colors[i]))
    while len(queue) > 0:
        vertex, passed_colors = queue.pop()
        for next in edges[vertex - 1]:
            next_passed_colors = passed_colors | colors[next - 1]
            if passed_colors == next_passed_colors:
                continue
            if next_passed_colors < max_color_value:
                queue.append((next, next_passed_colors))
            result += 1

print(result)
