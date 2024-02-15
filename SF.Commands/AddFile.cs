using SF.Core;
using SF.Domain;

namespace SF.Commands;

[InputAction]
public class AddFileInputAction : InputAction<AddFileCommand>
{
	protected override string Module => "file";

	protected override string Action => "add";
    
	protected override string HelpString => "add a file";

	private readonly IFileRegistry fileRegistry;

	public AddFileInputAction(IFileRegistry fileRegistry)
	{
		this.fileRegistry = fileRegistry;
	}

	protected override AddFileCommand GetCommandInternal(string[] args)
	{
		return new AddFileCommand(fileRegistry, args);
	}
}

[Command]
public class AddFileCommand : Command
{

	private readonly string filePath;

	private readonly IFileRegistry registry;

	private readonly string name;

	public AddFileCommand(IFileRegistry registry,string[] args)
	{
		this.registry = registry;
		
		this.filePath = args[0].ToString();
		this.name = filePath;
		if (args.Length == 2)
		{
			this.name = args[1].ToString();
		}
	}

	public override void Execute()
	{
		registry.Add(filePath, name);
		Console.WriteLine($"FileDescriptor {filePath} added!");
	}
}