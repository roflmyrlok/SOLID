using SF.Core;
using SF.Domain;
using System;
using SF.Commands.Actions;

namespace SF.Commands
{
	[InputAction]
	public class ValidateCsvFileInputAction : InputAction<ValidateCsvFile>
	{
		protected override string Action => "validate";
		protected override string HelpString => "validate csv/json file";
		
		protected override string[] SupportedExtensions => [".csv"];

		private readonly ICurrentUser _currentUser;

		public ValidateCsvFileInputAction(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		protected override ValidateCsvFile GetCommandInternal(string[] args)
		{
			return new ValidateCsvFile(_currentUser, args);
		}
	}

	[Command]
	public class ValidateCsvFile : Command
	{
		private readonly IFileActionStrategy<bool> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ICurrentUser _currentUser;

		public ValidateCsvFile(ICurrentUser currentUser, string[] args)
		{
			_filePath = args[0];
			_currentUser = currentUser;
			_fileActionStrategy = new CsvValidationFileActionStrategy();
		}

		public override void Execute()
		{
			try
			{
				var valid = _currentUser.GetFileSystem().Execute(_filePath, _fileActionStrategy);
				Console.WriteLine(valid ? "valid file" : "non-valid file");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}