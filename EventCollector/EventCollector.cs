using Newtonsoft.Json;


public class EventCollector : IEventCollector
{
	private string _defaultSaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "events_collected", "events_1");

	public void CollectEvent(string eventName, DateTime timestamp, Dictionary<string, List<string>> param)
	{
		var eventData = new
		{
			event_name = eventName,
			timestamp = timestamp,
			parameters = param
		};
		string jsonEventData = JsonConvert.SerializeObject(eventData, Formatting.Indented);
		WriteJsonToFile(jsonEventData, _defaultSaveFilePath);
	}

	private void WriteJsonToFile(string json, string filePath)
	{
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			File.AppendAllText(filePath, json + Environment.NewLine);
			//Console.WriteLine("Event saved successfully.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error saving event: {ex.Message}");
		}
	}

}