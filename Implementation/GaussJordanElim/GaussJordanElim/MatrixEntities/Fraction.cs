namespace GaussJordanElim.MatrixEntities;

internal class Fraction : IMatrixEntry<Fraction>
{
	int numerator;
	int denominator;

	static readonly Fraction zero = new();
	static readonly Fraction one = new(1);

	public static Fraction Zero => zero;

	public static Fraction One => one;

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

	public Fraction Clone() => new(numerator, denominator);

	public bool Equals(Fraction? other)
	{
		if (other is null)
		{
			return false;
		}

		Simplify();
		other.Simplify();

		return numerator == other.numerator && denominator == other.denominator;
	}

	public override bool Equals(object? obj) => obj is Fraction other && Equals(other);

	public override int GetHashCode() => HashCode.Combine(numerator, denominator);

	public static bool operator ==(Fraction? a, Fraction? b) => a?.Equals(b) ?? b is null;

	public static bool operator !=(Fraction? a, Fraction? b) => !(a == b);

	public bool IsZero() => numerator == 0;

	public bool IsOne() => numerator == denominator;

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
