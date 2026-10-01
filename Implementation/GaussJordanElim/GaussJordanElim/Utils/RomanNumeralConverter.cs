using System.Text;

namespace GaussJordanElim.Utils;

internal static class RomanNumeralConverter
{
	static readonly (int Value, string Numeral)[] Map =
	[
		(1000, "M"), (900, "CM"), (500, "D"), (400, "CD"), (100, "C"), (90, "XC"), (50, "L"), (40, "XL"), (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I"),
	];

	public static string ToRoman(int number)
	{
		if (number <= 0)
		{
			return number.ToString();
		}

		var result = new StringBuilder();

		foreach (var (value, numeral) in Map)
		{
			while (number >= value)
			{
				result.Append(numeral);
				number -= value;
			}
		}

		return result.ToString();
	}
}
