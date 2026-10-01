# Gauss-Jordan Elimination

Gauss-Jordan elimination reduces an augmented matrix `[A|b]` (where `A` is a coefficient matrix and `b` is a constants matrix) to reduced row-echelon form (RREF) using the three elementary row operations:

1. Swapping two rows
2. Scalar multiplication of a row (a row is divided by its pivot to normalize it)
3. Elimination- every other row's entry in the pivot column is zeroed, by adding multiples of the pivot row to it.

Once every column that has a pivot is reduced to a single `1` with zeros above and below it, the right-hand side column directly contains the solution.
