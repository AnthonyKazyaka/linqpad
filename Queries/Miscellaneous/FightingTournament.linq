<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

Random mainRand = new Random();

class BattleResult
{
	public Organism Winner;
	public Organism Loser;
}

class Organism
{
	private Random organismRand = new Random();

	public string Name;
	public int Wins = 0;

	public int MaxHealth = 100;
	public int Health = 100;
	public int Speed = 50;
	public int Defense = 10;
	public int Attack = 10;
	public bool IsAlive = true;
	
	public Organism()
	{
		Name = Path.GetRandomFileName();
		Name = Name.Replace(".", string.Empty);
	}
	
	public void LevelUp()
	{
		MaxHealth += 1;
		Speed += 1;
		Defense += 1;
		Attack += 1;
	
//		MaxHealth = (int)(MaxHealth * 1.05f);
//		Speed = (int)(Speed * 1.05f);
//		Defense = (int)(Defense * 1.05f);
//		Attack = (int)(Attack * 1.05f);
		
		//(Name + " has leveled up!").Dump();
	}
	
	public void AttackEnemy(Organism enemy)
	{
		int offset = organismRand.Next((int)(-0.5f * Attack), (int)(0.25f * Attack));
		int totalAttack = Attack + offset;
		//(Name + " attacked " + enemy.Name).Dump();
		enemy.TakeDamage(totalAttack);
	}
	
	public void TakeDamage(int damageAmount)
	{
		int totalDamage = (damageAmount - organismRand.Next((int)(0.25f * Defense), Defense));
		totalDamage = (totalDamage > 0) ? totalDamage : 0;
		Health -= totalDamage;
		
		//(Name + " received " + totalDamage + " in damage - " + Health + " total health remaining\n").Dump();
		
		if(Health <= 0)
		{
			Health = 0;
			IsAlive = false;
		}
	}
}

void Main()
{
	Queue<Organism> fighterQueue = new Queue<Organism>();
	
	List<Organism> fightResults = new List<Organism>();
	
	for(int i = 0; i < 1000; i++)
	{
		Organism newFighter = new Organism();
		ModifyOrganismStats(newFighter);
		fighterQueue.Enqueue(newFighter);
	}
	
	Organism fighter1 = fighterQueue.Dequeue();
	Organism fighter2 = fighterQueue.Dequeue();
	
	Organism winner = new Organism();
	Organism loser = new Organism();
	
	while(fighterQueue.Count > 0)
	{
		BattleResult results = Fight(fighter1, fighter2);
		winner = results.Winner;
		loser = results.Loser;
		
		//winner.LevelUp();
		winner.Wins++;
		
		loser = fighterQueue.Dequeue();
		
		if(!fightResults.Contains(winner))
		{
			fightResults.Add(winner);
		}
		if(!fightResults.Contains(loser))
		{
			fightResults.Add(loser);
		}
		
		fighter1 = winner;
		fighter2 = loser;
	}
	
	BattleResult finalResult = Fight(fighter1, fighter2);
	
	winner = finalResult.Winner;
	loser = finalResult.Loser;
	
	//winner.LevelUp();
	winner.Wins++;
	
	if(!fightResults.Contains(winner))
	{
		fightResults.Add(winner);
	}
	if(!fightResults.Contains(loser))
	{
		fightResults.Add(loser);
	}
		
	fightResults.OrderByDescending(x => x.Wins).Dump();
}

// Returns the winner of the fight
private BattleResult Fight(Organism fighter1, Organism fighter2)
{
	fighter1.Health = fighter1.MaxHealth;
	fighter2.Health = fighter2.MaxHealth;

	List<Organism> turnList = new List<Organism>();
	if(fighter1.Speed > fighter2.Speed)
	{
		turnList.Add(fighter1);
		turnList.Add(fighter2);
	}
	else
	{
		turnList.Add(fighter2);
		turnList.Add(fighter1);
	}
	
	int turnIndex = 0;
	while(fighter1.IsAlive && fighter2.IsAlive)
	{
		turnList[turnIndex].AttackEnemy(turnList[(turnIndex + 1 ) % 2]);
		turnIndex = (turnIndex + 1) % 2;
	}
	
	//fighter1.Dump();
	//fighter2.Dump();
	
	
	BattleResult results = new BattleResult(){ Winner = GetWinner(fighter1, fighter2), Loser = GetLoser(fighter1, fighter2) };
	return results;
}

private Organism GetWinner(Organism fighter1, Organism fighter2)
{
	return (fighter1.IsAlive) ? fighter1 : fighter2;
}

private Organism GetLoser(Organism fighter1, Organism fighter2)
{
	return (fighter1.IsAlive) ? fighter2 : fighter1;
}

private void ModifyOrganismStats(Organism fighter)
{
	List<Action<Organism>> modifiers = new List<Action<Organism>>();
	modifiers.Add(AdjustMaxHealth);
	modifiers.Add(AdjustSpeed);
	modifiers.Add(AdjustDefense);
	modifiers.Add(AdjustAttack);
	
	for(int i = 0; i < 20; i++)
	{
		int modifierIndex = mainRand.Next(0, modifiers.Count);
		modifiers[modifierIndex](fighter);
	}
	
	//fighter.Dump();
}

private void AdjustMaxHealth(Organism fighter)
{
	fighter.MaxHealth += mainRand.Next((int)(-0.1f * fighter.MaxHealth), (int)(0.1f * fighter.MaxHealth));
}

private void AdjustSpeed(Organism fighter)
{
	fighter.Speed += mainRand.Next((int)(-0.3f * fighter.Speed), (int)(0.3f * fighter.Speed));
}

private void AdjustDefense(Organism fighter)
{
	fighter.Defense += mainRand.Next((int)(-0.2f * fighter.Defense), (int)(0.2f * fighter.Defense));
}

private void AdjustAttack(Organism fighter)
{
	fighter.Attack += mainRand.Next((int)(-0.4f * fighter.Attack), (int)(0.4f * fighter.Attack));
}
// Define other methods and classes here