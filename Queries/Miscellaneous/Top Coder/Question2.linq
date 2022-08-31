<Query Kind="Program" />

void Main()
{
	//ConvertToLetter(2).Dump();
	const string message = "14522518 7159147 2015 79225 251521 2116 14522518 7159147 2015 12520 251521 4152314";
	
	var unconvertedWords = message.Split(" ");
	unconvertedWords.Dump();

	for (int i = 0; i < unconvertedWords.Length; i++)
	{
		var encryptedWord = unconvertedWords[i];
		for (int j = 0; j < encryptedWord.Length - 1; j++)
		{
			var firstDigit = int.Parse(encryptedWord[j].ToString());
			var secondDigit = int.Parse(encryptedWord[j + 1].ToString());
			var sum = (firstDigit * 10 + secondDigit);

			if (secondDigit == 0 || sum <= 26)
			{
				ConvertToLetter(sum).Dump();
			}
			else if (firstDigit != 0)
			{
				ConvertToLetter(firstDigit).Dump();
			}
//			else if(
//			{
//				if (firstDigit >= 1)
//				{
//					ConvertToLetter(firstDigit).Dump();
//				}
//
//				if (sum < 26 && firstDigit != 0)
//				{
//					ConvertToLetter(sum).Dump();
//				}
//			}
		}
		int lastIndex = encryptedWord.Length - 1;
		var lastNumber = int.Parse(encryptedWord[lastIndex].ToString());
		var lastLetter = ConvertToLetter(lastNumber);
		lastLetter.Dump();
		
		("-------------------").Dump();
	}
}

public string ConvertToLetter(int number)
{
	const string letters = " ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	if (number > 26)
	{ return string.Empty;}
	
	var letter = letters[number];
	return letter.ToString();
}

// Define other methods and classes here
