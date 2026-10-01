using GaussJordanElim.MatrixEntities;
using GaussJordanElim.Utils;

namespace GaussJordanElim.Solvers;

internal class MagicSquareSolver<T> : IMatrixSolver<T> where T : IMatrixEntry<T>, new()
{
	public T[,] Solve(T?[,] square)
	{
		// Size is also referred to as 'n'
		var size = square.GetLength(0);
		var cols = square.GetLength(1);

		if (cols != size)
		{
			throw new ArgumentException($"Magic square is not a square size, instead has {size} rows and {cols} columns.");
		}

		var coefficientsMatrix = GenerateCoefficientsMatrixForMagicSquare(size);
		var variableCount = MatrixUtils.CountNulls(square);
		var condensedCoefficientsMatrix = MatrixUtils.MakeEmptyMatrix<T>(size * 2 + 2, variableCount + 1);

		// At this point in time, every n x n square has the same resultant matrix.
		// We can input the square specific information into this matrix by removing
		// every variable whose value is known (its cell value in the square is not null)
		// and instead subtracting its value from the adjoint column at the end of
		// the matrix. So an equation previously expressed as 1x + 1y + 1z + -1sum = 0
		// becomes 0x + 1y + 1z + -1sum = -4, and the x column can be stripped entirely
		// After this stage the matrix is solvable.
		// 
		// There might be better ways to do this, but it is simple to just create a new
		// matrix and copy everything in. This isn't the algorithm being assessed anyway,
		// This is my solution and the time/space complexity doesn't change it is just
		// unoptimized.
		var constantsVector = MatrixUtils.MakeEmptyMatrix<T>(size * 2 + 2, 1);

		for (int row = 0; row < 2 * size + 2; row++)
		{
			int writingCol = 0;
			for (int col = 0; col < size * size; col++)
			{
				var currentMagicSquareCell = square[col / size, col % size];
				if (currentMagicSquareCell is not null && coefficientsMatrix[row, col].IsOne())
				{
					constantsVector[row, 0] -= currentMagicSquareCell;
				}
				else
				{
					condensedCoefficientsMatrix[row, writingCol] = coefficientsMatrix[row, col];
				}

				if (currentMagicSquareCell is null)
				{
					writingCol++;
				}
			}

			condensedCoefficientsMatrix[row, variableCount] = T.Zero - T.One;
		}

		// Augment by joining the coefficients with the constants
		var augmentedMatrix = MatrixUtils.JoinMatrices(condensedCoefficientsMatrix, constantsVector);
		
		var solver = new GaussJordanSolver<T>();
		var solvedMatrix = solver.Solve(augmentedMatrix);

		var result = MatrixUtils.MakeEmptyMatrix<T>(size, size);

		int solvedVariableIndex = 0;
		for (int row = 0; row < size; row++)
		{
			for (int col = 0; col < size; col++)
			{
				var cell = square[row, col];

				if (cell is not null)
				{
					result[row, col] = cell;
				}
				else
				{
					result[row, col] = solvedMatrix[solvedVariableIndex, variableCount + 1];
					solvedVariableIndex++;
				}
			}
		}

		return result;
	}

	static T[,] GenerateCoefficientsMatrixForMagicSquare(int squareSize)
	{
		// This matrix keeps track of the information we have been provided in the magic square
		// in the form of a system of linear equations.
		var coefficientsMatrix = MatrixUtils.MakeEmptyMatrix<T>(2 * squareSize + 2, squareSize * squareSize + 1);

		// Entering information from the square into the matrix: the first n rows are the rows
		// from the square
		for (int magicSquareRowIndex = 0; magicSquareRowIndex < squareSize; magicSquareRowIndex++)
		{
			for (int magicSquareColIndex = 0; magicSquareColIndex < squareSize; magicSquareColIndex++)
			{
				var variablePosition = squareSize * magicSquareRowIndex + magicSquareColIndex;
				var variableQuantity = T.One;
				coefficientsMatrix[magicSquareRowIndex, variablePosition] = variableQuantity;
			}
		}

		// Next n rows are the columns from the square
		for (int colInfo = 0; colInfo < squareSize; colInfo++)
		{
			for (int colIndex = 0; colIndex < squareSize; colIndex++)
			{
				var variablePosition = colInfo + squareSize * colIndex;
				var variableQuantity = T.One;
				coefficientsMatrix[squareSize + colInfo, variablePosition] = variableQuantity;
			}
		}

		// Last 2 rows are the diagonals from the square
		for (int rowAndCol = 0; rowAndCol < squareSize; rowAndCol++)
		{
			coefficientsMatrix[2 * squareSize, (squareSize + 1) * rowAndCol] = T.One;
		}

		for (int rowAndCol = 0; rowAndCol < squareSize; rowAndCol++)
		{
			coefficientsMatrix[2 * squareSize + 1, (squareSize - 1) * (rowAndCol + 1)] = T.One;
		}

		// Column on the end for -1 to balance the equation that a + b + c = sum,
		// the `-1` is for the variable sum

		for (int row = 0; row < 2 * squareSize + 2; row++)
		{
			coefficientsMatrix[row, squareSize * squareSize] = T.Zero - T.One;
		}

		return coefficientsMatrix;
	}
}
