"""
https://open.kattis.com/problems/shovelling
"""

import sys
from typing import List
from queue import Queue

# Constants
BLOCKED = "#"
SNOW = "o"
CLEAR = "."
SHOVELLED = "~"


class Pos:
    def __init__(self, x: int, y: int):
        self.x = x
        self.y = y


class Rect:
    def __init__(self, width: int, height: int):
        self.width = width
        self.height = height


class Square:
    def __init__(self, pos: Pos, sign: str):
        self._pos = pos
        self._char = sign

    def getChar(self):
        return self._char

    def getNeighbours(self, size: Rect) -> List[Pos]:
        if self._pos.x > 0:
            yield Pos(self._pos.x - 1, self._pos.y)
        if self._pos.x < size.width - 1:
            yield Pos(self._pos.x + 1, self._pos.y)
        if self._pos.y > 0:
            yield Pos(self._pos.x, self._pos.y - 1)
        if self._pos.y < size.height - 1:
            yield Pos(self._pos.x, self._pos.y + 1)


class MapData:
    def __init__(self, size: Rect, data: str):
        self._size = size
        self._squares: List[Square] = [None] * len(data)
        self._dist: List[List[int]] = [None] * len(data)
        for y in range(size.height):
            for x in range(size.width):
                p = y*size.width + x
                self._squares[p] = Square(Pos(x, y), data[p])
        self.house_A = data.find('A')
        self.house_B = data.find('B')
        self.house_C = data.find('C')
        self.house_D = data.find('D')

    def printMap(self):
        print("{} {}".format(self._size.width, self._size.height))
        for y in range(self._size.height):
            line = ""
            for x in range(self._size.width):
                pos = y * self._size.width + x
                sq = self._squares[pos]
                line += str(sq.getChar())
            print(line)

    def getSquare(self, pos: Pos):
        return self._squares[pos.y*self._size.width + pos.x]

    def solve(self):
        pivot = self.getPivot()
        while pivot >= 0:
            self._squares[pivot]._char = SHOVELLED
            pivot = self.getPivot()

    def getPivot(self) -> int:
        self.calcDist(self.house_A)
        self.calcDist(self.house_B)
        self.calcDist(self.house_C)
        self.calcDist(self.house_D)
        d = [0] * len(self._squares)
        for i in range(len(self._squares)):
            d[i] += self._dist[self.house_A][i]
            d[i] += self._dist[self.house_B][i]
            d[i] += self._dist[self.house_C][i]
            d[i] += self._dist[self.house_D][i]
        minCostPivot = []
        minCostValue = 9999
        for i in range(len(d)):
            if d[i] == 0:
                return -1
            if self._squares[i].getChar() != SNOW:
                continue
            if d[i] < minCostValue:
                minCostPivot.clear()
                minCostPivot.append(i)
                minCostValue = d[i]
            elif d[i] == minCostValue:
                minCostPivot.append(i)
        return minCostPivot[0]

    def calcDist(self, start):
        dist = [9999] * len(self._squares)
        q = Queue()
        dist[start] = 0
        q.put(start)
        while not q.empty():
            next_pos = q.get()
            next: Square = self._squares[next_pos]
            for neighbour in next.getNeighbours(self._size):
                neighbour_pos = neighbour.y*self._size.width + neighbour.x
                neighbour_square = self._squares[neighbour_pos]
                if neighbour_square.getChar() == BLOCKED:
                    continue
                neighbour_cost = dist[next_pos]
                if neighbour_square.getChar() == SNOW:
                    neighbour_cost += 1
                if neighbour_cost < dist[neighbour_pos]:
                    dist[neighbour_pos] = neighbour_cost
                    q.put(neighbour_pos)
        self._dist[start] = dist


def readMaps():
    while True:
        dim = sys.stdin.readline().split()
        size = Rect(int(dim[0]), int(dim[1]))
        if size.width == 0 and size.height == 0:
            break

        map = ""
        for i in range(size.height):
            map += sys.stdin.readline()[:size.width]

        yield MapData(size, map)
        sys.stdin.readline()  # Empty line


for map in readMaps():
    map.solve()
    map.printMap()
    print()  # Empty line

print("0 0", end='')
