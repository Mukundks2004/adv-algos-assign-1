using GaussJordanElim.MatrixEntities;
using GaussJordanElim.Utils;

namespace GaussJordanElim.Solvers;

internal class LuSolver<T> : IMatrixSolver<T> where T : IMatrixEntry<T>, new()
{
	/*
	 * Some things to keep in mind about this method:
	 * - Gauss Jordan tolerates an under determined system (and an overdetermined consistent
	 * system iirc) and leaves free variables in place. LU decomp is meant for squares, 
	 * so it will optimistically treat every column outside the n x n coefficient matrix as 
	 * vectors to solve for and if there are fewer variables than pieces of information it
	 * will fail outright
	 */
	public T[,] Solve(T[,] matrix)
	{
		int rowCount = matrix.GetLength(0);
		int colCount = matrix.GetLength(1);

		int size = rowCount;

		if (colCount < rowCount)
		{
			throw new ArgumentException($"Matrix has {rowCount} rows but only {colCount} columns, cannot extract a square {rowCount} x {rowCount} coefficient matrix");
		}

		// 1) Setup:
		// a) U is pulled directly from the right n x n of the input matrix
		// b) L is by default the identity matrix
		// c) rhs is all the vectors to solve for augmented together
		T[,] u = MatrixUtils.MakeEmptyMatrix<T>(size, size);
		T[,] l = MatrixUtils.MakeEmptyMatrix<T>(size, size);
		T[,] rhs = MatrixUtils.MakeEmptyMatrix<T>(size, colCount - size);

		for (int row = 0; row < size; row++)
		{
			for (int col = 0; col < size; col++)
			{
				u[row, col] = matrix[row, col].Clone();
			}

			for (int col = size; col < colCount; col++)
			{
				rhs[row, col - size] = matrix[row, col].Clone();
			}

			l[row, row] = T.One.Clone();
		}

		// 2) Converting L and U to lower and upper triangle form respectively via forward substitution
		// similarly to the Gauss Jordan method, we need to ensure the pivot is non zero to enable 
		// elementary row operations to clear the lower triangle of U. When a swap is made in U,
		// a corresponding swap must be made in L and RHS to ensure the equations remain consistent.
		for (int pivotIndex = 0; pivotIndex < size; pivotIndex++)
		{
			if (u[pivotIndex, pivotIndex].IsZero())
			{
				int swapRowIndex = pivotIndex + 1;
				while (swapRowIndex < size && u[swapRowIndex, pivotIndex].IsZero())
				{
					swapRowIndex++;
				}

				if (swapRowIndex == size)
				{
					throw new ArgumentException("Matrix is singular so LU decomposition cannot work");
				}

				for (int col = 0; col < size; col++)
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

			// Forward substitution algorithm is identical to Gauss Jordan
			T pivotCell = u[pivotIndex, pivotIndex];

			for (int rowToEliminateIndex = pivotIndex + 1; rowToEliminateIndex < size; rowToEliminateIndex++)
			{
				T multiplier = u[rowToEliminateIndex, pivotIndex] / pivotCell;
				l[rowToEliminateIndex, pivotIndex] = multiplier;

				for (int col = pivotIndex; col < size; col++)
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
		T[,] result = MatrixUtils.MakeEmptyMatrix<T>(size, colCount);

		for (int col = 0; col < size; col++)
		{
			result[col, col] = T.One.Clone();
		}

		for (int rhsColIndex = 0; rhsColIndex < rhs.GetLength(1); rhsColIndex++)
		{
			// Each iteration here solves one column/coefficients vector, so you can 
			// analyse the contents in isolation
			T[] y = new T[size];

			for (int row = 0; row < size; row++)
			{
				T sum = rhs[row, rhsColIndex].Clone();

				for (int col = 0; col < row; col++)
				{
					sum -= (l[row, col] * y[col]);
				}

				y[row] = sum;
			}

			T[] x = new T[size];

			for (int row = size - 1; row >= 0; row--)
			{
				T sum = y[row].Clone();

				for (int col = row + 1; col < size; col++)
				{
					sum -= (u[row, col] * x[col]);
				}

				x[row] = sum / u[row, row];
			}

			for (int row = 0; row < size; row++)
			{
				result[row, size + rhsColIndex] = x[row];
			}
		}

		return result;
	}
}
