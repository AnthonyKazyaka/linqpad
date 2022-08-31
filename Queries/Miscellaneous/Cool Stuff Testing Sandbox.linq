<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
	DateTime today = new DateTime(2016, 1, 3);
	
	Calendar cal = dfi.Calendar;
	//cal.GetWeekOfYear(today, dfi.CalendarWeekRule, DayOfWeek.Monday).Dump();
	
	var foo = GetPositiveIntegers().Take(1000).Dump();
}

private class Foo
{
	public string @class = "This is the class variable";
}

IEnumerable<int> GetPositiveIntegers()
{
	int i = 1;
	while (true)
	{
		yield return i++;
	}
}

// Define other methods and classes here