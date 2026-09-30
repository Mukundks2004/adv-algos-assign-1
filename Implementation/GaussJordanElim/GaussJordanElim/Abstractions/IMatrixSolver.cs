namespace GaussJordanElim.Abstractions;

internal interface IMatrixSolver
{
	T[] Solve<T>(T[,] matrix) where T : IMatrixEntry<T>;
}
