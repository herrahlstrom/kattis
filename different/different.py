"""
A Different Problem
https://open.kattis.com/problems/different
"""

from sys import stdin

for line in stdin:
    a, b = [int(x) for x in line.split()]
    if a > b:
        print(a-b)
    else:
        print(b-a)