using SF.Core;
using SF.Domain;
using System;

namespace SF.Commands
{
	[InputAction]
	public class AddFileInputAction : InputAction<AddFileCommand>
	{
		protected override string Module => "file";
		protected override string Action => "add";
		protected override string HelpString => "add a file";

		private readonly IFileSystem _fileSystem;

		public AddFileInputAction(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
		}

		protected override AddFileCommand GetCommandInternal(string[] args)
		{
			return new AddFileCommand(_fileSystem, args);
		}
	}

	[Command]
	public class AddFileCommand : Command
	{
		private readonly string _filePath;
		private readonly IFileSystem _fileSystem;
		private readonly string _name;

		public AddFileCommand(IFileSystem fileSystem, string[] args)
		{
			_fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

			if (args == null || args.Length < 1)
			{
				throw new ArgumentException("File path must be provided.");
			}

			_filePath = args[0];
			_name = args.Length > 1 ? args[1] : _filePath;
		}

		public override void Execute()
		{
			var tmp = _fileSystem.Add(_filePath, _name);
			if (tmp)
			{
				Console.WriteLine($"FileDescriptor {_filePath} added!");
			}
			else
			{
				Console.WriteLine($"no {_filePath}!");
			}
			
		}
	}
}