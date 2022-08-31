<Query Kind="Program">
  <NuGetReference>MSTest.TestFramework</NuGetReference>
  <Namespace>Microsoft.VisualStudio.TestTools.UnitTesting</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "OpenAiService"

async Task Main()
{
	var service = new OpenAiService(DefaultOpenAiSettings);
	var summarizer = new OpenAiSummarizer(service);
	
	//var methodBody = GetMethodBody("GetSummary").Dump();
	
	var codeToSummarize = Util.CurrentQuery.Text;
	var codeSummary = await summarizer.GetCodeWithXmldocComments(codeToSummarize);
	codeSummary.Dump();
}

/// <summary>
/// A class that provides methods for summarizing text and code using the OpenAI API.
/// </summary>
public class OpenAiSummarizer
{
	protected IOpenAiService _service { get; set; }

	protected readonly string _defaultTextSummarizationCommandText = "Summarize the following text:";
	protected readonly string _defaultCodeSummarizationCommandText = "Summarize the following code:";
	protected readonly string _defaultCodeEditCommandText = "Add xmldoc comments for each public member in the C# code.";

	public OpenAiSummarizer(IOpenAiService service)
	{
		_service = service;
	}
	/// <summary>
	/// Gets a summary of the specified text.
	/// </summary>
	/// <param name="textToSummarize">The text to summarize.</param>
	/// <returns>A summary of the specified text.</returns>

	public async Task<string> GetSummary(string textToSummarize) => await GetSummary(_defaultTextSummarizationCommandText, textToSummarize);
	/// <summary>
	/// Gets a summary of the specified text.
	/// </summary>
	/// <param name="instructionText">The instruction text to include in the request.</param>
	/// <param name="textToSummarize">The text to summarize.</param>
	/// <returns>A summary of the specified text.</returns>

	public async Task<string> GetSummary(string instructionText, string textToSummarize)
	{
		var completionRequest = new OpenAiCompletionRequest
		{
			Prompt = $"{instructionText}{Environment.NewLine}{textToSummarize}"
		};

		/// <summary>
		/// Gets a summary of the specified code.
		/// </summary>
		/// <param name="codeToSummarize">The code to summarize.</param>
		/// <returns>A summary of the specified code.</returns>
		return await GetCompletionSuggestion(completionRequest);
	}
	/// <summary>
	/// Gets a summary of the specified code.
	/// </summary>
	/// <param name="instructionText">The instruction text to include in the request.</param>
	/// <param name="codeToSummarize">The code to summarize.</param>
	/// <returns>A summary of the specified code.</returns>

	public async Task<string> GetCodeSummary(string codeToSummarize) => await GetCodeSummary(_defaultCodeSummarizationCommandText, codeToSummarize);

	public async Task<string> GetCodeSummary(string instructionText, string codeToSummarize)
	{
		var completionRequest = new OpenAiCompletionRequest
		{
			Prompt = $"{instructionText}{Environment.NewLine}{codeToSummarize}",
			//Engine = Engines.CodexEngine
		};

		return await GetCompletionSuggestion(completionRequest);
	}

	protected string GetSuggestionFromOpenAiResponse(OpenAiResponse response) => response.Choices?.OrderBy(c => c.Index).FirstOrDefault().Text.Trim();

	protected async Task<string> GetCompletionSuggestion(OpenAiCompletionRequest request)
	{
		var completionResponse = await _service.GetCompletion(request);
		var summary = GetSuggestionFromOpenAiResponse(completionResponse);

		return summary;
	}

	// Returns the code provided with the xmldoc comments added.
	public async Task<string> GetCodeWithXmldocComments(string codeToSummarize)
	{
		var editRequest = new OpenAiEditRequest
		{
			Instruction = _defaultCodeEditCommandText,
			Input = codeToSummarize,
			Engine = Engines.CodexEngine,
			Operation = Operations.Edit
		};

		var editResponse = await _service.GetEdit(editRequest);
		var edit = GetSuggestionFromOpenAiResponse(editResponse);
		return edit;
	}
}

// You can define other methods, fields, classes and namespaces here