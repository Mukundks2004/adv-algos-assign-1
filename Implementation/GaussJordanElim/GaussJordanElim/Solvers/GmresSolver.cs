using GaussJordanElim.MatrixEntities;

namespace GaussJordanElim.Solvers;

internal class GmresSolver : IMatrixSolver
{
	public GmresSolverParams Params { get; }

	public GmresSolver()
	{

	}

	public T[,] Solve<T>(T[,] matrix) where T : IMatrixEntry<T>, new()
	{
		throw new NotImplementedException();
	}
}
