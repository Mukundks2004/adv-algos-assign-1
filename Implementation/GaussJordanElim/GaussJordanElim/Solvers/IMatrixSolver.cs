using GaussJordanElim.MatrixEntities;

namespace GaussJordanElim.Solvers;

internal interface IMatrixSolver
{
	T[,] Solve<T>(T[,] matrix) where T : IMatrixEntry<T>, new();
}
