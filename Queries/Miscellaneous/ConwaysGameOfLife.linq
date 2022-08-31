<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
</Query>

// LINQPad script for Conway's Game of Life
public void Main()
{
    var grid = new Grid(100, 100);
    grid.Randomize();
    grid.Print();
    
    for (int i = 0; i < 100; i++)
    {
		$"Step {i}".Dump();
        grid.Next();
        grid.Print();
		
		Thread.Sleep(200);
		Util.ClearResults();
    }
}

public class Grid
{
    private int _width;
    private int _height;
    private bool[,] _cells;
    
    public Grid(int width, int height)
    {
        _width = width;
		_height = height;
		_cells = new bool[width, height];
	}

	public void Randomize()
	{
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				_cells[x, y] = RandomBool();
			}
		}
	}

	public void Print()
	{
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				Console.Write(_cells[x, y] ? "*" : " ");
			}
			Console.WriteLine();
		}
	}

	public void Next()
	{
		var next = new bool[_width, _height];
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				int neighbors = CountNeighbors(x, y);
				if (_cells[x, y])
				{
					next[x, y] = neighbors == 2 || neighbors == 3;
				}
				else
				{
					next[x, y] = neighbors == 3;
				}
			}
		}
		_cells = next;
	}

	private int CountNeighbors(int x, int y)
	{
		int count = 0;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				int neighborX = x + i;
				int neighborY = y + j;
				if (i == 0 && j == 0)
				{
					continue;
				}
				if (neighborX < 0 || neighborX >= _width || neighborY < 0 || neighborY >= _height)
				{
					continue;
				}
				if (_cells[neighborX, neighborY])
				{
					count++;
				}
			}
		}
		return count;
	}
	
	private Random _random = new Random();
	
	private bool RandomBool()
	{
		return _random.Next(0, 2) == 0;
	}

	public void Print(int x, int y)
	{
		Console.Write(_cells[x, y] ? "*" : " ");
	}

	public void Print(int x, int y, int width, int height)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Console.Write(_cells[x + i, y + j] ? "*" : " ");
			}
			Console.WriteLine();
		}
	}

	public void Print(int x, int y, int width, int height, int offsetX, int offsetY)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Console.Write(_cells[x + i + offsetX, y + j + offsetY] ? "*" : " ");
			}
			Console.WriteLine();
		}
	}
}
