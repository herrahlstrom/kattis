"""
https://open.kattis.com/problems/haypoints
"""

mn = input().split()

# number of words in the Hay Point dictionary
m = int(mn[0])

# number of job descriptions
n = int(mn[1])

hay_points = {}

for i in range(m):
    line = input().split()
    hay_points[line[0]] = int(line[1])

for i in range(n):
    sum = 0
    line = ""
    while True:
        line = input()
        if line == ".":
            print(sum)
            break
        for word in line.split():
            if word in hay_points:
                sum += hay_points[word]
        