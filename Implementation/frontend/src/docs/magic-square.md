# Magic Square Solver

A magic square is an `n x n` grid of numbers where every row, every column, and both diagonals sum to the same value. Given a square with some cells filled in and others left blank, the blanks can be found by treating each one as a variable in a system of linear equations:

- One equation per row (`sum of row = magic sum`)
- One equation per column
- One equation per diagonal

This system is then solved with Gauss Jordan elimination to find the value of every blank cell.
