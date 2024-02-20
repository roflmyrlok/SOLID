using SF.Core;
using SF.Domain;
using SF.Domain.Actions;
using System;

namespace SF.Commands
{
	[InputAction]
	public class ValidateFileInputAction : InputAction<ValidateFile>
	{
		protected override string Module => "file";
		protected override string Action => "validate";
		protected override string HelpString => "validate csv/json file";

		private readonly IFileSystem _fileSystem;

		public ValidateFileInputAction(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}

		protected override ValidateFile GetCommandInternal(string[] args)
		{
			return new ValidateFile(_fileSystem, args);
		}
	}

	[Command]
	public class ValidateFile : Command
	{
		private readonly IFileActionStrategy<bool> _fileActionStrategy;
		private readonly string _filePath;
		private readonly IFileSystem _fileSystem;

		public ValidateFile(IFileSystem fileSystem, string[] args)
		{
			_filePath = args[0];
			_fileSystem = fileSystem;

			// Decide which strategy to use based on the file type
			if (fileSystem.GetFileExtension(_filePath) == ".csv")
			{
				_fileActionStrategy = new CsvValidationFileActionStrategy();
			}
			else if (fileSystem.GetFileExtension(_filePath) == ".json")
			{
				_fileActionStrategy = new JsonValidationFileActionStrategy();
			}
			else
			{
				throw new Exception("Non-CSV/JSON file validation is not supported.");
			}
		}

		public override void Execute()
		{
			try
			{
				var valid = _fileSystem.Execute(_filePath, _fileActionStrategy);
				if (valid)
				{
					Console.WriteLine("valid file");
				}
				else
				{
					Console.WriteLine("non-valid file");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return;
			}
		}
	}
}