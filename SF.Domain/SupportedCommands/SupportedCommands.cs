namespace SF.Domain;

public class SupportedCommands : ISupportedCommands
{
	private Dictionary<string, List<string>> _typeSupportedActions = new Dictionary<string, List<string>>();
	
	public void SetUpActionStrategies(Dictionary<string, List<string>> typeSupportedActions)
	{
		_typeSupportedActions = typeSupportedActions;
	}
	
	public List<string> GetSupportedCommands(string type)
	{
		return _typeSupportedActions[type];
	}
}