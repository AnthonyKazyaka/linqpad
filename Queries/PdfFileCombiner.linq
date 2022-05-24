<Query Kind="Program">
  <NuGetReference>itext7</NuGetReference>
  <Namespace>iText.Kernel.Pdf</Namespace>
  <Namespace>iText.Kernel.Utils</Namespace>
</Query>

void Main()
{
	// Please update these values. They are not real
	var file1Path = @"C:\Temp\Foo.pdf";
	var file2Path = @"C:\Temp\LOL.pdf";
	var outputPath = @"C:\Temp\bar.pdf";
	
	CombinePdfDocument(file1Path, file2Path, outputPath);

	$"{outputPath} was created by combining {file1Path} and {file2Path}".Dump();
	"".Dump();
	$"This action was completed at {DateTimeOffset.Now}".Dump();
}

PdfDocument CombinePdfDocument(string file1Path, string file2Path, string outputPath)
{
	PdfDocument combinedDocument = new PdfDocument(new PdfWriter(outputPath));
	PdfDocument file1 = new PdfDocument(new PdfWriter(file1Path));
	PdfDocument file2 = new PdfDocument(new PdfWriter(file2Path));

	PdfMerger merger = new PdfMerger(combinedDocument);
	merger.Merge(file1, 1, file1.GetNumberOfPages());	
	merger.Merge(file2, 1, file2.GetNumberOfPages());	

	combinedDocument.Close();
	return combinedDocument;
}
