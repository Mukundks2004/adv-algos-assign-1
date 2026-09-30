using System.Runtime.CompilerServices;

namespace GaussJordanElim;

internal class Fraction : IField<Fraction>
{
	public int Numerator { get; }
	public int Denominator { get; }

	public Fraction()
	{
		Numerator = 0;
		Denominator = 0;
	}

	public Fraction(int numerator)
	{
		this.Numerator = numerator;
		Denominator = 1;
	}

	public Fraction(int numerator, int denominator)
	{
		if (denominator == 0)
		{
			throw new DivideByZeroException();
		}

		this.Numerator = numerator;
		this.Denominator = denominator;
	}

	public static Fraction operator +(Fraction left, Fraction right)
	{
		int denominator = left.Denominator * right.Denominator;
		int numerator = (left.Numerator * right.Denominator) + (left.Denominator * right.Numerator);

		return new Fraction(numerator, denominator);
	}

	public static Fraction operator -(Fraction left, Fraction right)
	{
		int denominator = left.Denominator * right.Denominator;
		int numerator = (left.Numerator * right.Denominator) - (left.Denominator * right.Numerator);

		return new Fraction(numerator, denominator);
	}

	public static Fraction operator *(Fraction left, Fraction right)
	{
		int denominator = left.Denominator * right.Denominator;
		int numerator = left.Numerator * right.Numerator;

		return new Fraction(numerator, denominator);
	}

	public static Fraction operator /(Fraction left, Fraction right)
	{
		if (right.Numerator == 0)
		{
			throw new DivideByZeroException();
		}

		int numerator = left.Numerator * right.Denominator;
		int denominator = left.Denominator * right.Numerator;

		return new Fraction(numerator, denominator);
	}
}
