namespace SF.Core;

public abstract class InputAction<TCommand> : IInputAction where TCommand : ICommand
{
    protected abstract string Action { get; }
    protected abstract string[] SupportedExtensions { get; }
    
    protected abstract string HelpString {get;}

    public bool CanHandle(string command)
    {
        var split = command.Split(" ");
        
        if (Action != split[0])
        {
            return false;
        }

        return true;
    }

    public ICommand GetCommand(string input)
    {
        return GetCommandInternal(input.Split(" ")[1..]);
    }

    public string[] GetSupportedExtension()
    {
        return SupportedExtensions;
    }
    
    public string GetAction()
    {
        return Action;
    }

    protected abstract TCommand GetCommandInternal(string[] args);

}