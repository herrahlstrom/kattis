"""
Honeycomb Walk
https://open.kattis.com/problems/honey
"""

modulus = 28


class HexPos:
    def __init__(self, a, b, c):
        self._a = a
        self._b = b
        self._c = c

    def __add__(self, other):
        return HexPos(self._a + other._a, self._b + other._b, self._c + other._c)

    def __hash__(self) -> int:
        return hash((self._a, self._b, self._c))

    def __eq__(self, other) -> bool:
        return other._a == self._a and other._b == self._b and other._c == self._c

    def canReachStart(self, stepsLeft: int) -> bool:
        if abs(self._a) > stepsLeft:
            return False
        if abs(self._b) > stepsLeft:
            return False
        if abs(self._c) > stepsLeft:
            return False
        return True


class HexBlock:
    def __init__(self, pos: HexPos, count: int):
        self._pos = pos
        self._count = count

    def getCount(self):
        return self._count

    def getPos(self) -> HexPos:
        return self._pos


directions = [
    HexPos(-1, +1, 0),
    HexPos(+1, -1, 0),
    HexPos(-1, 0, +1),
    HexPos(+1, 0, -1),
    HexPos(0, -1, +1),
    HexPos(0, +1, -1),
]

start = HexPos(0, 0, 0)

number_of_tests = int(input())
for i in range(number_of_tests):
    steps = int(input())
    blocks = [HexBlock(start, 1)]
    while steps > 0:
        buffer = {}
        for block in blocks:
            for d in directions:
                next_pos: HexPos = block.getPos() + d
                if not next_pos.canReachStart(steps):
                    continue
                current_count = buffer.get(next_pos, 0)
                buffer[next_pos] = current_count + block.getCount()
        blocks.clear()
        for pos in buffer:
            blocks.append(HexBlock(pos, buffer[pos]))
        steps -= 1
    result = 0
    for block in blocks:
        if block.getPos() == start:
            result += block.getCount()
    print(result)
