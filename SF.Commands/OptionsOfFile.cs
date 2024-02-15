using SF.Core;
using SF.Domain;

namespace SF.Commands;

[InputAction]
public class OptionsOfFileInputAction : InputAction<OptionsOfFileCommand>
{
	protected override string Module => "file";

	protected override string Action => "options";
    
	protected override string HelpString => "options of a file";

	private readonly IInteractableFile _fileTxtInteractable;

	public OptionsOfFileInputAction(IInteractableFile fileInteractable)
	{
		this._fileTxtInteractable = fileInteractable;
	}

	protected override OptionsOfFileCommand GetCommandInternal(string[] args)
	{
		return new OptionsOfFileCommand(_fileTxtInteractable, args);
	}
}

[Command]
public class OptionsOfFileCommand : Command
{
	

	private readonly IInteractableFile _interactable;

	private readonly string filePath;

	public OptionsOfFileCommand(IInteractableFile interactable,string[] args)
	{
		this._interactable = interactable;
		
		this.filePath = args[0].ToString();
	}

	public override void Execute()
	{
		var answerList = new List<string>{};
		var extention = _interactable.GetFileExtension(filePath);
		answerList.Add("filepath: " + _interactable.GetFileFullPath(filePath));
		answerList.Add("file Size:" +_interactable.GetFileSizeInBytes(filePath).ToString() + "bytes");
		switch (extention)
		{
			case ".txt":
				answerList.Add("available: summary");
				break;
		}
			
	}
}