using SF.Core;
using SF.Domain;

namespace SF.Commands;

[InputAction]
public class PrintFileInputAction : InputAction<PrintFile>
{
	protected override string Module => "file";

	protected override string Action => "print";
    
	protected override string HelpString => "print csv/json file";

	private readonly IInteractableFile _interactableFile;

	public PrintFileInputAction(IInteractableFile interactableFile)
	{
		this._interactableFile = interactableFile;
	}

	protected override PrintFile GetCommandInternal(string[] args)
	{
		return new PrintFile(_interactableFile, args);
	}
}

[Command]
public class PrintFile : Command
{
	

	private readonly IInteractableFileCsv _interactable;

	private readonly string filePath;

	public PrintFile(IInteractableFile interactable,string[] args)
	{

		filePath = args[0];
		if (interactable.GetFileExtension(filePath) == ".csv")
		{
			_interactable = (IInteractableFileCsv)interactable;
		}
		else if (false)
		{
			
		}
		else
		{
			throw new Exception("non csv/json file print");
		}
	}

	public override void Execute()
	{
		if (!_interactable.Exist(filePath))
		{
			Console.WriteLine($"no file at {filePath}!");
			return;
		}
		var summary = _interactable.GetCsvTable(filePath);
		foreach (var line in summary)
		{
			Console.WriteLine(line);
		}
		
	}
}