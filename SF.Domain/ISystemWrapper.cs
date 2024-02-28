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
	public T Execute<T>(string fileName, IFileActionStrategy<T> strategy);
}