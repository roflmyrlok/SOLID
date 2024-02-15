namespace SF.Commands;

using SF.Core;
using SF.Domain;

[InputAction]
public class ListFilesInputAction : InputAction<ListFilesCommand>
{
	protected override string Module => "file";

	protected override string Action => "list";
    
	protected override string HelpString => "list all file";

	private readonly IInteractableFile _interactableFile;

	public ListFilesInputAction(IInteractableFile interactableFile)
	{
		this._interactableFile = interactableFile;
	}

	protected override ListFilesCommand GetCommandInternal(string[] args)
	{
		return new ListFilesCommand(_interactableFile);
	}
}

[Command]
public class ListFilesCommand : Command
{
	

	private readonly IInteractableFile _interactable;
	

	public ListFilesCommand(IInteractableFile interactable)
	{
		this._interactable = interactable;
	}

	public override void Execute()
	{
		var files = _interactable.GetAll();
		foreach (var file in files)
		{
			Console.WriteLine(file.Name, file.FilePath);
		}
	}
}