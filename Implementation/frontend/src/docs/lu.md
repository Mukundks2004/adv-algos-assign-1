# LU Decomposition

LU decomposition factors a matrix `A` into a lower triangular matrix `L`
(with `1`s on the diagonal) and an upper-triangular matrix `U`, such that
`A = L * U`. Solving `Ax = b` then happens in two super fast triangular steps.

1. First, eliminate below each pivot in `U`, recording the multiplier used for each row in the corresponding position of `L`. Row swaps are tracked as well, since a zero pivot must be avoided. This is called decomposition since we turn `A` into `L` and `U`.
2. Forward substitution: solve `Ly = b` for `y`, top to bottom
3. Back substitution: solve `Ux = y` for `x`, bottom to top
