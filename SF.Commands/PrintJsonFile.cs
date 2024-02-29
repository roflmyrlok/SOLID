using SF.Core;
using SF.Domain;
using System;
using SF.Commands.Actions;

namespace SF.Commands
{
	[InputAction]
	public class PrintJsonFileInputAction : InputAction<PrintJsonFile>
	{
		protected override string Action => "print";
		protected override string HelpString => "print csv/json file";
		
		protected override string[] SupportedExtensions => [".json"];

		private readonly ICurrentUser _currentUser;

		public PrintJsonFileInputAction(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		protected override PrintJsonFile GetCommandInternal(string[] args)
		{
			return new PrintJsonFile(_currentUser, args);
		}
	}

	[Command]
	public class PrintJsonFile : Command
	{
		private readonly IFileActionStrategy<string> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ICurrentUser _currentUser;

		public PrintJsonFile(ICurrentUser currentUser, string[] args)
		{
			_filePath = args[0];
			_currentUser = currentUser;
			_fileActionStrategy = new JsonTableFileActionStrategy();
		}

		public override void Execute()
		{
			try
			{
				var table = _currentUser.GetFileSystem().Execute(_filePath, _fileActionStrategy);
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