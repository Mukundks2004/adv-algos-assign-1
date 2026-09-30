namespace GaussJordanElim;

internal static class MatrixUtils
{
	public static T[,] MakeEmptyMatrix<T>(int rows, int cols) where T : new()
	{
		var res = new T[rows, cols];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				res[i, j] = new T();
			}
		}

		return res;
	}

	public static void PrintMatrix<T>(T?[,] matrix)
	{
		int rows = matrix.GetLength(0);
		int cols = matrix.GetLength(1);

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				var cell = matrix[i, j];
				Console.Write($"{(cell == null ? "?  " : cell.ToString())} ");
			}

			Console.Write(Environment.NewLine);
		}
	}

	public static T[,] Clone<T>(T[,] original) where T : class, IDeepCloneable<T>, new()
	{
		var result = MakeEmptyMatrix<T>(original.GetLength(0), original.GetLength(1));

		for (int i = 0; i < result.GetLength(0); i++)
		{
			for (int j = 0; j < result.GetLength(1); j++)
			{
				result[i, j] = original[i, j].Clone();
			}
		}

		return result;
	}

	public static T?[,] CloneNullable<T>(T?[,] original) where T : class, IDeepCloneable<T>, new()
	{
		var result = MakeEmptyMatrix<T?>(original.GetLength(0), original.GetLength(1));

		for (int i = 0; i < result.GetLength(0); i++)
		{
			for (int j = 0; j < result.GetLength(1); j++)
			{
				result[i, j] = original[i, j]?.Clone();
			}
		}

		return result;
	}

	public static T[,] AdjoinMatrices<T>(T[,] first, T[,] second) where T : IDeepCloneable<T>, new()
	{
		int firstRows = first.GetLength(0);
		int firstCols = first.GetLength(1);
		int secondRows = second.GetLength(0);
		int secondCols = second.GetLength(1);

		if (firstRows != secondRows)
		{
			throw new ArgumentException($"First matrix has {firstRows} rows, while second has {secondRows} rows, these cannot be adjoined");
		}

		T[,] result = MakeEmptyMatrix<T>(firstRows, firstCols + secondCols);

		for (int i = 0; i < firstRows; i++)
		{
			for (int j = 0; j < firstCols; j++)
			{
				result[i, j] = first[i, j].Clone();
			}

			for (int j = 0; j < secondCols; j++)
			{
				result[i, firstCols + j] = second[i, j].Clone();
			}
		}

		return result;
	}

	public static T Get<T>(T[,] matrix, int row, int col) => matrix[row, col];

	public static int CountNulls<T>(T[,] array) => array.Cast<T>().Count(element => element == null);
}
