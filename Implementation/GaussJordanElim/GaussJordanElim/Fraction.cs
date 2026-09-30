using System.Runtime.CompilerServices;

namespace GaussJordanElim;

internal class Fraction : IField<Fraction>
{
	int numerator;
	int denominator;

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
		int denominator = left.denominator * right.denominator;
		int numerator = (left.numerator * right.denominator) + (left.denominator * right.numerator);

		return new Fraction(numerator, denominator);
	}

	public static Fraction operator -(Fraction left, Fraction right)
	{
		int denominator = left.denominator * right.denominator;
		int numerator = (left.numerator * right.denominator) - (left.denominator * right.numerator);

		return new Fraction(numerator, denominator);
	}

	public static Fraction operator *(Fraction left, Fraction right)
	{
		int denominator = left.denominator * right.denominator;
		int numerator = left.numerator * right.numerator;

		return new Fraction(numerator, denominator);
	}

	public static Fraction operator /(Fraction left, Fraction right)
	{
		if (right.numerator == 0)
		{
			throw new DivideByZeroException();
		}

		int numerator = left.numerator * right.denominator;
		int denominator = left.denominator * right.numerator;

		return new Fraction(numerator, denominator);
	}

	public bool IsOne() => numerator == denominator;

	public bool IsZero() => numerator == 0;

	public override string ToString()
	{
		Simplify();
		return $"{numerator}/{denominator}";
	}

	void Simplify()
	{
		int gcd = Gcd(numerator, denominator);
		numerator /= gcd;
		denominator /= gcd;
	}

	// https://en.wikipedia.org/wiki/Euclidean_algorithm#Implementations
	static int Gcd(int a, int b)
	{
		while (b != 0)
		{
			int temp = b;
			b = a % b;
			a = temp;
		}

		return a;
	}
}
