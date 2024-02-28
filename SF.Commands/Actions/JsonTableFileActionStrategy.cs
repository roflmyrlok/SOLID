using Newtonsoft.Json;
using System;
using System.IO;
using SF.Domain.ExternalInterfaces;

namespace SF.Domain.Actions
{
	[ActionStrategie]
	public class JsonTableFileActionStrategy : IFileActionStrategy<string>
	{
		public string Execute(string filePath)
		{
			return GetJsonTable(filePath);
		}

		private string GetJsonTable(string filePath)
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
	}
}