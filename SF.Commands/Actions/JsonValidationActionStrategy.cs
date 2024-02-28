using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using SF.Domain.ExternalInterfaces;

namespace SF.Domain.Actions
{
	
	[ActionStrategie]
	public class JsonValidationFileActionStrategy : IFileActionStrategy<bool>
	{
		public bool Execute(string filePath)
		{
			return Validate(filePath);
		}

		private bool Validate(string filePath)
		{
			try
			{
				if (!File.Exists(filePath) || !Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
					return false;
				string json = File.ReadAllText(filePath);
				return IsValidJson(json);
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
}