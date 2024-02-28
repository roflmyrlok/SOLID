namespace SF.Domain
{

	public class FileDescriptor
	{
		public string FilePath { get; set; }
		public string Name { get; set; }

		public FileDescriptor(string name, string filePath)
		{
			Name = name;
			FilePath = filePath;
		}
	}
}