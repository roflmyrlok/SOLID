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

		private readonly ISystemWrapper _systemWrapper;

		public ValidateJsonFileInputAction(ISystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override ValidateJsonFile GetCommandInternal(string[] args)
		{
			return new ValidateJsonFile(_systemWrapper, args);
		}
	}

	[Command]
	public class ValidateJsonFile : Command
	{
		private readonly IFileActionStrategy<bool> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ISystemWrapper _systemWrapper;

		public ValidateJsonFile(ISystemWrapper systemWrapper, string[] args)
		{
			_filePath = args[0];
			_systemWrapper = systemWrapper;
			_fileActionStrategy = new JsonValidationFileActionStrategy();
		}

		public override void Execute()
		{
			try
			{
				var valid = _systemWrapper.Execute(_filePath, _fileActionStrategy);
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