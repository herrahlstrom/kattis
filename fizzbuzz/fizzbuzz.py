import sys

def solve(input):
    xyn = input.split(" ")
    x = int(xyn[0])
    y = int(xyn[1])
    n = int(xyn[2])
    for i in range(1, n + 1):
        if i % x == 0 and i % y == 0:
            print("FizzBuzz")
        elif i % x == 0:
            print("Fizz")
        elif i % y == 0:
            print("Buzz")
        else:
            print(i)

input = sys.stdin.readline().strip()
if len(input) > 0:
    solve(input)
