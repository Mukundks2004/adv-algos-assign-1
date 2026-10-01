using GaussJordanElim.MatrixEntities;
using GaussJordanElim.Utils;

namespace GaussJordanElim.Solvers;

internal class MagicSquareSolver<T> : IMatrixSolver<T> where T : class, IMatrixEntry<T>, new()
{
	public T[,] Solve(T?[,] square) => SolveWithSteps(square).Result;

	public (T[,] Result, List<SolverStep<T>> Steps) SolveWithSteps(T?[,] square)
	{
		// Size is also referred to as 'n'
		var size = square.GetLength(0);
		var cols = square.GetLength(1);

		if (cols != size)
		{
			throw new ArgumentException($"Magic square is not a square size, instead has {size} rows and {cols} columns.");
		}

		var steps = new List<SolverStep<T>>();

		var coefficientsMatrix = GenerateCoefficientsMatrixForMagicSquare(size);
		steps.Add(new SolverStep<T>(MatrixUtils.Clone(coefficientsMatrix), $"Generated the coefficients for the system of linear equations describing every row, column and diagonal sum of the {size} x {size} magic square (the final column holds the -1 coefficient for the magic sum)"));

		var variableCount = MatrixUtils.CountNulls(square);

		if (variableCount + 1 > 2 * size + 2)
		{
			throw new ArgumentException($"Not enough cells were given to uniquely determine the magic square: {variableCount} cells are unknown, which is too many to solve for with only {2 * size + 2} row/column/diagonal equations.");
		}

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

		steps.Add(new SolverStep<T>(MatrixUtils.Clone(condensedCoefficientsMatrix), $"Condensed the coefficients by removing the columns for the {size * size - variableCount} known cells and folding their values into the constants (on the right of the augmented matrix below)"));

		// Augment by joining the coefficients with the constants
		var augmentedMatrix = MatrixUtils.JoinMatrices(condensedCoefficientsMatrix, constantsVector);
		steps.Add(new SolverStep<T>(MatrixUtils.Clone(augmentedMatrix), "Augmented the condensed coefficients matrix with the constants column, ready to solve for the unknown cells and the magic sum"));

		var solver = new GaussJordanSolver<T>();
		var (solvedMatrix, gaussJordanSteps) = solver.SolveWithSteps(augmentedMatrix);
		steps.AddRange(gaussJordanSteps);

		ValidateSolvedSystem(solvedMatrix, variableCount);

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

		steps.Add(new SolverStep<T>(MatrixUtils.Clone(result), "Copied the given cells and the solved unknown cells back into the magic square"));

		return (result, steps);
	}

	static void ValidateSolvedSystem(T[,] solvedMatrix, int variableCount)
	{
		int rowCount = solvedMatrix.GetLength(0);
		int constantColumnIndex = variableCount + 1;

		for (int i = 0; i <= variableCount; i++)
		{
			if (!solvedMatrix[i, i].IsOne())
			{
				throw new ArgumentException("Not enough cells were given to uniquely determine the magic square; more than one square satisfies the given cells.");
			}
		}

		for (int row = 0; row < rowCount; row++)
		{
			bool allCoefficientsAreZero = true;
			for (int col = 0; col <= variableCount; col++)
			{
				if (!solvedMatrix[row, col].IsZero())
				{
					allCoefficientsAreZero = false;
					break;
				}
			}

			if (allCoefficientsAreZero && !solvedMatrix[row, constantColumnIndex].IsZero())
			{
				throw new ArgumentException("The given cells are inconsistent; no magic square can satisfy all of the given values and sums.");
			}
		}
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
