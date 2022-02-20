<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

#load "CardGameEngine\QuarantaNugetPackageUpdater.linq"

bool hasIncrementedSinceLastApply = false;
string projectFileContent = string.Empty;
string currentVersion = string.Empty;
string originalVersion = string.Empty;
int major;
int minor;
int revision;
string sourceFilePath = @"C:\Users\askan\source\repos\CardGameEngine\CardGameEngine\CardGameEngine.csproj";

void Main()
{
	BuildCardGameEngine();
//	projectFileContent = File.ReadAllText(sourceFilePath);
//	
//	// var packageVersionPattern = @"\<\w*Version\>(?<version>(?<major>\d+)\.(?<minor>\d+)\.(?<revision>\d+))(?<suffix>\.(?<patch>\d+))?\</\w*Version\>";
//	var packageVersionPattern = @"\<\S*Version\>(?<version>(?<major>\d+)\.(?<minor>\d+)\.(?<revision>\d+))\.?\d*\<\S*Version\>";
//	var versionMatch = Regex.Match(projectFileContent, packageVersionPattern);
//	
//	currentVersion = versionMatch.Groups["version"].Value;
//	originalVersion = currentVersion;
//
//	major = int.Parse(versionMatch.Groups["major"].Value);
//	minor = int.Parse(versionMatch.Groups["minor"].Value);
//	revision = int.Parse(versionMatch.Groups["revision"].Value);
//	
//	var newVersionText = () => $"{major}.{minor}.{revision}";
//	var versionProjectText = () => $"CardGameEngine.{newVersionText()}";
//	
//	Label versionLabel;
//	var dumpContainer =
//		new StackPanel(
//			false,
//			versionLabel = new Label(versionProjectText()),
//			new WrapPanel("1em",
//				new Button("Increment Major Version", b => { major++; hasIncrementedSinceLastApply = true; versionLabel.Text = versionProjectText();}),
//				new Button("Increment Minor Version", b => { minor++; hasIncrementedSinceLastApply = true; versionLabel.Text = versionProjectText();}),
//				new Button("Increment Revision", b => { revision++; hasIncrementedSinceLastApply = true; versionLabel.Text = versionProjectText();})
//			),
//			new Label(),
//			new WrapPanel("1em",
//				new Button("Apply to project", b => { UpdateProjectVersion(newVersionText()); hasIncrementedSinceLastApply = false; }),
//				new Button("Reset", b => { ResetVersionNumbers(); versionLabel.Text = versionProjectText();}),
//				new Button("Apply update to Quaranta", b => ApplyChangeToQuaranta())
//			)
//	).Dump();
}

private void UpdateProjectVersion(string version)
{
	if (!hasIncrementedSinceLastApply)
	{
		$"The project is already at version {version}".Dump();
		return;
	}
	
	UpdateProjectFileContents(version);
	
}

private void UpdateProjectFileContents(string version)
{
	projectFileContent = projectFileContent.Replace(currentVersion, version);

	if (currentVersion != version)
	{
		File.WriteAllText(sourceFilePath, projectFileContent);
		$"Successfully updated {sourceFilePath} from CardGameEngine version {currentVersion} to {version} at {DateTime.Now}".Dump();
		currentVersion = version;
	}
	else
	{
		$"CardGameEngine is already version {version} at {DateTime.Now}".Dump();
	}
}

private void ResetVersionNumbers()
{
	currentVersion = originalVersion;

	var versionNumbers = currentVersion.Split(".").Select(x => int.Parse(x)).ToList();
	major = versionNumbers[0];
	minor = versionNumbers[1];
	revision = versionNumbers[2];
}

private void ApplyChangeToQuaranta()
{
	BuildCardGameEngine();
	UpdateQuarantaNugetPackageReference();
}

private void BuildCardGameEngine()
{
	Process proc = new Process();
	proc.StartInfo.FileName = "CMD.exe";
	proc.StartInfo.Arguments = $"/C dotnet pack {sourceFilePath}";
	proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
	proc.Start();
	proc.WaitForExit();
}