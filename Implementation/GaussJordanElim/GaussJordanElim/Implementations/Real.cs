using GaussJordanElim.Abstractions;

namespace GaussJordanElim.Implementations;

internal class Real : IField<Real>
{
	readonly double backingValue;

	public static Real Zero => new(0);

	public static Real One => new(1);

	public Real()
	{
		backingValue = 0;
	}

	public Real(double value)
	{
		backingValue = value;
	}

	public Real Clone() => new(backingValue);

	public bool Equals(Real? other)
	{
		if (other is null)
		{
			return false;
		}

		return backingValue == other.backingValue;
	}

	public override bool Equals(object? obj) => obj is Real other && Equals(other);

	public override int GetHashCode() => HashCode.Combine(backingValue);

	public static bool operator ==(Real? a, Real? b) => a?.Equals(b) ?? b is null;

	public static bool operator !=(Real? a, Real? b) => !(a == b);

	public static Real operator +(Real left, Real right)
	{
		return new(left.backingValue + right.backingValue);
	}

	public static Real operator -(Real left, Real right)
	{
		return new(left.backingValue - right.backingValue);
	}

	public static Real operator *(Real left, Real right)
	{
		return new(left.backingValue * right.backingValue);
	}

	public static Real operator /(Real left, Real right)
	{
		if (right.backingValue == 0)
		{
			throw new DivideByZeroException();
		}

		return new(left.backingValue / right.backingValue);
	}
}
