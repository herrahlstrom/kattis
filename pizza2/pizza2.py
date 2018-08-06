import sys
import math

input = sys.stdin.readline().strip()
if len(input) > 0:
    rc = input.split(" ")
    r = int(rc[0])
    c = int(rc[1])
    circle_total = r * r * math.pi
    circle_chees = (r - c) * (r - c) * math.pi
    print((circle_chees/circle_total)*100)
