<Query Kind="Program" />

public static Random random = new Random();

void Main()
{
	//	Deck deck = GenerateStandardDeck().OrderBy(x=>x.Suit).ThenBy(x=>x.Rank) as Deck;
	Deck deck = GenerateStandardDeck();
	deck.AddRange(GenerateStandardDeck());	
	
	var grouping = new CardGrouping(new StandardCard(3, Suit.Spades), new StandardCard(4, Suit.Spades), new StandardCard(5, Suit.Spades));
	grouping.IsRun().Dump();

	grouping.Add(new StandardCard(2, Suit.Spades));
	grouping.IsRun().Dump();
	
	grouping.Add(new Joker(

	//deck.GetSets().Dump();
	//deck.Shuffle().Dump();
}

// Define other methods and classes here
public class StandardCard
{
	public Suit Suit {get;set;}
	public Rank Rank {get;set;}
	
	public StandardCard()
	{
		var randomStandardCard = GetRandomStandardCard();
		
		Suit = randomStandardCard.Suit;
		Rank = randomStandardCard.Rank;
	}
	
	public StandardCard(Rank rank, Suit suit)
	{
		Rank = rank;
		Suit = suit;
	}

	public StandardCard(int rank, Suit suit) : this((Rank)rank, suit) { }
	
//	public override string ToString()
//	{
//		if(this is Joker)
//		{
//			return "J";
//		}
//	
//		string rankString = string.Empty;
//		string suitString = Suit.ToString().Substring(0, 1);
//		
//		if(Rank >= Rank.Two && Rank <= Rank.Ten)
//		{
//			rankString = ((int)Rank).ToString();
//		}
//		else if(Rank >= Rank.Jack)
//		{
//			rankString = Rank.ToString().Substring(0, 1);
//		}
//		else if(Rank == Rank.Ace)
//		{
//			rankString = "A";
//		}
//		
//		return rankString + suitString;
//	}

	public override string ToString()
	{
		if(this is Joker)
		{
			return "Joker";
		}
		else
		{
			return Rank.ToString() + " of " + Suit.ToString();
		}
	}
}

public class Joker : StandardCard	 { }

public class CardGrouping : List<StandardCard>
{
	public CardGrouping() {}

	public CardGrouping(params StandardCard[] cards)
	{
		this.AddRange(cards);
	}
	
	public CardGrouping(IEnumerable<StandardCard> cards) : this(cards.ToArray()) { }
	
	public CardGrouping(Suit suit, Rank minRank, Rank? maxRank = null)
	{
		if(maxRank == null)
		{
			maxRank = Rank.King;
		}
		
		for(int index = (int)minRank; index <= (int)maxRank; index++)
		{
			this.Add(new StandardCard((Rank)index, suit));
		}
	}
	
	public CardGrouping(Suit suit, int minRank, int? maxRank = null) : this(suit, (Rank)minRank, (Rank?)maxRank)
	{
		if (maxRank == null)
		{
			maxRank = (int)Rank.King;
		}

		for (int index = minRank; index <= maxRank; index++)
		{
			this.Add(new StandardCard((Rank)index, suit));
		}
	}
	
//	public List<Run> GetRuns()
//	{
//		//this.GroupBy
//		return null;
//	}
//	
//	public List<Set> GetSets()
//	{
//		return GetSets(this);
//	}
//	
//	public static List<Run> GetRuns()
//	{
//		List<Run> runs = new List<Run>();		
//		var standardCards = cardGrouping.OfType<StandardCard>().ToList();
//		
//		var index = 0;
//		while(index < standardCards.Count)
//		{
//			var standardCard = standardCards[index];
//			if(!runs.Any(x=>x.Rank == standardCard.Rank))
//			{
//				var set = new Set(standardCard);
//				sets.Add(set);
//			}
//			else
//			{
//				var eligibleSets = sets.Where(x=>x.StandardCards.Any(y => y.Rank == standardCard.Rank) && !x.StandardCards.Any(y=> y.Suit == standardCard.Suit));
//			
//				if(eligibleSets.Any())
//				{
//					eligibleSets.First().Add(new StandardCard(standardCard.Rank, standardCard.Suit));
//				}
//				else
//				{
//					sets.Add(new Set(standardCard));
//					standardCards.RemoveAt(index);
//					
//					// Skip incrementing the index
//					continue;
//				}
//			}
//			
//			index++;
//		}
//		
//		return sets;
//	}
//	
//	// Should this be broken out as ISetEvaluator and SetEvaluator to be injectable so the game logic can be dynamic instead of a static function?
//	// Might be easier for unit testing if multiple games will be supported.
//	public static List<Set> GetSets(CardGrouping cardGrouping)
//	{		
//		List<Set> sets = new List<Set>();		
//		var standardCards = cardGrouping.OfType<StandardCard>().ToList();
//		
//		var index = 0;
//		while(index < standardCards.Count)
//		{
//			var standardCard = standardCards[index];
//			if(!sets.Any(x=>x.Rank == standardCard.Rank))
//			{
//				var set = new Set(standardCard);
//				sets.Add(set);
//			}
//			else
//			{
//				var eligibleSets = sets.Where(x=>x.StandardCards.Any(y => y.Rank == standardCard.Rank) && !x.StandardCards.Any(y=> y.Suit == standardCard.Suit));
//			
//				if(eligibleSets.Any())
//				{
//					eligibleSets.First().Add(new StandardCard(standardCard.Rank, standardCard.Suit));
//				}
//				else
//				{
//					sets.Add(new Set(standardCard));
//					standardCards.RemoveAt(index);
//					
//					// Skip incrementing the index
//					continue;
//				}
//			}
//			
//			index++;
//		}
//		
//		return sets;
//	}

	public bool IsSet()
	{
		var rank = this.First().Rank;
		return this.All(x=>x.Rank == rank);
	}
	
	public bool IsUniqueSet()
	{
		return IsSet() && this.GroupBy(x=>x.Suit).Count() == this.Count;
	}
	
	public bool IsRun()
	{
		var suit = this.First().Suit;
		if(this.Any(x => x.Suit != suit))
		{
			return false;
		}
		
		var orderedCards = this.OrderBy(x=>x.Rank).ToList();
		var runOrder = new CardGrouping(Enum.GetNames(typeof(Rank)).Select(x=>new StandardCard((Rank)Enum.Parse(typeof(Rank), x), suit)));
		runOrder.Add(new StandardCard(Rank.Ace, suit));
		
		for(var i = 0; i < runOrder.Count; i++)
		{
			var runOrderSubgrouping = new CardGrouping(runOrder.Skip(i).Take(this.Count).ToList());
			
			if(!runOrderSubgrouping.Select(x=>x.Rank).ToList().Except(orderedCards.Select(x=>x.Rank).ToList()).Any())
			{
				return true;
			}
//			for(var j = 0; j < this.Count; j++)
//			{
//				if(orderedCards[j].Rank != runOrderSubgrouping[j].Rank)
//				{
//					isRun = false;
//				}
//				else
//				{
//					
//				}
//			}
		}
		
		return false;
//		
//		for(var i = 0; i < orderedCards.Count; i++)
//		{
//			if(orderedCards[i].Rank == Rank.Ace)
//			{
//				if(orderedCards[i + 1].Rank == Rank.Two || orderedCards.Last().Rank == Rank.King)
//				{
//				
//				}
//				else
//				{
//					return false;
//				}
//			}
//		}
	}
	
	public int GetPointValue()
	{
		var pointValue = this.Where(x => !(x is Joker) && x.Rank >= Rank.Two && x.Rank <= Rank.Ten).Sum(x => (int)x.Rank);
		pointValue += this.Count(x => !(x is Joker) && x.Rank >= Rank.Jack && x.Rank <= Rank.King) * 10;	// Face card value in most games
		
		var aces = this.Where(x => x.Rank == Rank.Ace);
		var numberOfAces = aces.Count();
		pointValue += numberOfAces > 1 ? numberOfAces * 11 : numberOfAces;
		
		var jokers = this.OfType<Joker>();
		pointValue += jokers.Count() * 25;
		
		return pointValue;
	}
}

public class Hand : CardGrouping 
{
	// Temporary - shouldn't necessarily be StandardCards here
	public Hand(IEnumerable<StandardCard> cards) : base(cards) { }
	public Hand(params StandardCard[] cards) : base(cards) { }
}

public interface IDeck
{
	void Deal(List<Player> players);
	void Deal(params Player[] players);
}

public class Deck : CardGrouping, IDeck
{
	//private static Random random = new Random();
	
	public Deck(IEnumerable<StandardCard> cards) : base(cards) { }
	public Deck(params StandardCard[] cards) : base(cards) { }
	
	public void Deal(List<Player> players)
	{
		
	}
	
	public void Deal(params Player[] players)
	{
	
	}
}

public enum Suit
{
	Hearts,
	Spades,
	Diamonds,
	Clubs
}

public enum Rank : int
{
    Ace   = 1,
    Two   = 2,
    Three = 3,
    Four  = 4,
    Five  = 5,
    Six   = 6,
    Seven = 7,
    Eight = 8,
    Nine  = 9,
    Ten   = 10,
    Jack  = 11,
    Queen = 12,
    King  = 13,
}

public static Deck GenerateStandardDeck()
{						
	List<Rank> rank = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
	List<Suit> suit = Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList();
	
	var fullSetOfCards = rank.Join(suit, (Rank x) => true, (Suit y) => true, (Rank x, Suit y) => new StandardCard(x, y)).ToList();
	
	return new Deck(fullSetOfCards);
}

public static Deck GenerateExtendedDeck()
{
	var standardDeck = GenerateStandardDeck();
	var jokers = new[] {new Joker(), new Joker()};
	
	var extendedDeck = standardDeck;
	extendedDeck.AddRange(jokers);
	return extendedDeck;
}

public static StandardCard GetRandomStandardCard()
{
	var rank = GetRandomElement<Rank>(Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList());
	var suit = GetRandomElement<Suit>(Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList());
	
	return new StandardCard(rank, suit);
}

public static T GetRandomElement<T>(IList<T> possibleValues)
{
	return possibleValues[random.Next(possibleValues.Count)];	
}

public static class CardGameExtensions
{
	private static Random random = new Random();
	
	public static T GetRandomElement<T>(this List<T> possibleValues)
	{
		return possibleValues[random.Next(possibleValues.Count)];
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = random.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}

public class Player
{
	public Guid Id {get;set;}
	public string Name {get;set;}
	public Hand Hand {get;set;}
	
	public Player()
	{
		Id = Guid.NewGuid();
		Name = Path.GetRandomFileName();
		Hand = new Hand();
	}
}

public class Set : CardGrouping
{
	public Rank Rank {get; private set;}
	
	public Set(IEnumerable<StandardCard> cards) : base()
	{	
		if(cards == null || !cards.Any())
		{
			throw new ArgumentException("The standard cards passed in were null or empty.");
		}
		var rank = cards.First().Rank;
		if(cards.All(x => x.Rank == rank))
		{
			this.AddRange(cards);
			Rank = rank;
		}
		else
		{
			throw new ArgumentException("The standard cards passed in did not all match in Rank.");
		}
	}
	
	public Set(params StandardCard[] cards) : this(cards.ToList()) { }

}

public class Run : CardGrouping
{
	public Suit Suit {get;set;}
	public Rank MinimumRank {get;set;}
	public Rank MaximumRank {get;set;}
	
	public Run(IEnumerable<StandardCard> cards) : base(cards) { }
	public Run(params StandardCard[] cards) : base(cards) { }
}