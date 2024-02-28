namespace SF.Domain;

public interface ISystemWrapper
{
	//setup and restore methods
	public void SetUpActionStrategies(Dictionary<string, List<string>> typeSupportedActions);
	public void SetUpEventCollector(IEventCollector eventCollector);
	void Restore(string filePath);
	void Save(string filePath);
	
	// for files related commands
	List<string> GetSupportedCommands(string type);
	void Add(string filePath, string fileName);
	void Remove(string fileName);
	List<string> GetAllFiles();
	long GetFileSizeInBytes(string fileName);
	string GetFileFullPath(string fileName);
	string GetFileExtension(string fileName);
	T Execute<T>(string fileName, IFileActionStrategy<T> strategy);
	 
	// for account related commands to be moved in separate interface?
	bool Login(string accountName, string password);
	bool Login(string accountName);
	bool IsAllowedToChangePlan(string planName);
	void ChangePlan(string planName);
}