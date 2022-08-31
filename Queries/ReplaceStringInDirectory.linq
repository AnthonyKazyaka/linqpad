<Query Kind="Expression" />

void Main()
{
	string path = @"";

	ProcessFiles(path);
}

void ProcessFiles(string path)
{
	string textToReplace = @"";
	string newText = @"";

	Stack<string> stack;
	List<string> files;
	string[] directories;
	string dir;

	stack = new Stack<string>();
	stack.Push(path);


	while (stack.Count > 0)
	{
		// Pop a directory
		dir = stack.Pop();

		files = Directory.GetFiles(dir).ToList();

		foreach (string file in files)
		{
			string text = File.ReadAllText(file);

			if (text.Contains(textToReplace))
			{
				Console.WriteLine(file);

				text = text.Replace(textToReplace, newText);

				File.WriteAllText(file, text);
			}
		}

		directories = Directory.GetDirectories(dir);
		foreach (string directory in directories)
		{
			// Push each directory into stack
			stack.Push(directory);
		}
	}
}

// Define other methods and classes here