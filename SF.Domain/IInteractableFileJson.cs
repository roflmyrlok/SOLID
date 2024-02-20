using Newtonsoft.Json.Linq;

namespace SF.Domain;

using Newtonsoft.Json;
using System;
using System.IO;

public interface IInteractableJson : IInteractableFile
{
	string GetJsonTable(string filePath);
	bool Validate(string filePath);
}

public class InteractableFileJson : InteractableFile, IInteractableJson
{
	public string GetJsonTable(string filePath)
	{
		try
		{
			string json = File.ReadAllText(filePath);
			dynamic parsedJson = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error reading JSON file: {ex.Message}");
			return null;
		}
	}

	public bool Validate(string filePath)
	{
		try
		{
			if (!File.Exists(filePath) || !Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
				return false;
			string json = File.ReadAllText(filePath);
			bool isValidJson = IsValidJson(json);
			return isValidJson;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error validating JSON file: {ex.Message}");
			return false;
		}
	}

	private bool IsValidJson(string json)
	{
		try
		{
			JToken.Parse(json);
			return true;
		}
		catch (JsonReaderException ex)
		{
			// JSON is not valid
			Console.WriteLine($"Invalid JSON format: {ex.Message}");
			return false;
		}
	}
}
