namespace SF.Commands;

using SF.Core;
using SF.Domain;

[InputAction]
public class ListFilesInputAction : InputAction<ListFilesCommand>
{
	protected override string Module => "file";

	protected override string Action => "list";
    
	protected override string HelpString => "list all file";

	private readonly IFileRegistry fileRegistry;

	public ListFilesInputAction(IFileRegistry fileRegistry)
	{
		this.fileRegistry = fileRegistry;
	}

	protected override ListFilesCommand GetCommandInternal(string[] args)
	{
		return new ListFilesCommand(fileRegistry);
	}
}

[Command]
public class ListFilesCommand : Command
{
	

	private readonly IFileRegistry registry;
	

	public ListFilesCommand(IFileRegistry registry)
	{
		this.registry = registry;
	}

	public override void Execute()
	{
		var files = registry.GetAll();
		foreach (var file in files)
		{
			Console.WriteLine(file.Name, file.FilePath);
		}
	}
}