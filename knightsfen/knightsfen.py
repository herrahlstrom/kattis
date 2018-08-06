"""
https://open.kattis.com/problems/knightsfen
"""

import sys

target = "111110111100 110000100000"


def solve(boards, depth):

    # Check if not solvable
    if depth > 10:
        return None
    
    # Check if solved
    for board in boards:
        if board == target:
            return depth

    # Make all possible moves
    new_boards = list()
    for board in boards:
        blank = board.index(' ')
        for candidate in get_valid_moves(blank):
            new_boards.append(get_new_board(board, blank, candidate))

    # Add new board to boards
    for board in new_boards:
        if not board in boards:
            boards.append(board)

    # Check which boards that is still solvable
    boards = [board for board in boards if get_distance(board) + depth <= 10]

    return solve(boards, depth + 1)


def get_valid_moves(index):
    pos = (index % 5, index // 5)
    for delta in [(-1, -2), (-1, 2), (-2, -1), (-2, 1), (1, -2), (1, 2), (2, -1), (2, 1)]:
        newpos = (delta[0] + pos[0], delta[1] + pos[1])
        if newpos[0] < 0 or newpos[0] > 4:
            continue
        if newpos[1] < 0 or newpos[1] > 4:
            continue
        yield newpos[0] + newpos[1] * 5


def get_distance(board):
    distance = 0
    for i in range(25):
        if board[i] != target[i]:
            distance += 1
    return distance

def get_new_board(board, a, b):
    if a > b:
        a, b = b, a
    return \
        board[0:a] + \
        board[b] + \
        board[a + 1: b] + \
        board[a] + \
        board[b + 1:]


def main():
    n = int(sys.stdin.readline())
    for i in range(n):
        board = ""
        for j in range(5):
            board += sys.stdin.readline()[:5]
        moves = solve([board], 0)
        if moves is None or moves > 10:
            print("Unsolvable in less than 11 move(s).")
        else:
            print("Solvable in {0} move(s).".format(moves))


if __name__ == "__main__":
    main()
