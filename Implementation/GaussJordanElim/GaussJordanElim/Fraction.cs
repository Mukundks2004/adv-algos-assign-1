namespace GaussJordanElim;

internal class Fraction : IField<Fraction>
{
	readonly int numerator;
	readonly int denominator;

	public Fraction()
	{
		numerator = 0;
		denominator = 0;
	}

	public Fraction(int numerator)
	{
		this.numerator = numerator;
		denominator = 1;
	}

	public Fraction(int numerator, int denominator)
	{
		if (denominator == 0)
		{
			throw new DivideByZeroException();
		}

		this.numerator = numerator;
		this.denominator = denominator;
	}

	public static Fraction operator +(Fraction left, Fraction right)
	{
		return new Fraction();
	}

	public static Fraction operator -(Fraction left, Fraction right)
	{
		throw new NotImplementedException();
	}

	public static Fraction operator *(Fraction left, Fraction right)
	{
		throw new NotImplementedException();
	}

	public static Fraction operator /(Fraction left, Fraction right)
	{
		throw new NotImplementedException();
	}
}
