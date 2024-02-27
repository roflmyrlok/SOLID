/*using SF.Core;

namespace SF.Core;

[InputAction(new string[] {"any"})]

public class OptionsAction : InputAction<Options>
{
	protected override string Action => "options";

	protected override string HelpString => "view commands for a file";
    
	private readonly List<IInputAction> possibleCommands;

	public OptionsAction(List<IInputAction> possibleCommands)
	{
		this.possibleCommands = possibleCommands;
	}


	protected override Options GetCommandInternal(string[] args)
	{
		return new Options(possibleCommands, args);
	}

}
[Command]
public class Options : Command
{
	private string _fileName;
	private readonly List<IInputAction> _possibleCommands;

	public Options(List<IInputAction> possibleCommands, string[] args)
	{
		this._possibleCommands = possibleCommands;
		this._fileName = args[0];
	}

	public override void Execute()
	{
		foreach (var possibleCommand in _possibleCommands)
		{
			Console.WriteLine(_fileName);
			Console.WriteLine(possibleCommand.SupportedExtensions);
		}
	}
}*/