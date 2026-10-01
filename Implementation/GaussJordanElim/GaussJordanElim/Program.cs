using GaussJordanElim.MatrixEntities;
using GaussJordanElim.Solvers;
using GaussJordanElim.Utils;

namespace GaussJordanElim;

public class Program
{
	public static void Main()
	{
		int[,] rrefSample2 = {
			{ 1, 3, 3, 8, 5 },
			{ 0, 1, 3, 10, 8 },
			{ 0, 0, 0, -1, -4 },
			{ 0, 0, 0, 2, 8 }
		};

		int[,] rrefSample =
		{
			{ 2, 1, 1, 5 },
			{ 4, -6, 0, -2 },
			{ -2, 7, 2, 9 },
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
		var luSolver = new LuSolver<Fraction>();

		var gaussJordanSampleOverFraction = MatrixUtils.CreateFractionMatrixFromIntArray(rrefSample);
		var luSampleOverFraction = MatrixUtils.CreateFractionMatrixFromIntArray(rrefSample);

		var rrefResult = gaussJordanSolver.Solve(gaussJordanSampleOverFraction);
		MatrixUtils.PrintMatrix(rrefResult);

		var rrefResult2 = luSolver.Solve(luSampleOverFraction);
		MatrixUtils.PrintMatrix(rrefResult2);

		//var magicSquareSolver = new MagicSquareSolver<Fraction>();
		//var magicSquareSampleOverFraction = MatrixUtils.CreateFractionMatrixFromNullableIntArray(magicSquareSample2);

		//var magicSquareResult = magicSquareSolver.Solve(magicSquareSampleOverFraction);
		//MatrixUtils.PrintMatrix(magicSquareResult);
	}
}