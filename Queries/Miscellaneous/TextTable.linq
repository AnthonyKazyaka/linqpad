<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.EnterpriseServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.RegularExpressions.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Design.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.ApplicationServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.Protocols.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceProcess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Services.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Utilities.v4.0.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Framework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Tasks.v4.0.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Caching.dll</Reference>
  <Namespace>System.Web.UI.WebControls</Namespace>
</Query>

void Main()
{
	var people = new List<Person>
	{
		new Person { Name = "Anthony", Job = "Developer" },
		new Person { Name = "Pooja", Job = "Tester" },
		new Person { Name = "Joel", Job = "Team Leader" },
		new Person { Name = "Rich", Job = "Jokester" }
	};

	var peopleTable = new TextTable<Person>(people);
	peopleTable.HorizontalSeparator = $" | ";
	Util.FixedFont(peopleTable.ToString()).Dump();

	//	var newTable = new PlainTextTable(3, 5, "X");
	//	newTable.Dump();
	//	
	//	string.Join(Environment.NewLine, Enumerable.Repeat(string.Empty, 3)).Dump();
	//	
	//	newTable.HorizontalSeparator = " | ";
	//	newTable.VerticalSeparator = "\n------------\n";
	//	newTable.Dump();
}

private class Person
{
	public string Name { get; set; }
	public string Job { get; set; }
}

// Define other methods and classes here
public class TextTable
{
	public int Width { get; private set; }
	public int Height { get; private set; }
	public List<TextCell> Cells => Rows.SelectMany(x => x.Cells).ToList();
	public List<TextRow> Rows { get; set; }

	private string _horizontalSeparator = TextRow.DefaultSeparator;
	public string HorizontalSeparator
	{
		get
		{
			return _horizontalSeparator;
		}
		set
		{
			_horizontalSeparator = value;
			foreach (var row in Rows)
			{
				row.Separator = HorizontalSeparator;
			}
		}
	}

	public string VerticalSeparator { get; set; } = Environment.NewLine;

	public string InitialCellValue { get; set; }

	public TextTable(int width, int height)
	{
		Width = width;
		Height = height;

		InitializeTable();
	}

	public TextTable(int width, int height, string initialCellValue)
	{
		Width = width;
		Height = height;
		InitialCellValue = initialCellValue;

		InitializeTable();
	}

	protected void InitializeTable()
	{
		Rows = new List<TextRow>(Height);

		for (var i = 0; i < Rows.Count; i++)
		{
			var row = new TextRow(Enumerable.Repeat(InitialCellValue ?? string.Empty, Width).ToArray());
			Rows.Add(row);
		}

		foreach (var row in Rows)
		{
			row.Separator = HorizontalSeparator;
		}
	}

	protected List<int> GetMaxColumnWidths()
	{
		var maxWidths = new List<int>();
		for(var i = 0; i < Width; i++)
		{
			maxWidths.Add(GetMaxWidthForColumn(i));
		}
		
		return maxWidths;
	}

	protected int GetMaxWidthForColumn(int columnIndex)
	{
		if (columnIndex >= Width)
		{
			throw new IndexOutOfRangeException("The column index specified is larger than the number of columns in the table.");
		}

		int maxWidth = 0; //Rows.Max(x=>x[columnIndex].Value.Length);
		for (var i = 0; i < Rows.Count(); i++)
		{
			var cellWidth = Rows[i][columnIndex].Value.Length;
			if (maxWidth < cellWidth)
			{
				maxWidth = cellWidth;
			}
		}

		return maxWidth;
	}

	public override string ToString()
	{
		var columnWidths = GetMaxColumnWidths();
		Rows.ForEach(x=>x.SetColumnWidths(columnWidths));
		return string.Join(VerticalSeparator, Rows.Select(x => x.ToString()));
	}

	private object ToDump()
	{
		return ToString();
	}

	public TextCell this[int row, int column]
	{
		get
		{
			return Rows[row][column];
		}
	}
}

public class TextTable<T> : TextTable
{
	public TextTable(IEnumerable<T> sourceData) : base(sourceData.FirstOrDefault()?.GetType().GetProperties().Count() ?? 0, sourceData.Count())
	{
		var type = typeof(T);
		for (var i = 0; i < Height; i++)
		{
			var propertyValues = type.GetProperties().Select(x => x.GetValue(sourceData.ElementAt(i)).ToString()).ToArray();
			var row = new TextRow(propertyValues);
			Rows.Add(row);
		}
	}
}

public class TextRow
{
	public int Width { get; set; }
	public TextCell[] Cells { get; set; }
	public static string DefaultSeparator { get; set; } = "\t";
	public string Separator { get; set; } = DefaultSeparator;
	public List<int> ColumnWidths { get; private set;}
	
	public TextRow(params string[] values)
	{
		Width = values.Length;

		Cells = new TextCell[Width];
		for (var i = 0; i < Width; i++)
		{
			Cells[i] = new TextCell(values[i]);
		}
	}
	
	public void SetColumnWidths(IEnumerable<int> widths)
	{
		ColumnWidths = widths.ToList();
	}

	public TextRow(string separator, params string[] values) : this(values)
	{
		Separator = separator;
	}

	public override string ToString()
	{
		for(var i = 0; i < Cells.Count(); i++)
		{
			var cellWidth = Cells[i].Value.Length;
			var spacesToPad = ColumnWidths[i] - cellWidth;
			var whitespaceAdjustedCellValue = Cells[i].Value + string.Join(string.Empty, Enumerable.Repeat(" ", spacesToPad));
		}
		return string.Join(Separator, Cells.Select(x => x.Value));
	}

	private object ToDump()
	{
		return ToString();
	}

	public TextCell this[int index]
	{
		get
		{
			return Cells[index];
		}
	}
}

public class TextCell
{
	public string Value { get; set; } = string.Empty;

	public TextCell(string value)
	{
		Value = value;
	}

	public override string ToString()
	{
		return Value;
	}

	private object ToDump()
	{
		return ToString();
	}
}