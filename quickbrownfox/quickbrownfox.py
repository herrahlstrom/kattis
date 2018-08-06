"""
https://open.kattis.com/problems/quickbrownfox
"""
import sys

alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".lower()

n = int(sys.stdin.readline())
for i in range(0, n):
    text = sys.stdin.readline().lower()
    missing = alphabet
    for c in text:
        if c in missing:
            missing = missing.replace(c, '')
        if not missing:
            break
    if missing:
        print("missing " + missing)
    else:
        print("pangram")
    