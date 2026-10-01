using GaussJordanElim.MatrixEntities;
using GaussJordanElim.Solvers;
using SolverApi.Models;

namespace SolverApi.Mapping;

internal static class MatrixMapper
{
	public static Real[,] ToRealMatrix(double?[][] matrix)
	{
		int rowCount = matrix.Length;
		int colCount = rowCount == 0 ? 0 : matrix[0].Length;

		var result = new Real[rowCount, colCount];

		for (int row = 0; row < rowCount; row++)
		{
			if (matrix[row].Length != colCount)
			{
				throw new ArgumentException("Matrix rows must all have the same length");
			}

			for (int col = 0; col < colCount; col++)
			{
				var cell = matrix[row][col];
				result[row, col] = cell.HasValue ? new Real(cell.Value) : null!;
			}
		}

		return result;
	}

	public static double[][] ToJaggedArray(Real[,] matrix)
	{
		int rowCount = matrix.GetLength(0);
		int colCount = matrix.GetLength(1);

		var result = new double[rowCount][];

		for (int row = 0; row < rowCount; row++)
		{
			result[row] = new double[colCount];

			for (int col = 0; col < colCount; col++)
			{
				result[row][col] = matrix[row, col].ToDouble();
			}
		}

		return result;
	}

	public static List<SolveStepDto> ToStepDtos(List<SolverStep<Real>> steps)
	{
		var result = new List<SolveStepDto>(steps.Count);

		foreach (var step in steps)
		{
			result.Add(new SolveStepDto
			{
				State = ToJaggedArray(step.State),
				Description = step.Description,
			});
		}

		return result;
	}
}
