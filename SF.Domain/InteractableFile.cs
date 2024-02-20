namespace SF.Domain;

public class InteractableFile : IInteractableFile
{
	private static InteractableFile instance;

	public static InteractableFile Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new InteractableFile();
			}

			return instance;
		}
	}

	private readonly List<FileDescriptor> files = new List<FileDescriptor>();
    
	public void Add(string name, string filePath)
	{
		var fileDescriptor = new FileDescriptor(name, filePath);
        
		files.Add(fileDescriptor);
	}

	public void Remove(string filePath)
	{
		var file = files.FirstOrDefault(e => e.FilePath == filePath);
		if (file != null)
		{
			files.Remove(file);
		}
	}

	public List<FileDescriptor> GetAll()
	{
		return files;
	}

	public string GetFileFullPath(string filePath)
	{
		FileInfo fileInfo = new FileInfo(filePath);
		return fileInfo.FullName;
	}
    
	public long GetFileSizeInBytes(string filePath)
	{
		FileInfo fileInfo = new FileInfo(filePath);
		if (fileInfo.Exists)
		{
			long fileSizeInBytes = fileInfo.Length;
			return   fileSizeInBytes;
		}
		return 0;
	}

	public string GetFileExtension(string filePath)
	{
		return Path.GetExtension(filePath);
	}

	public bool Exist(string filePath)
	{
		if (File.Exists(filePath))
		{
			return true;
		}

		return false;
	}
}