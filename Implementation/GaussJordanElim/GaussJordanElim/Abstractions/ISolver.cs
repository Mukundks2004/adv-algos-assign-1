namespace GaussJordanElim.Abstractions;

internal interface ILinearEquationsSolver
{
	T[] Solve<T>(T?[,] matrix);
}
