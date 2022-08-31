<Query Kind="Statements" />

var fullNumbers = File.ReadAllLines(@"C:\Users\vzs1d8\Downloads\LowestNumber.csv");

var numbersWithDigitsRemoved = new List<string>();

for (int i = 0; i < fullNumbers.Count(); i++)
{
	int numbersRemoved = 0;
	var numberString = fullNumbers[i];
	var largestDigits = numberString.ToCharArray().Select(x=>int.Parse(x.ToString())).OrderByDescending(x=>x).Take(9).ToList();
	
	int index = 0;
	while(index < numberString.Length)
	{
		int number = int.Parse(numberString[index].ToString());
		if (numbersRemoved < 9 && largestDigits.Contains(int.Parse(number.ToString())))
		{
			largestDigits.Remove(number);
			numberString = numberString.Remove(numberString.IndexOf(number.ToString()), 1);
			numbersRemoved++;
		}
		else
		{
			index++;
		}
	}
	
	numbersWithDigitsRemoved.Add(numberString);
}

numbersWithDigitsRemoved.Where(x=>int.Parse(x) > 0).OrderBy(x=>int.Parse(x)).Dump();