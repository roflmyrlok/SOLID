using SF.Core;
using SF.Domain;

namespace SF.Commands;

[InputAction]
public class RemoveFileInputAction : InputAction<RemoveFileCommand>
{
	protected override string Module => "file";

	protected override string Action => "remove";
    
	protected override string HelpString => "remove a file";

	private readonly IFileRegistry fileRegistry;

	public RemoveFileInputAction(IFileRegistry fileRegistry)
	{
		this.fileRegistry = fileRegistry;
	}

	protected override RemoveFileCommand GetCommandInternal(string[] args)
	{
		return new RemoveFileCommand(fileRegistry, args);
	}
}

[Command]
public class RemoveFileCommand : Command
{
	

	private readonly IFileRegistry registry;

	private readonly string filePath;

	public RemoveFileCommand(IFileRegistry registry,string[] args)
	{
		this.registry = registry;
		
		this.filePath = args[0].ToString();
	}

	public override void Execute()
	{
		registry.Remove(filePath);
		Console.WriteLine($"FileDescriptor {filePath} removed!");
	}
}