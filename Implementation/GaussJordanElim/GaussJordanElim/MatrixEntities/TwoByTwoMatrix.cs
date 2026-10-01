using GaussJordanElim.Abstractions;

namespace GaussJordanElim.MatrixEntities;

internal class TwoByTwoMatrix<T> : IField<TwoByTwoMatrix<T>> where T : IMatrixEntry<T>
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

	static readonly TwoByTwoMatrix<T> zero = new(T.Zero, T.Zero, T.Zero, T.Zero);
	static readonly TwoByTwoMatrix<T> one = new(T.One, T.Zero, T.Zero, T.One);

	public static TwoByTwoMatrix<T> Zero => zero;

	public static TwoByTwoMatrix<T> One => one;

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

	public bool IsZero() => this == zero;

	public bool IsOne() => this == one;

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
		return new TwoByTwoMatrix<T>(
			left.a * right.a + left.b * right.c,
			left.a * right.b + left.b * right.d,
			left.c * right.a + left.d * right.c,
			left.c * right.b + left.d * right.d
		);
	}

	public static TwoByTwoMatrix<T> operator /(TwoByTwoMatrix<T> left, TwoByTwoMatrix<T> right)
	{
		var determinantRight = right.a * right.d - right.b * right.c;

		if (determinantRight.IsZero())
		{
			throw new DivideByZeroException();
		}

		var inverseRight = new TwoByTwoMatrix<T>(
			right.d / determinantRight,
			Minus(right.b) / determinantRight,
			Minus(right.c) / determinantRight,
			right.a / determinantRight
		);

		return left * inverseRight;
	}

	public override string ToString()
	{
		return $"{a} {b} {c} {d}";
	}

	static T Minus(T arg)
	{
		return T.Zero - arg;
	}
}
