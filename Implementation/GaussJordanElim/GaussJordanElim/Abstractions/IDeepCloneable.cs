namespace GaussJordanElim.Abstractions;

internal interface IDeepCloneable<T> where T : IDeepCloneable<T>
{
	T Clone();
}
