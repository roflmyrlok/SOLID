namespace SF.Core;

public abstract class InputAction<TCommand> : IInputAction where TCommand : ICommand
{
    private List<string> _supportedExtensions;
    protected abstract string Action { get; }

    public List<string> SupportedExtensions(){ return SupportedExtensions();}
    
    protected abstract string HelpString { get; }

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

    List<string> IInputAction.SupportedExtensions => _supportedExtensions;

    protected abstract TCommand GetCommandInternal(string[] args);

}