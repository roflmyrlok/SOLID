using SF.Core;
using SF.Domain;
using System;

namespace SF.Commands
{
	[InputAction]
	public class RemoveFileInputAction : InputAction<RemoveFileCommand>
	{
		protected override string Module => "file";
		protected override string Action => "remove";
		protected override string HelpString => "remove a file";

		private readonly IFileSystem _fileSystem;

		public RemoveFileInputAction(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}

		protected override RemoveFileCommand GetCommandInternal(string[] args)
		{
			if (args.Length < 1)
			{
				throw new ArgumentException("File name is required for remove command.");
			}

			return new RemoveFileCommand(_fileSystem, args[0]);
		}
	}

	[Command]
	public class RemoveFileCommand : Command
	{
		private readonly IFileSystem _fileSystem;
		private readonly string _filePath;

		public RemoveFileCommand(IFileSystem fileSystem, string filePath)
		{
			_fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
			_filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
		}

		public override void Execute()
		{
			
			var tmp = _fileSystem.Remove(_filePath);
			if (tmp)
			{
				Console.WriteLine($"FileDescriptor {_filePath} removed!");
			}
			else
			{
				Console.WriteLine($"no {_filePath} !");
			}
		}
	}
}