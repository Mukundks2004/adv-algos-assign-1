using GaussJordanElim.MatrixEntities;

namespace GaussJordanElim.Solvers;

internal class GmresSolver<T> : IMatrixSolver<T> where T : IMatrixEntry<T>, new()
{
	public GmresSolverParams Params { get; }

	public GmresSolver()
	{

	}

	public T[,] Solve(T[,] matrix)
	{
		throw new NotImplementedException();
	}
}
