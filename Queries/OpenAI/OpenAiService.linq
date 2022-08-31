<Query Kind="Program">
  <NuGetReference>Microsoft.AspNet.WebApi.Client</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var openAiSettings = DefaultOpenAiSettings;
	var provider = new OpenAiService(openAiSettings);

	var summarizer = new OpenAiSummarizer(provider);
	var queryText = string.Join(string.Empty, Util.CurrentQuery.Text.Take(100));

	(await summarizer.GetCodeSummary(queryText)).Dump();
	//var request =
	//	new OpenAiCompletionRequest
	//	{
	//		Prompt = $"The following countries are in the G7: ",
	//		MaxTokens = _defaultMaxTokens,
	//		Temperature = _defaultTemperature,
	//		FrequencyPenalty = _defaultFrequencyPenalty,
	//		PresencePenalty = _defaultPresencePenalty
	//	};
	
	var request = new OpenAiEditRequest
	{
		Input = @"public interface IOpenAiProvider
{
	Task<string> GetCompletion(OpenAiCompletionRequest request);
	Task<string> GetEdit(OpenAiEditRequest request);
	Task<string> GetClassification(OpenAiClassificationRequest request);
	Task<string> ExecuteRequest(OpenAiRequest request);
}",
		Instruction = "Convert the return types to be Task<OpenAiResponse."
	};
	
	var response = await provider.ExecuteRequest(request);
	var suggestion = response.Choices.OrderBy(c => c.Index).FirstOrDefault();
	
	suggestion.Dump("Suggestion").Text.DumpJson();
}

public class OpenAiCompletionRequest : OpenAiRequest
{	
	public string Prompt { get; set; }
	public string Suffix { get; set; }
	public double Temperature { get; set; } = 0.7;
	[JsonProperty("max_tokens")]
	public int MaxTokens { get; set; } = 256;
	[JsonProperty("top_p")]
	public double TopP { get; set; }
	[JsonProperty("frequency_penalty")]
	public double FrequencyPenalty { get; set; } = 0.0;
	[JsonProperty("presence_penalty")]
	public double PresencePenalty { get; set; } = 0.0;
	public int? N {get;set;} // How many completions to generate for each prompt. Defaults to 1.
	public bool Stream { get; set; }
	[JsonProperty("log_probs")]
	public int? LogProbabilities { get; set; }
	public bool Echo { get; set; }
	public string Stop { get; set; }
	[JsonProperty("best_of")]
	public int? BestOf { get; set; }
	[JsonProperty("logit_bias")]
	public string LogitBias { get; set; }
	public string User { get; set; }

	public override Operations Operation => Operations.Completion;
	public override Engines Engine => Engines.DavinciTextEngine;
}

public class OpenAiRequest 
{
	[JsonIgnore]
	public string Id { get; set; }
	[JsonIgnore]
	public virtual Engines Engine { get; set; }
	[JsonIgnore]
	public virtual Operations Operation {get;set;}
}

public class OpenAiResponse
{
	public string Object { get; set; }
	public int Created { get; set; }
	public List<OpenAiChoice> Choices { get; set; }
}

public class OpenAiChoice
{
	public string Text { get; set; }
	public int Index { get; set; }
}

public class OpenAiEditRequest : OpenAiRequest
{
	public string Input {get;set;}
	public string Instruction {get;set;}
	public double Temperature {get;set;}
	[JsonProperty("top_p")]
	public int TopP {get;set;}

	public override Operations Operation => Operations.Edit;
	public override Engines Engine => Engines.DavinciTextEngineEdit;
}

public class OpenAiClassificationRequest : OpenAiRequest
{
	public Models Model {get;set;}
	public string Query {get;set;}
	public List<string> Examples {get;set;}
	public string File {get;set;}
	public List<string> Labels {get;set;}
	[JsonProperty("search_model")]
	public Models SearchModel => Model;
	public double Temperature {get;set;}
	[JsonProperty("log_probs")]
	public int? LogProbabilities {get;set;}
	[JsonProperty("max_examples")]
	public int MaxExamples {get;set;}
	[JsonProperty("logit_bias")]
	public List<LogitBias> LogitBias {get;set;}
	[JsonProperty("return_prompt")]
	public string ReturnPrompt {get;set;}
	[JsonProperty("return_metadata")]
	public string ReturnMetadata {get;set;}
	public List<string> Expand {get;set;}
	public string User {get;set;}

	public override Operations Operation => Operations.Classification;
	public override Engines Engine => Engines.DavinciTextEngine;
}

public class LogitBias
{
	public string Name {get;set;}
	public double Value {get;set;}
}

public interface IOpenAiService
{
	Task<OpenAiResponse> GetCompletion(OpenAiCompletionRequest request);
	Task<OpenAiResponse> GetEdit(OpenAiEditRequest request);
	Task<OpenAiResponse> GetClassification(OpenAiClassificationRequest request);
	Task<OpenAiResponse> ExecuteRequest(OpenAiRequest request);
}

public enum Engines
{
	[Description("text-davinci-002")]
	DavinciTextEngine,
	[Description("text-davinci-edit-001")]
	DavinciTextEngineEdit,
	[Description("code-davinci-002")]
	CodexEngine,
	[Description("text-curie-001")]
	CurieTextEngine,
	[Description("text-babbage-001")]
	BabbageTextEngine,
	[Description("text-ada-001")]
	AdaTextEngine
}

public enum Operations
{
	[Description("completions")]
	Completion,
	[Description("edits")]
	Edit,
	[Description("search")]
	Search,
	[Description("answers")]
	Answers,
	[Description("classifications")]
	Classification
}

public enum Models
{
	[Description("davinci")]
	Davinci,
	[Description("ada")]
	Ada,
	[Description("babbage")]
	Babbage,
	[Description("curie")]
	Curie
}

public class OpenAiSettings
{
	public string Hostname { get; set; }
	public string ApiVersion { get; set; }
	public string EnginesPath { get; set; }
	public string ApiKey { get; set; }
	public Engines Engine = Engines.DavinciTextEngine;
}
public static OpenAiSettings DefaultOpenAiSettings =>
	new OpenAiSettings
	{
		Hostname = "https://api.openai.com",
		ApiVersion = "v1",
		EnginesPath = "engines",
		ApiKey = Util.GetPassword("openai.auth.apikey"),
		Engine = Engines.DavinciTextEngine
	};

public class OpenAiService : IOpenAiService
{
	protected JsonSerializerSettings _jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore, StringEscapeHandling = StringEscapeHandling.EscapeHtml };
	protected HttpClient _client;
	protected OpenAiSettings _settings;

	public OpenAiService(OpenAiSettings openAiSettings)
	{
		_settings = openAiSettings;
		
		ConfigureHttpClient();
	}

	public async Task<OpenAiResponse> GetCompletion(OpenAiCompletionRequest request) => await ExecuteRequest(request);
	public async Task<OpenAiResponse> GetEdit(OpenAiEditRequest request) => await ExecuteRequest(request);
	public async Task<OpenAiResponse> GetClassification(OpenAiClassificationRequest request) => await ExecuteRequest(request);

	protected HttpRequestMessage GetHttpRequest(OpenAiRequest request)
	{
		var httpRequest = new HttpRequestMessage(HttpMethod.Post, GetRequestRoute(request));
		httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request, _jsonSettings), Encoding.UTF8, "application/json");

		return httpRequest;
	}
	
	public async Task<OpenAiResponse> ExecuteRequest(OpenAiRequest request)
	{
		var httpRequest = GetHttpRequest(request);

		var response = await _client.SendAsync(httpRequest);
		return await response.Content.ReadAsAsync<OpenAiResponse>();
	}

	protected void ConfigureHttpClient()
	{
		 _client = new HttpClient();
		
		_client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Util.GetPassword("openai.auth.apikey")}");
		_client.DefaultRequestHeaders.Add("Accept", "application/json");
		_client.BaseAddress = new Uri(_settings.Hostname);
	}

	protected string GetRequestRoute(OpenAiRequest request)
	{
		var engineName = request.Engine.GetDescription();

		var route = request switch
		{
			OpenAiRequest r when (r is OpenAiCompletionRequest || r is OpenAiEditRequest) =>
				$"/{_settings.ApiVersion}/{_settings.EnginesPath}/{request.Engine.GetDescription()}/{request.Operation.GetDescription()}",
			OpenAiClassificationRequest c =>
				$"/{_settings.ApiVersion}/{request.Operation.GetDescription()}",
			_ =>
				string.Empty
		};
		
		return route;
	}
}

public static class EnumExtensions
{
	public static string GetDescription(this Enum value)
	{
		Type type = value.GetType();
		string name = Enum.GetName(type, value);
		if (name != null)
		{
			FieldInfo field = type.GetField(name);
			if (field != null)
			{
				DescriptionAttribute attr = 
					Attribute.GetCustomAttribute(field, 
						typeof(DescriptionAttribute)) as DescriptionAttribute;
				if (attr != null)
				{
					return attr.Description;
				}
			}
		}
		return null;
	}
}