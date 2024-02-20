namespace SF.Domain.Actions
{
	public interface IFileActionStrategy<T>
	{
		T Execute(string filePath);
	}
}