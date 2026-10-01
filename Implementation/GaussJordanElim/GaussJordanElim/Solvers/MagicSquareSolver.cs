using GaussJordanElim.MatrixEntities;
using GaussJordanElim.Utils;

namespace GaussJordanElim.Solvers;

internal class MagicSquareSolver : IMatrixSolver
{
	public T[,] Solve<T>(T[,] square) where T : IMatrixEntry<T>, new()
	{
		// Size is also referred to as 'n'
		var size = square.GetLength(0);
		var cols = square.GetLength(1);

		if (cols != size)
		{
			throw new ArgumentException($"Magic square is not a square size, instead has {size} rows and {cols} columns.");
		}

		var result = MatrixUtils.MakeEmptyMatrix<T>(2 * size + 2, size * size + 1);

		// Entering information from the square into the matrix: the first n rows are the rows
		// from the square
		for (int magicSquareRowIndex = 0; magicSquareRowIndex < size; magicSquareRowIndex++)
		{
			for (int magicSquareColIndex = 0; magicSquareColIndex < size; magicSquareColIndex++)
			{
				var variablePosition = size * magicSquareRowIndex + magicSquareColIndex;
				var variableQuantity = T.One.Clone();
				result[magicSquareRowIndex, variablePosition] = variableQuantity;
			}
		}

		// Second n rows are the columns from the square
		for (int colInfo = 0; colInfo < size; colInfo++)
		{
			for (int colIndex = 0; colIndex < size; colIndex++)
			{
				var variablePosition = size * colInfo + colIndex;
				var variableQuantity = T.One.Clone();
				result[size + colInfo, variablePosition] = variableQuantity;
			}
		}

		// Last 2 rows are the diagonals from the square
		for (int rowAndCol = 0; rowAndCol < size; rowAndCol++)
		{
			result[2 * size, (size + 1) * rowAndCol] = T.One.Clone();
		}

		for (int rowAndCol = 0; rowAndCol < size; rowAndCol++)
		{
			result[2 * size + 1, (size - 1) * (rowAndCol + 1)] = T.One.Clone();
		}

		// Column on the end for -1 to balance the equation that a + b + c = sum,
		// the `-1` is for the variable sum

		for (int row = 0; row < 2 * size + 2; row++)
		{
			result[row, size * size] = T.Zero.Clone() - T.One.Clone();
		}

		var adjointVector = MatrixUtils.MakeEmptyMatrix<T>(size * 2 + 2, 1);


		var newColCount = MatrixUtils.CountNulls(result);

		var solver = new GaussJordanSolver();
		return solver.Solve(square);
	}
}
