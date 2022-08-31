<Query Kind="Program" />

void Main()
{
	var groups = GenerateGroups();

	foreach (var group in groups.OrderByDescending(x => x.MovieGoers.Count))
	{
		AssignSeats(group);
	}
	
	DisplayTheater();
	
	groups.OrderBy(x=>x.MovieGoers[0].AssignedSeat).Dump();
}

private void AssignSeats(MovieGoerGroup group)
{
	int seatsToReserve = group.MovieGoers.Count;
	int movieGoerIndex = 0;

	for (var i = 0; i < MovieSeats.Count(); i++)
	{
		if (MovieSeats[i].Count(x => x == 0) < seatsToReserve)
		{
			continue;
		}
		
		for (var j = 0; j < MovieSeats[i].Count(); j++)
		{
			if (MovieSeats[i][j] == 0 && seatsToReserve > 0)
			{
				MovieSeats[i][j] = 1;
				group.MovieGoers[movieGoerIndex].AssignedSeat = (i, j);
				movieGoerIndex++;
				seatsToReserve--;
			}
		}
//		foreach (var row in MovieSeats.Where(x => x.Count(y => y == 0) > group.MovieGoers.Count).OrderByDescending(x => x.Count()))
//		{
//			for (var j = 0; j < row.Count(); j++)
//			{
//				if (row[j] == 0 && seatsToReserve > 0)
//				{
//					row[j] = 1;
//
//					seatsToReserve--;
//				}
//			}
//		}
	}
}

private void DisplayTheater()
{
	for (var i = 0; i < MovieSeats.Count(); i++)
	{
		DisplayRow(i);
	}
}

private void DisplayRow(int rowNumber)
{
	string.Join(" ", MovieSeats[rowNumber].Select(x => x > 0 ? "X" : "0")).Dump();

}

private List<MovieGoerGroup> GenerateGroups(int numberOfGroups = 30)
{
	return Enumerable.Range(0, numberOfGroups).Select(x=>CreateGroup()).ToList();
}

public int[][] MovieSeats { get; set; } = new int[][]
{
	new int[20],
	new int[20],
	new int[20],
	new int[20],
	new int[20],
	new int[20],
	new int[20],
	new int[20],
	new int[20],
	new int[20],
};

public class MovieGoerGroup
{
	public List<MovieGoer> MovieGoers { get; set; } = new List<MovieGoer>();
}

private MovieGoerGroup CreateGroup()
{
	return new MovieGoerGroup
	{
		MovieGoers = GetRandomMovieGoers().Take(random.Next(9) + 1).ToList()
	};
}

private Random random = new Random();
private IEnumerable<MovieGoer> GetRandomMovieGoers()
{
	while (true)
	{
		yield return new MovieGoer
		{
			Name = CreateRandomFullName(),
			Age = GetRandomAge()
		};
	}
}

private string CreateRandomFullName()
{
	return $"{FirstNamePossibilities[random.Next(FirstNamePossibilities.Count)]} {LastNamePossibilities[random.Next(LastNamePossibilities.Count)]}";
}

private int GetRandomAge()
{
	return random.Next(14, 100);
}

private List<string> FirstNamePossibilities = new List<string>()
{
	"Anthony",
	"Jacob",
	"Justin",
	"Jeremy",
	"Alex",
	"Andrew",
	"Joe",
	"Jon",
	"Tom",
	"Doug",
	"Sarah",
	"Erin",
	"Chris",
	"Michael",
};

private List<string> LastNamePossibilities = new List<string>()
{
	"Kazyaka",
	"Engel",
	"Wright",
	"Drager",
	"Zettler",
	"Mueller",
	"Hagerman",
	"Vitale",
	"Smith",
	"Jones",
	"Strange",
	"Patel",
	"Wong",
	"Anderson",
};

public class MovieGoer
{
	public string Name { get; set; }
	public int Age { get; set; }
	public (int Row, int Column) AssignedSeat { get; set;}
}

// Define other methods and classes here