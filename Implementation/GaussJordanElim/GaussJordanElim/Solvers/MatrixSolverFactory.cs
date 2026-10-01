using GaussJordanElim.MatrixEntities;

namespace GaussJordanElim.Solvers;

internal static class MatrixSolverFactory
{
	public static IMatrixSolver<T> Create<T>(SolveType solveType) where T : IMatrixEntry<T>, new()
	{
		return solveType switch
		{
			SolveType.GaussJordan => new GaussJordanSolver<T>(),
			SolveType.Lu => new LuSolver<T>(),
			_ => throw new ArgumentOutOfRangeException(nameof(solveType), solveType, "Unsupported solve type"),
		};
	}
}
