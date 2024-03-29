namespace SF.Domain;

public interface ISupportedCommands
{
	void SetUpActionStrategies(Dictionary<string, List<string>> typeSupportedActions);
	List<string> GetSupportedCommands(string type);
}