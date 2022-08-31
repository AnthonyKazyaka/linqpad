<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.Mvc</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Mvc</Namespace>
  <Namespace>Microsoft.AspNetCore.Mvc.Routing</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{
	EndpointExtractor.Extract().Dump();
}

public class Endpoint
{
    public string FunctionName { get; set; }
    public string HttpMethod { get; set; }
    public string Route { get; set; }
    public List<string> Parameters { get; set; }
}

public static class EndpointExtractor
{
    public static List<Endpoint> Extract()
    {
        List<Endpoint> endpoints = new List<Endpoint>();
        var controllerTypes = GetControllersFromDll();

        foreach (var controllerType in controllerTypes)
        {
            // Extract the Endpoint objects from the controller
            var extractedEndpoints = ExtractEndpoints(controllerType);
            endpoints.AddRange(extractedEndpoints);
        }

        return endpoints;
    }

    /// <summary>
    /// Gets all the controllers from the DLL
    /// </summary>
    /// <returns></returns>
    private static List<Type> GetControllersFromDll()
    {
        // Path of the DLL
        //string dllPath = @"C:\Git\akazyaka\approval-letter-service\RocketMortgage.Origination.ApprovalLetter\bin\Debug\net6.0\RocketMortgage.Origination.ApprovalLetter.dll";
		string dllPath = @"C:\Git\origination-services\Services\WebHost\RocketMortgage.Origination.BusinessLayerApi\bin\Debug\netcoreapp3.1\RocketMortgage.Origination.BusinessLayerApi.dll";
		
        // Load the assembly
        Assembly assembly = Assembly.LoadFrom(dllPath);

        // Get all the types from the assembly
        var types = assembly.GetTypes();

        // Filter for only the controller types
        var controllerTypes = types.Where(t => typeof(ControllerBase).IsAssignableFrom(t)).ToList();

        return controllerTypes;
    }

    /// <summary>
    /// Extracts the endpoints from the controller
    /// </summary>
    /// <param name="controllerType"></param>
    /// <returns></returns>
    private static List<Endpoint> ExtractEndpoints(Type controllerType)
    {
        List<Endpoint> endpoints = new List<Endpoint>();

        // Get all the methods from the controller
        var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        // Loop through each method and extract the endpoint information
        foreach (var method in methods)
		{
			// Ignore the methods that don't have a [Http*] attribute
			if (!method.CustomAttributes.Any(a => a.AttributeType.Name.StartsWith("Http")))
			{
				continue;
			}

			// Create a new Endpoint object
			Endpoint endpoint = new Endpoint();
			endpoint.FunctionName = method.Name;

			// Get the HTTP method from the attribute
			endpoint.HttpMethod = string.Join(",", method.GetCustomAttribute<HttpMethodAttribute>().HttpMethods);

			// Get the route template from the attribute
			endpoint.Route = method.GetCustomAttribute<RouteAttribute>()?.Template;

			// Get the parameters from the method
			endpoint.Parameters = method.GetParameters().Select(p => p.Name).ToList();

			endpoints.Add(endpoint);
		}

		return endpoints;
	}
}