<Query Kind="Program">
  <NuGetReference>itext7</NuGetReference>
  <Namespace>iText.Kernel.Pdf</Namespace>
  <Namespace>iText.Kernel.Utils</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	// Prompt the user to input the path to the PDF files to combine.
	string[] filePaths = GetFilePaths();
	
	var outputFileName = $"output_{DateTime.UtcNow:yyyyMMddTHHmmss}.pdf";
	var outputFilePath = $"{filePaths[2]}\\{outputFileName}";
	CombinePdfDocuments(filePaths, outputFilePath);

	// Output a message to the user indicating that the PDF files have been combined and display the timestamp.
	Console.WriteLine($"The PDF files ({string.Join(", ", filePaths.Take(2))}) have been combined.");
	Console.WriteLine("The timestamp is: " + DateTime.Now.ToString());

	//// Ask the user if they want to open the combined PDF file, open the directory in File Explorer, or exit.
	//// Then perform the appropriate action.
	//Console.WriteLine("\nWould you like to (1) open the combined PDF file, (2) open the directory in File Explorer, or (3) exit?");
	//string userInput = Console.ReadLine();
	//switch (userInput)
	//{
	//	case "1":
	//		OpenFile(outputFilePath);
	//		break;
	//	case "2":
	//		OpenDirectory(Path.GetDirectoryName(filePaths[2]));
	//		break;
	//	case "3":
	//		Environment.Exit(0);
	//		break;
	//	default:
	//		Console.WriteLine("Invalid input.");
	//		break;
	//}
}

void OpenFile(string filePath)
{
	Process.Start(filePath);
}

void OpenDirectory(string directoryPath)
{
	Process.Start(directoryPath);
}

string[] GetFilePaths()
{
	// Prompt the user to input the path to the PDF files to combine.
	string[] filePaths = new string[3];

	// Open a dialog box to select the first PDF file.
	filePaths[0] = GetFilePath("Select the first PDF file to combine");
	filePaths[1] = GetFilePath("Select the second PDF file to combine");
	filePaths[2] = GetDirectoryPath("Select the output location for the combined PDF file");
	
	return filePaths;
}

string GetFilePath(string prompt)
{
	// Open a dialog box to select the first PDF file.
	string filePath = "";
	using (OpenFileDialog openFileDialog = new OpenFileDialog())
	{
		openFileDialog.Title = prompt;
		openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
		openFileDialog.FilterIndex = 1;
		openFileDialog.RestoreDirectory = true;

		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			filePath = openFileDialog.FileName;
		}
	}

	return filePath;
}

string GetDirectoryPath(string prompt)
{
	// Open a dialog box to select the output location for the combined PDF file.
	string directoryPath = "";
	using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
	{
		folderBrowserDialog.Description = prompt;
		folderBrowserDialog.ShowNewFolderButton = true;

		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			directoryPath = folderBrowserDialog.SelectedPath;
		}
	}

	return directoryPath;
}

PdfDocument CombinePdfDocuments(string[] filePaths, string outputPath)
{
	PdfDocument combinedDocument = new PdfDocument(new PdfWriter(outputPath));
	PdfDocument file1 = new PdfDocument(new PdfReader(filePaths[0]));
	PdfDocument file2 = new PdfDocument(new PdfReader(filePaths[1]));

	PdfMerger merger = new PdfMerger(combinedDocument);
	merger.Merge(file1, 1, file1.GetNumberOfPages());
	merger.Merge(file2, 1, file2.GetNumberOfPages());
	
	combinedDocument.Close();
	return combinedDocument;
}