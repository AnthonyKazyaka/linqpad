<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

void Main()
{
	UpdateQuarantaNugetPackageReference();
}

public void UpdateQuarantaNugetPackageReference()
{
	var sourceDirectory = @"C:\Users\askan\source\repos\CardGameEngine\CardGameEngine\bin\Debug";
	var destinationDirectory = @"C:\Users\askan\source\Local NuGet";
	var latestVersionFileName = Directory.GetFiles(sourceDirectory).OrderByDescending(x => File.GetLastWriteTime(x)).First();
	
	var fileNameVersionPattern = @"CardGameEngine.(?<version>(?<major>\d+)\.(?<minor>\d+)\.(?<revision>\d+))\.nupkg";
	var fileNameMatch = Regex.Match(latestVersionFileName, fileNameVersionPattern);
	var latestVersion = fileNameMatch.Groups["version"].Value;
	var versionToReplaceInFileName = $@"{sourceDirectory}\CardGameEngine.{latestVersion}.nupkg";
	
	var sourceFileFullPath = $@"{sourceDirectory}\CardGameEngine.{latestVersion}.nupkg";
	var destinationFileFullPath = $@"{destinationDirectory}\CardGameEngine.{latestVersion}.nupkg";
	if (!File.Exists(destinationFileFullPath))
	{
		File.Copy(sourceFileFullPath, destinationFileFullPath);
		$"Copied the latest package {latestVersion} to the destination at {DateTime.Now}".Dump();
	}
	
	var projectsToUpdate = new[] { @"Quaranta\Quaranta.csproj", @"Tests\Quaranta.Tests\Quaranta.Tests.csproj" };
	foreach (var projectToUpdate in projectsToUpdate)
	{
		var projectFileLocation = $@"C:\Users\askan\source\repos\Quaranta\{projectToUpdate}";
		var projectFileContent = File.ReadAllText(projectFileLocation);
	
		var packageReferencePattern = @"\<PackageReference Include=""CardGameEngine"" Version=""(?<version>(?<major>\d+)\.(?<minor>\d+)\.(?<revision>\d+))"" /\>";
		var currentReferenceMatch = Regex.Match(projectFileContent, packageReferencePattern);
	
		var currentVersion = currentReferenceMatch.Groups["version"].Value;
		if (currentVersion != latestVersion)
		{
	
			var currentVersionPackageReference = currentReferenceMatch.Value;
			var newVersionPackageReference = currentVersionPackageReference.Replace(currentVersion, latestVersion);
	
			var updatedProjectFileContent = projectFileContent.Replace(currentVersionPackageReference, newVersionPackageReference);
			File.WriteAllText(projectFileLocation, updatedProjectFileContent);
			$"Successfully updated {projectToUpdate} to use CardGameEngine.{latestVersion} at {DateTime.Now}".Dump();
		}
		else
		{
			$"{projectToUpdate} is already using the latest version of CardGameEngine (v{latestVersion}) at {DateTime.Now}".Dump();
		}
	}
}

// You can define other methods, fields, classes and namespaces here