namespace GaussJordanElim.Abstractions;

internal interface IMatrixEntry<T> : IField<T> where T : IField<T>
{
	bool IsZero();
}
