using System.ComponentModel;

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
}
