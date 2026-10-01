using GaussJordanElim.MatrixEntities;

namespace GaussJordanElim.Solvers;

internal interface IMatrixSolver<T> where T : IMatrixEntry<T>, new()
{
	T[,] Solve(T[,] matrix);
}
