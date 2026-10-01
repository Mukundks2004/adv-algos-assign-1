using GaussJordanElim.Implementations;
using GaussJordanElim.Utils;

namespace GaussJordanElim;

public class Program
{
	public static void Main()
	{
		int[,] sampleMatrix = {
			{ 1, 1, 0 },
			{ 1, 2, -3 }
		};

		var solver = new GaussJordanSolver();
		var sampleMatrixOverFraction = MatrixUtils.CreateFractionMatrixFromIntArray(sampleMatrix);

		var result = solver.Solve(sampleMatrixOverFraction);
		MatrixUtils.PrintMatrix(result);
	}
}