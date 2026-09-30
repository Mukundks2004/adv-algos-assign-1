using GaussJordanElim.Abstractions;

namespace GaussJordanElim.Implementations;

internal class TwoByTwoMatrix<T> : IField<TwoByTwoMatrix<T>> where T : IRing<T>
{
	readonly T a, b, c, d;

	public TwoByTwoMatrix()
	{
		a = T.Zero;
		b = T.Zero;
		c = T.Zero;
		d = T.Zero;
	}

	public TwoByTwoMatrix(T a, T b, T c, T d)
	{
		this.a = a;
		this.b = b;
		this.c = c;
		this.d = d;
	}

	public static TwoByTwoMatrix<T> Zero => throw new NotImplementedException();

	public static TwoByTwoMatrix<T> One => throw new NotImplementedException();

	public TwoByTwoMatrix<T> Clone()
	{
		throw new NotImplementedException();
	}

	public bool Equals(TwoByTwoMatrix<T>? other)
	{
		if (other is null)
		{
			return false;
		}

		return a.Equals(other.a) && b.Equals(other.b) && c.Equals(other.c) && d.Equals(other.d);
	}

	public override bool Equals(object? obj) => obj is TwoByTwoMatrix<T> other && Equals(other);

	public override int GetHashCode() => HashCode.Combine(a, b, c, d);

	public static bool operator ==(TwoByTwoMatrix<T>? a, TwoByTwoMatrix<T>? b) => a?.Equals(b) ?? b is null;

	public static bool operator !=(TwoByTwoMatrix<T>? a, TwoByTwoMatrix<T>? b) => !(a == b);

	public static TwoByTwoMatrix<T> operator +(TwoByTwoMatrix<T> left, TwoByTwoMatrix<T> right)
	{
		return new TwoByTwoMatrix<T>(left.a + right.a, left.b + right.b, left.c + right.c, left.d + right.d);
	}

	public static TwoByTwoMatrix<T> operator -(TwoByTwoMatrix<T> left, TwoByTwoMatrix<T> right)
	{
		return new TwoByTwoMatrix<T>(left.a - right.a, left.b - right.b, left.c - right.c, left.d - right.d);
	}

	public static TwoByTwoMatrix<T> operator *(TwoByTwoMatrix<T> left, TwoByTwoMatrix<T> right)
	{
		throw new NotImplementedException();
	}

	public static TwoByTwoMatrix<T> operator /(TwoByTwoMatrix<T> left, TwoByTwoMatrix<T> right)
	{
		throw new NotImplementedException();
	}
}
