using SF.Core;
using SF.Domain;

namespace SF.Commands;

[InputAction]
public class AddFileInputAction : InputAction<AddFileCommand>
{
	protected override string Module => "file";

	protected override string Action => "add";
    
	protected override string HelpString => "add a file";

	private readonly IInteractableFile _interactableFile;

	public AddFileInputAction(IInteractableFile interactableFile)
	{
		this._interactableFile = interactableFile;
	}

	protected override AddFileCommand GetCommandInternal(string[] args)
	{
		return new AddFileCommand(_interactableFile, args);
	}
}

[Command]
public class AddFileCommand : Command
{

	private readonly string filePath;

	private readonly IInteractableFile _interactable;

	private readonly string name;

	public AddFileCommand(IInteractableFile interactable,string[] args)
	{
		this._interactable = interactable;
		
		this.filePath = args[0].ToString();
		this.name = filePath;
		if (args.Length == 2)
		{
			this.name = args[1].ToString();
		}
	}

	public override void Execute()
	{
		_interactable.Add(filePath, name);
		Console.WriteLine($"FileDescriptor {filePath} added!");
	}
}