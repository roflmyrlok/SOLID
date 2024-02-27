namespace SF.Core;
public interface IInputAction
{
	bool CanHandle(string command);

	ICommand GetCommand(string input);
    
	List<string> SupportedExtensions { get; }
}