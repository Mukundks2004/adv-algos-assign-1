namespace GaussJordanElim;

internal static class MatrixUtils
{
	public static T[,] MakeEmptyArray<T>(int rows, int cols) where T : new()
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

	public static T[] MakeEmptyVector<T>(int length) where T : new()
	{
		var res = new T[length];

		for (int i = 0; i < length; i++)
		{
			res[i] = new T();
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
		var result = MakeEmptyArray<T>(original.GetLength(0), original.GetLength(1));

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
		var result = MakeEmptyArray<T?>(original.GetLength(0), original.GetLength(1));

		for (int i = 0; i < result.GetLength(0); i++)
		{
			for (int j = 0; j < result.GetLength(1); j++)
			{
				result[i, j] = original[i, j]?.Clone();
			}
		}

		return result;
	}

	public static void PrintVector<T>(T[] vector) where T : class
	{
		foreach (var v in vector)
		{
			Console.Write(v.ToString() + " ");
		}

		Console.WriteLine();
	}

	//public static T[,] AdjoinVectorToMatrix(Task[,])

	public static T Get<T>(T[,] matrix, int row, int col) => matrix[row, col];

	public static int CountNulls<T>(T[,] array) => array.Cast<T>().Count(element => element == null);
}
