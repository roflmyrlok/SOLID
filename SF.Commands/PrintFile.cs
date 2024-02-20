using SF.Core;
using SF.Domain;
using SF.Domain.Actions;
using System;

namespace SF.Commands
{
	[InputAction]
	public class PrintFileInputAction : InputAction<PrintFile>
	{
		protected override string Module => "file";
		protected override string Action => "print";
		protected override string HelpString => "print csv/json file";

		private readonly IFileSystem _fileSystem;

		public PrintFileInputAction(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}

		protected override PrintFile GetCommandInternal(string[] args)
		{
			return new PrintFile(_fileSystem, args);
		}
	}

	[Command]
	public class PrintFile : Command
	{
		private readonly IFileActionStrategy<string> _fileActionStrategy;
		private readonly string _filePath;
		private readonly IFileSystem _fileSystem;

		public PrintFile(IFileSystem fileSystem, string[] args)
		{
			_filePath = args[0];
			_fileSystem = fileSystem;

			// Decide which strategy to use based on the file type
			if (fileSystem.GetFileExtension(_filePath) == ".csv")
			{
				_fileActionStrategy = new CsvTableFileActionStrategy();
			}
			else if (fileSystem.GetFileExtension(_filePath) == ".json")
			{
				_fileActionStrategy = new JsonTableFileActionStrategy();
			}
			else
			{
				Console.WriteLine("Non-CSV/JSON file print is not supported.");
			}
		}

		public override void Execute()
		{
			try
			{
				var table = _fileSystem.Execute(_filePath, _fileActionStrategy);
				Console.WriteLine(table);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}