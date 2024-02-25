namespace SF.Core;

public interface IInputAction
{
    bool CanHandle(string command);

    ICommand GetCommand(string input);
    
    string Description { get; }
}

public abstract class InputAction<TCommand> : IInputAction where TCommand : ICommand
{
    protected abstract string Action { get; }

    public string Description => $"{Action} > {HelpString}";
    
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
        return GetCommandInternal(input.Split(" ")[2..]);
    }

    protected abstract TCommand GetCommandInternal(string[] args);

}

public class InputActionAttribute : Attribute
{
    
}