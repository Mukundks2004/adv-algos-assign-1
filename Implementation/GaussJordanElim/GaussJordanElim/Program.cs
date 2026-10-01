using GaussJordanElim.MatrixEntities;
using GaussJordanElim.Solvers;
using GaussJordanElim.Utils;

namespace GaussJordanElim;

public class Program
{
	public static void Main()
	{
		int[,] rrefSample = {
			{ 1, 3, 3, 8, 5 },
			{ 0, 1, 3, 10, 8 },
			{ 0, 0, 0, -1, -4 },
			{ 0, 0, 0, 2, 8 }
		};

		int?[,] magicSquareSample =
		{
			{ 4, 9, null },
			{ null, null, null },
			{ 8, null, null }
		};

		int?[,] magicSquareSample2 =
		{
			{ 1,     null,     2 },
			{ null,  null,     null },
			{ 3,     null,     null }
		};

		var gaussJordanSolver = new GaussJordanSolver<Fraction>();
		var rrefSampleOverFraction = MatrixUtils.CreateFractionMatrixFromIntArray(rrefSample);

		var rrefResult = gaussJordanSolver.Solve(rrefSampleOverFraction);
		MatrixUtils.PrintMatrix(rrefResult);

		//var magicSquareSolver = new MagicSquareSolver<Fraction>();
		//var magicSquareSampleOverFraction = MatrixUtils.CreateFractionMatrixFromNullableIntArray(magicSquareSample2);

		//var magicSquareResult = magicSquareSolver.Solve(magicSquareSampleOverFraction);
		//MatrixUtils.PrintMatrix(magicSquareResult);
	}
}