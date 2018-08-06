"""
https://open.kattis.com/problems/trainpassengers
"""

import sys

cn = sys.stdin.readline().split(" ")
c = int(cn[0])
n = int(cn[1])
train = 0

try:
    for i in range(0, n):
        les = sys.stdin.readline().split(" ")
        l = int(les[0])#Left
        e = int(les[1])#Enter
        s = int(les[2])#Stay
        if l > train:
            raise Exception
        train -= l
        train += e
        if train > c:
            raise Exception
        if s > 0 and train < c:
            raise Exception
    if train > 0:
        raise Exception
    print("possible")
except:
    print("impossible")
