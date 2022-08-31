<Query Kind="Program">
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	Draw();
}

// You can define other methods, fields, classes and namespaces here
public void Draw()
{
	var bitmap = new Bitmap(1024, 500);
	
	int additiveFactor = 1;
	
	for (var x = 0; x < bitmap.Width; x++)
	{
		for (var y = 0; y < bitmap.Height; y++)
		{
			var color = Color.Black;
			if(x % 8 == 0 || y % 10 == 0)
			{
				color = Color.White;
			}
			//else if(x % 31 == 0 || y % 31 == 0)
			//{
			//	color = Color.ForestGreen;
			//}
			//else if(x % 2 == 0 || x % 11 == 0)
			//{
			//	color = Color.Yellow;
			//}
			bitmap.SetPixel(x, y, color);
		}
		additiveFactor++;
	}

	bitmap.Dump();
}