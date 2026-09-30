namespace GaussJordanElim;

internal interface IField<T> : IRing<T> where T : IField<T>
{
	static abstract T operator /(T left, T right);
}
