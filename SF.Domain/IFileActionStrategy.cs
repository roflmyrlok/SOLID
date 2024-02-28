namespace SF.Domain
{
	public interface IFileActionStrategy<T>
	{
		T Execute(string filePath);
	}
}