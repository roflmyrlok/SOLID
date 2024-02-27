namespace SF.Core;
public interface IInputAction
{
	bool CanHandle(string command);

	string GetAction();

	ICommand GetCommand(string input);

	string[] GetSupportedExtension();
}