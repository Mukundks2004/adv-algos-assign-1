namespace GaussJordanElim;

internal interface IMatrixSolver
{
	T[] Solve<T>(T?[,] matrix);
}
