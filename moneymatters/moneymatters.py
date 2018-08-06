"""
https://open.kattis.com/problems/moneymatters
"""
import sys
import queue

nm = sys.stdin.readline().split(" ")
n = int(nm[0])
m = int(nm[1])

balance = [0] * n
friends = [[] for x in range(n)]

for i in range(0, n):
    balance[i] = int(sys.stdin.readline())
for i in range(0, m):
    ff = sys.stdin.readline().split(" ")
    f1 = int(ff[0])
    f2 = int(ff[1])
    friends[f1].append(f2)
    friends[f2].append(f1)

try:
    visited = [False] * n
    for i in range(0, n):
        if visited[i]:
            continue
        s = 0
        q = queue.Queue()
        q.put_nowait(i)
        while not q.empty():
            next = q.get_nowait()
            if visited[next]:
                continue
            s += balance[next]
            visited[next] = True
            for f in friends[next]:
                if not visited[f]:
                    q.put_nowait(f)
        if s != 0:
            raise UserWarning()            
    print("POSSIBLE")
except UserWarning:
    print("IMPOSSIBLE")
