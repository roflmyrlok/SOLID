using SF.Core;
using SF.Domain;
using System;
using SF.Commands.Actions;

namespace SF.Commands
{
	[InputAction]
	public class ValidateJsonFileInputAction : InputAction<ValidateJsonFile>
	{
		protected override string Action => "validate";
		protected override string HelpString => "validate csv/json file";
		
		protected override string[] SupportedExtensions => [".json"];

		private readonly ICurrentUser _currentUser;

		public ValidateJsonFileInputAction(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		protected override ValidateJsonFile GetCommandInternal(string[] args)
		{
			return new ValidateJsonFile(_currentUser, args);
		}
	}

	[Command]
	public class ValidateJsonFile : Command
	{
		private readonly IFileActionStrategy<bool> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ICurrentUser _currentUser;

		public ValidateJsonFile(ICurrentUser currentUser, string[] args)
		{
			_filePath = args[0];
			_currentUser = currentUser;
			_fileActionStrategy = new JsonValidationFileActionStrategy();
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