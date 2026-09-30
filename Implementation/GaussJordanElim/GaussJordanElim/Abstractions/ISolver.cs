namespace GaussJordanElim.Abstractions;

internal interface ISolver
{
	T[] Solve<T>(T?[,] matrix);
}
