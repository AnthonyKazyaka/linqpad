<Query Kind="Program">
  <NuGetReference>Octokit</NuGetReference>
  <Namespace>Octokit</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows</Namespace>
</Query>

async Task Main()
{
	var service = new GitHubService();
	//var allGitHubCommits = await Util.Cache(async () => await service.GetAllCommits());
	//var gitHubCommitMessages = allGitHubCommits.Select(x => x.Commit.Message).ToList();

//	var foo = gitHubCommitMessages.Skip(15).Take(15).ToList();
//	var textLines = GetLinesOfText(foo).Dump();
//
//	var joinedLines = string.Join(Environment.NewLine, textLines);
//	joinedLines.CopyToClipboard();
	//GetPullRequestInfoFromPR(Text).Dump();
	
	var userComments = await service.GetAllCommentForUser("origination", "closing-dates", "akazyaka");
	userComments.Dump();
}

public class PullRequest
{
	public List<string> CommitMessages {get;set;}
	public string Description {get;set;}
	public int Id {get;set;}
}

public void DisplayPullRequest(PullRequest pr)
{
	
}

public List<string> GetLinesOfText(List<string> commitMessages)
{
	if(commitMessages is null)
	{
		return null;
	}
	else if(!commitMessages.Any())
	{
		return new List<string>();
	}
	List<string> linesOfText = new();

	return commitMessages;
}

//public string Get

// You can define other methods, fields, classes and namespaces here

public class GitHubService
{
	GitHubClient _client;
	
	public GitHubService()
	{
		var credentials = new Credentials(Util.GetPassword("github.auth.username"), Util.GetPassword("github.auth.password"));
		_client = new GitHubClient(new ProductHeaderValue("rocket-launcher"), new Uri("https://git.rockfin.com"))
		{
			Credentials = credentials
		};
	}
	
	public async Task<Release> GetLatestRelease(string owner, string repo)
	{
		return await _client.Repository.Release.GetLatest(owner, repo);
	}
	
	public async Task<List<Release>> GetAllReleases(string owner, string repo)
	{
		return (await _client.Repository.Release.GetAll(owner, repo)).ToList();
	}

	public async Task<List<GitHubCommit>> GetAllCommits(string owner, string repo)
	{
		return (await _client.Repository.Commit.GetAll(owner, repo)).ToList();
	}

	// A method that gets a list of all comments by a specified user in the origination/approval-letter-service repository
	public async Task<List<PullRequestReviewComment>> GetAllCommentForUser(string owner, string repo, string user)
	{
		var commentsInRepoPrs = (await _client.Repository.PullRequest.ReviewComment.GetAllForRepository(owner, repo));
		return commentsInRepoPrs.Where(x=>x.User.Login.ToLowerInvariant() == user.ToLowerInvariant()).ToList();
	}
}

public class PullRequestInfo
{
	public string Type { get; set; }
	public string StoryNumber { get; set; }
	public string Message { get; set; }
	public string Id { get; set; }
	public string CommitMessages { get; set; }
}

public PullRequestInfo GetPullRequestInfoFromPR(string pullRequestBody)
{
	var match = Regex.Match(pullRequestBody, @"(?<pullRequest>(?<type>\w+):\s+(\[TFS-(?<storyNumber>\d+)\])?(?<message>.*)\((#(?<prid>\d+))\)?)(((\r)?\n(?<commitMessages>.*))+)");
	if (match.Success)
	{
		//match.Dump();
		return new PullRequestInfo
		{
			Type = match.Groups["type"].Value,
			StoryNumber = match.Groups["storyNumber"].Value,
			Message = match.Groups["message"].Value.Trim(),
			Id = match.Groups["prid"].Value,
			CommitMessages = string.Join(Environment.NewLine, match.Groups["commitMessages"].Captures)
		};
	}

	return null;
}

public string Text => @"Fix: [TFS-0000000] Addressing Linter Concerns from Previous PR (#326)
* Fix: [TFS-0000000] Update using statement ordering based on lint results
* Fix: [TFS-0000000] Adding back import for Guid type
* Fix: [TFS-0000000] Add Guid reference in classes where it was accidentally removed";