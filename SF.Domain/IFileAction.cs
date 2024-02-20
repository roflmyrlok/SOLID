using SF.Domain.Actions;

namespace SF.Domain
{
	public interface IFileAction<T>
	{
		T Execute(string shortcut, IFileActionStrategy<T> strategy); 
	}
}