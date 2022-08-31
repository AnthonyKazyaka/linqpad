<Query Kind="Program" />

void Main()
{
	var sumOfNumbers = 0;
	
	for (int i = 3; i <= 10000000; i++)
	{
		var digits = i.ToString().ToCharArray().Select(x=>int.Parse(x.ToString()));
		var factorializedDigits = digits.Select(x=>Factorial(x));
		var sum = factorializedDigits.Sum();

		if (i == sum)
		{
			sum.Dump();
			sumOfNumbers += i;
		}
	}
	sumOfNumbers.Dump("Sum");
}

int Factorial(int number)
{
	if (number >= 2)
	{
		return number * Factorial(number - 1);
	}
	
	return 1;
}

// Define other methods and classes here
