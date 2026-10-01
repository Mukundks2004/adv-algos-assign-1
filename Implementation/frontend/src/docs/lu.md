# LU Decomposition

LU decomposition factors a matrix `A` into a lower-triangular matrix `L`
(with `1`s on the diagonal) and an upper-triangular matrix `U`, such that
`A = L * U`. Solving `Ax = b` then happens in two cheap triangular steps:

1. **Decompose**: eliminate below each pivot in `U`, recording the
   multiplier used for each row in the corresponding position of `L`.
   Row swaps are tracked as well, since a zero pivot must be avoided.
2. **Forward substitution**: solve `Ly = b` for `y`, top to bottom.
3. **Back substitution**: solve `Ux = y` for `x`, bottom to top.

_Add diagrams/screenshots here to illustrate each step._
