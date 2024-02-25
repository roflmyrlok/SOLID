namespace SF.Core;

public class HelpInputAction : InputAction<HelpCommand>
{
    protected override string Action => "options";

    protected override string HelpString => "options for file";
    
    private readonly List<IInputAction> _possibleCommands;

    public HelpInputAction(List<IInputAction> possibleCommands)
    {
        this._possibleCommands = possibleCommands;
    }


    protected override HelpCommand GetCommandInternal(string[] args)
    {
        return new HelpCommand(_possibleCommands);
    }

}

public class HelpCommand : Command
{
    
    private readonly List<IInputAction> possibleCommands;

    public HelpCommand(List<IInputAction> possibleCommands)
    {
        this.possibleCommands = possibleCommands;
    }

    public override void Execute()
    {
        foreach (var possibleCommand in possibleCommands)
        {
            Console.WriteLine(possibleCommand.Description);
        }
    }
}