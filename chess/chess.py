"""
https://open.kattis.com/problems/chess
"""

import sys
from math import sqrt


def solve(pos, goal, walk):
    if len(walk) > 8:
        return None
    dist = get_distance(pos, goal)
    if dist == 0:
        return walk
    best_pos = None
    best_dist = 999
    for new_pos in get_possible_moves(pos):
        new_dist = get_distance(new_pos, goal)
        if new_dist < best_dist:
            best_pos = new_pos
            best_dist = new_dist
    return solve(best_pos, goal, walk + best_pos[0] + best_pos[1])


def get_possible_moves(pos):
    valid_x = ("A", "B", "C", "D", "E", "F", "G", "H")
    valid_y = ("1", "2", "3", "4", "5", "6", "7", "8")
    for d in [(1, 1), (1, -1), (-1, 1), (-1, -1)]:
        for i in range(1, 8):
            new_pos = (chr(ord(pos[0]) + d[0] * i),
                       str(int(pos[1]) + d[1] * i))
            if not new_pos[0] in valid_x:
                continue
            if not new_pos[1] in valid_y:
                continue
            yield new_pos


def get_distance(a, b):
    x = abs((ord(a[0]) - 65) - (ord(b[0]) - 65))
    y = abs(int(a[1]) - int(b[1]))
    return sqrt(x**2 + y**2)


def main():
    n = int(sys.stdin.readline())
    for i in range(n):
        arr = sys.stdin.readline().split()
        a = (arr[0], arr[1])
        b = (arr[2], arr[3])
        moves = solve(a, b, a[0] + a[1])
        if moves is None:
            print("Impossible")
        else:
            print(len(moves) // 2 - 1, end=' ')
            for c in moves:
                print(c, end=' ')
            print()


if __name__ == "__main__":
    main()
