<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "OpenAiService"
#load "OpenAiSummarizer"

async Task Main()
{
	var service = new OpenAiService(DefaultOpenAiSettings);
	var summarizer = new OpenAiSummarizer(service);
	
	RepoDocGenerator generator = new RepoDocGenerator(summarizer);
	var hash = "c504d9f138f1a5ff7a1bc99fb9018c73f2da4fcc";
	(await generator.SummarizeCommit(hash)).Dump("Commit Summary");
	
	//foreach (var file in generator.GetChangedFiles(2))
	//{
	//	var diff = generator.GetFileDiff(file, 2);
	//	(await generator.SummarizeDiff(diff)).Dump();
	//}
}

public interface IRepoDocGenerator
{
	List<string> GetChangedFiles(int commitCount);
	string GetFileDiff(string file, int historyDepth);
	string GetDiff(int historyDepth);
	Task<string> SummarizeDiff(string diff);
}

public class RepoDocGenerator : IRepoDocGenerator
{
	private readonly OpenAiSummarizer _summarizer;

	private const string RepoPath = @"C:\Git\rmbs\";

	public RepoDocGenerator(OpenAiSummarizer summarizer)
	{
        _summarizer = summarizer;
	}
	
	public List<string> GetChangedFiles(int commitCount = 1)
	{
		var changedFileGitOutput = ExecuteGitCommand("diff --name-only HEAD~" + commitCount);
		List<string> changedFiles = changedFileGitOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
		return changedFiles;
	}

	public string GetFileDiff(string file, int historyDepth = 1)
	{
		var gitOutput = ExecuteGitCommand($"diff HEAD HEAD~{historyDepth} -- {file}");
		return gitOutput;
	}

	public string GetDiff(int historyDepth = 1)
	{
		var gitOutput = ExecuteGitCommand($"diff HEAD HEAD~{historyDepth}");
		return gitOutput;
	}

    public async Task<string> SummarizeDiff(string diff)
    {
		return (await _summarizer.GetSummary("Concisely list the code changes in this diff.", diff));
    }
	
	public async Task<string> SummarizeCommit(string commitHash)
	{
		var gitOutput = ExecuteGitCommand($"show {commitHash}").Dump();
		return (await _summarizer.GetSummary("Describe what changes were made and what the affect is.", gitOutput));
	}
	
	public string GetCommitLog(int count = 1)
	{
		var gitOutput = ExecuteGitCommand($"log -n {count} --name-only");
		return gitOutput;
	}
	
	private string GetCommitMessage(string hash)
	{
		var gitOutput = ExecuteGitCommand($"show -s --format=%B {hash}");
		return gitOutput;
	}

	private string ExecuteGitCommand(string command)
	{
		ProcessStartInfo gitProcess = new ProcessStartInfo()
		{
			FileName = "git",
			Arguments = command,
			CreateNoWindow = true,
			UseShellExecute = false,
			RedirectStandardOutput = true,
			WorkingDirectory = RepoPath
		};
		
		Process git = Process.Start(gitProcess);
		return git.StandardOutput.ReadToEnd();
	}
}