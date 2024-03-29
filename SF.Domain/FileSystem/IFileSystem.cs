
namespace SF.Domain
{
	public interface IFileSystem 
	{
		void Add(string filePath, string name);

		void Remove(string name);

		List<FileDescriptor> GetAll();

		string GetFileFullPath(string name);

		long GetFileSizeInBytes(string name);

		long GetFileSizeInBytesFileNotRegistered(string filePath);

		string GetFileExtension(string name);

		bool ExistByName(string name);

		bool ExistByPath(string filePath);

		T Execute<T>(string name, IFileActionStrategy<T> strategy);
	}
}
