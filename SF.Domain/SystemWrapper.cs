using SF.Domain.Actions;

namespace SF.Domain;

public class SystemWrapper : ISystemWrapper
{
	private FileSystem _fileSystem = new FileSystem();
	private Dictionary<string,List<string>> _typeSupportedActions = new Dictionary<string, List<string>> ();

	public void  SetUp(Dictionary<string, List<string>>  typeSupportedActions)
	{
		_typeSupportedActions = typeSupportedActions;
	}

	public List<string> GetSupportedCommands(string type)
	{
		return _typeSupportedActions[type];
	}
	public void Add(string filePath, string fileName)
	{
		if (!_fileSystem.ExistByPath(filePath))
		{
			throw new Exception("file is missing");
			return;
		}
		_fileSystem.Add(filePath, fileName);
	}

	public void Remove(string fileName)
	{
		if (!_fileSystem.ExistByName(fileName))
		{
			throw new Exception($"no file with name {fileName}");
			return;
		}
		_fileSystem.Remove(fileName);
	}

	public List<string> GetAllFiles()
	{
		
		var tmp = _fileSystem.GetAll();
		if (tmp.Count == 0)
		{
			throw new Exception($"no files");
		}

		var ans = new List<string>();
		foreach (var file in tmp)
		{
			ans.Add(file.Name + "\n");
		}
		return ans;
	}

	public long GetFileSizeInBytes(string fileName)
	{
		if (!_fileSystem.ExistByName(fileName))
		{
			throw new Exception($"no file with name {fileName}");
		}
		return _fileSystem.GetFileSizeInBytes(fileName);
	}

	public string GetFileFullPath(string fileName)
	{
		if (!_fileSystem.ExistByName(fileName))
		{
			throw new Exception($"no file with name {fileName}");
		}
		return _fileSystem.GetFileFullPath(fileName);
	}

	public string GetFileExtension(string fileName)
	{
		if (!_fileSystem.ExistByName(fileName))
		{
			throw new Exception($"no file with name {fileName}");
		}
		return _fileSystem.GetFileExtension(fileName);
	}

	public T Execute<T>(string fileName, IFileActionStrategy<T> strategy)
	{
		if (!_fileSystem.ExistByName(fileName))
		{
			throw new Exception($"no file with name {fileName}");
		}
		return _fileSystem.Execute(fileName, strategy);
	}
}