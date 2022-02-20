<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

bool hasIncrementedSinceLastApply = false;
string projectFileContent = string.Empty;
string currentVersion = string.Empty;
string sourceFilePath = @"C:\Users\askan\source\repos\CardGameEngine\CardGameEngine\CardGameEngine.csproj";

void Main()
{
	projectFileContent = File.ReadAllText(sourceFilePath);
	
	// var packageVersionPattern = @"\<\w*Version\>(?<version>(?<major>\d+)\.(?<minor>\d+)\.(?<revision>\d+))(?<suffix>\.(?<patch>\d+))?\</\w*Version\>";
	var packageVersionPattern = @"\<\S*Version\>(?<version>(?<major>\d+)\.(?<minor>\d+)\.(?<revision>\d+))\.?\d*\<\S*Version\>";
	var versionMatch = Regex.Match(projectFileContent, packageVersionPattern);
	
	currentVersion = versionMatch.Groups["version"].Value;

	var major = int.Parse(versionMatch.Groups["major"].Value);
	var minor = int.Parse(versionMatch.Groups["minor"].Value);
	var revision = int.Parse(versionMatch.Groups["revision"].Value);
	
	var newVersionText = () => $"{major}.{minor}.{revision}";
	var versionProjectText = () => $"CardGameEngine.{newVersionText()}";
	
	Label versionLabel;
	var dumpContainer =
		new StackPanel(
			false,
			versionLabel = new Label(versionProjectText()),
			new WrapPanel("1em",
				new Button("Increment Major Version", b => { major++; hasIncrementedSinceLastApply = true; versionLabel.Text = versionProjectText();}),
				new Button("Increment Minor Version", b => { minor++; hasIncrementedSinceLastApply = true; versionLabel.Text = versionProjectText();}),
				new Button("Increment Revision", b => { revision++; hasIncrementedSinceLastApply = true; versionLabel.Text = versionProjectText();})
			),
			new Label(),
			new Button("Apply to project", b => { UpdateProjectVersion(newVersionText()); hasIncrementedSinceLastApply = false; })
	).Dump();
}

private void UpdateProjectVersion(string version)
{
	if (!hasIncrementedSinceLastApply)
	{
		$"The project is already at version {version}".Dump();
		return;
	}
	
	UpdateProjectFileContents(version);
	
	$"Updated the project to version {version} from version {currentVersion}!".Dump();
	currentVersion = version;
}

private void UpdateProjectFileContents(string version)
{
	projectFileContent = projectFileContent.Replace(currentVersion, version);

	if (currentVersion != version)
	{
		File.WriteAllText(sourceFilePath, projectFileContent);
		$"Successfully updated {sourceFilePath} to CardGameEngine.{version} at {DateTime.Now}".Dump();
	}
	else
	{
		$"CardGameEngine is already version {version} at {DateTime.Now}".Dump();
	}
}