namespace SF.Domain;

public interface ISystemWrapper
{
	void SetUp(Dictionary<string, List<string>>  actionSupportedFileTypes);
	List<string> GetSupportedCommands(string type);
	void Add(string filePath, string fileName);
	void Remove(string fileName);
	List<string> GetAllFiles();
	long GetFileSizeInBytes(string fileName);
	string GetFileFullPath(string fileName);
	string GetFileExtension(string fileName);
	 T Execute<T>(string fileName, IFileActionStrategy<T> strategy);
	 bool Login(string accountName, string password);
	 bool Login(string accountName);
}