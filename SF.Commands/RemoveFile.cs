using SF.Core;
using SF.Domain;

namespace SF.Commands;

[InputAction]
public class RemoveFileInputAction : InputAction<RemoveFileCommand>
{
	protected override string Module => "file";

	protected override string Action => "remove";
    
	protected override string HelpString => "remove a file";

	private readonly IInteractableFile _interactableFile;

	public RemoveFileInputAction(IInteractableFile interactableFile)
	{
		this._interactableFile = interactableFile;
	}

	protected override RemoveFileCommand GetCommandInternal(string[] args)
	{
		return new RemoveFileCommand(_interactableFile, args);
	}
}

[Command]
public class RemoveFileCommand : Command
{
	

	private readonly IInteractableFile _interactable;

	private readonly string filePath;

	public RemoveFileCommand(IInteractableFile interactable,string[] args)
	{
		this._interactable = interactable;
		
		this.filePath = args[0].ToString();
	}

	public override void Execute()
	{
		_interactable.Remove(filePath);
		Console.WriteLine($"FileDescriptor {filePath} removed!");
	}
}