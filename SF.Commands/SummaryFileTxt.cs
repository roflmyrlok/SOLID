using SF.Core;
using SF.Domain;

namespace SF.Commands;

[InputAction]
public class SummaryFileTxtInputAction : InputAction<SummaryTxtCommand>
{
	protected override string Module => "file";

	protected override string Action => "summary";
    
	protected override string HelpString => "summary of a txt file";

	private readonly IInteractableFile _interactableFile;

	public SummaryFileTxtInputAction(IInteractableFile interactableFile)
	{
		this._interactableFile = interactableFile;
	}

	protected override SummaryTxtCommand GetCommandInternal(string[] args)
	{
		return new SummaryTxtCommand(_interactableFile, args);
	}
}

[Command]
public class SummaryTxtCommand : Command
{
	

	private readonly IInteractableFileTxt _interactable;

	private readonly string filePath;

	public SummaryTxtCommand(IInteractableFile interactable,string[] args)
	{

		filePath = args[0];
		if (interactable.GetFileExtension(filePath) == ".txt")
		{
			_interactable = (IInteractableFileTxt)interactable;
		}
		else
		{
			throw new Exception("non txt file summary");
		}
	}

	public override void Execute()
	{
		var summary = _interactable.GetFileSummary(filePath);
		foreach (var line in summary)
		{
			Console.WriteLine(line);
		}
		
	}
}