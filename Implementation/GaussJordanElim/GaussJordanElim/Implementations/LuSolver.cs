using GaussJordanElim.Abstractions;
using GaussJordanElim.Utils;

namespace GaussJordanElim.Implementations;

internal class LuSolver : IMatrixSolver
{
	public T[,] Solve<T>(T[,] matrix) where T : IMatrixEntry<T>, new()
	{
		int rowCount = matrix.GetLength(0);
		int colCount = matrix.GetLength(1);

		// Unlike Gauss-Jordan (which tolerates a rectangular/under-determined system and leaves
		// free variables in place), LU decomposition factorizes a square coefficient matrix
		// A = L * U. So the first n columns are treated as the n x n coefficient matrix, and any
		// remaining columns are right hand side (b) vectors to solve for.
		int n = rowCount;

		if (colCount < n)
		{
			throw new ArgumentException($"Matrix has {rowCount} rows but only {colCount} columns, cannot extract a square {rowCount}x{rowCount} coefficient matrix");
		}

		// 1) Copy the coefficient columns into U (it will be reduced to upper triangular form)
		// and start L as the identity (its unit diagonal is never eliminated). The augmented
		// (right hand side) columns are copied separately into rhs so they can be permuted
		// alongside A without disturbing the original input matrix.
		T[,] u = MatrixUtils.MakeEmptyMatrix<T>(n, n);
		T[,] l = MatrixUtils.MakeEmptyMatrix<T>(n, n);
		T[,] rhs = MatrixUtils.MakeEmptyMatrix<T>(n, colCount - n);

		for (int row = 0; row < n; row++)
		{
			for (int col = 0; col < n; col++)
			{
				u[row, col] = matrix[row, col].Clone();
			}

			for (int col = n; col < colCount; col++)
			{
				rhs[row, col - n] = matrix[row, col].Clone();
			}

			l[row, row] = T.One.Clone();
		}

		// 2) Doolittle's algorithm: this is the forward elimination half of Gauss-Jordan, column
		// by column, except that instead of discarding the multiplier used to zero a cell we
		// *record* it into L. Once column pivotIndex has been cleared below the diagonal, U holds
		// the (partially) upper triangular matrix and L holds the multipliers required to
		// reconstruct A = L * U.
		for (int pivotIndex = 0; pivotIndex < n; pivotIndex++)
		{
			// If the pivot cell is zero we can't use it to eliminate the column below, so - just
			// like Gauss-Jordan - we scan downward for the first usable non zero row and swap it
			// into place. The swap must move the corresponding row of L (only the multipliers to
			// the left of pivotIndex have been computed so far), U and rhs together so that every
			// row still refers to the same original equation.
			if (u[pivotIndex, pivotIndex].IsZero())
			{
				int swapRowIndex = pivotIndex + 1;
				while (swapRowIndex < n && u[swapRowIndex, pivotIndex].IsZero())
				{
					swapRowIndex++;
				}

				if (swapRowIndex == n)
				{
					throw new ArgumentException("Matrix is singular, LU decomposition cannot proceed");
				}

				for (int col = 0; col < n; col++)
				{
					(u[pivotIndex, col], u[swapRowIndex, col]) = (u[swapRowIndex, col], u[pivotIndex, col]);
				}

				for (int col = 0; col < pivotIndex; col++)
				{
					(l[pivotIndex, col], l[swapRowIndex, col]) = (l[swapRowIndex, col], l[pivotIndex, col]);
				}

				for (int col = 0; col < rhs.GetLength(1); col++)
				{
					(rhs[pivotIndex, col], rhs[swapRowIndex, col]) = (rhs[swapRowIndex, col], rhs[pivotIndex, col]);
				}
			}

			T pivotCell = u[pivotIndex, pivotIndex];

			for (int rowToEliminateIndex = pivotIndex + 1; rowToEliminateIndex < n; rowToEliminateIndex++)
			{
				T multiplier = u[rowToEliminateIndex, pivotIndex] / pivotCell;
				l[rowToEliminateIndex, pivotIndex] = multiplier;

				for (int col = pivotIndex; col < n; col++)
				{
					u[rowToEliminateIndex, col] = u[rowToEliminateIndex, col] - (multiplier * u[pivotIndex, col]);
				}
			}
		}

		// 3) With A = L * U (row swaps already baked into U, L and rhs consistently), solving
		// A x = b reduces to two triangular solves per right hand side column: first L y = b
		// (forward substitution - trivial since L has a unit diagonal), then U x = y (back
		// substitution). This is the payoff of factorizing first: L and U are computed once and
		// reused for every right hand side column.
		T[,] result = MatrixUtils.MakeEmptyMatrix<T>(n, colCount);

		for (int col = 0; col < n; col++)
		{
			result[col, col] = T.One.Clone();
		}

		for (int rhsColIndex = 0; rhsColIndex < rhs.GetLength(1); rhsColIndex++)
		{
			T[] y = new T[n];

			for (int row = 0; row < n; row++)
			{
				T sum = rhs[row, rhsColIndex].Clone();

				for (int col = 0; col < row; col++)
				{
					sum = sum - (l[row, col] * y[col]);
				}

				y[row] = sum;
			}

			T[] x = new T[n];

			for (int row = n - 1; row >= 0; row--)
			{
				T sum = y[row].Clone();

				for (int col = row + 1; col < n; col++)
				{
					sum = sum - (u[row, col] * x[col]);
				}

				x[row] = sum / u[row, row];
			}

			for (int row = 0; row < n; row++)
			{
				result[row, n + rhsColIndex] = x[row];
			}
		}

		return result;
	}
}
