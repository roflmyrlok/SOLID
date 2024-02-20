using SF.Core;
using SF.Domain;
using System;
using System.Collections.Generic;

namespace SF.Commands
{
	[InputAction]
	public class ListFilesInputAction : InputAction<ListFilesCommand>
	{
		protected override string Module => "file";
		protected override string Action => "list";
		protected override string HelpString => "list all files";

		private readonly IFileSystem _fileSystem;

		public ListFilesInputAction(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
		}

		protected override ListFilesCommand GetCommandInternal(string[] args)
		{
			return new ListFilesCommand(_fileSystem);
		}
	}

	[Command]
	public class ListFilesCommand : Command
	{
		private readonly IFileSystem _fileSystem;

		public ListFilesCommand(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
		}

		public override void Execute()
		{
			var files = _fileSystem.GetAll();
			foreach (var file in files)
			{
				Console.WriteLine($"Name: {file.Name}, Path: {file.FilePath}");
			}
		}
	}
}