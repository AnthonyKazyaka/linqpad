var jsonSerializationOptions = new JsonSerializerOptions
{
	PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	ReferenceHandler = ReferenceHandler.Preserve
};
System.Text.Json.JsonSerializer.Serialize(LoanThirdParties.First(x=>x.ThirdPartyTypeCode == "Notary"), jsonSerializationOptions).Dump();