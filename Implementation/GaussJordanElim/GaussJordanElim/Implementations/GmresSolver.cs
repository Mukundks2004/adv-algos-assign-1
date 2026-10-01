using GaussJordanElim.Abstractions;

namespace GaussJordanElim.Implementations;

internal class GmresSolver : IMatrixSolver
{
	public T[,] Solve<T>(T[,] matrix) where T : IMatrixEntry<T>, new()
	{
		throw new NotImplementedException();
	}
}
