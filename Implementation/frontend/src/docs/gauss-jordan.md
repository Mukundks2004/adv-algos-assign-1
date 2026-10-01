# Gauss-Jordan Elimination

Gauss-Jordan elimination reduces an augmented matrix `[A|b]` to reduced
row-echelon form (RREF) using three elementary row operations:

1. **Swap** two rows.
2. **Normalize** a row by dividing it by its pivot value.
3. **Eliminate** every other row's entry in the pivot column, using the
   pivot row.

Once every column that has a pivot is reduced to a single `1` with zeros
above and below it, the right-hand side column directly contains the
solution.

_Add diagrams/screenshots here to illustrate each step._
