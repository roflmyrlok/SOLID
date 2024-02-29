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

		private readonly ICurrentUser _systemWrapper;

		public PrintJsonFileInputAction(ICurrentUser systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override PrintJsonFile GetCommandInternal(string[] args)
		{
			return new PrintJsonFile(_systemWrapper, args);
		}
	}

	[Command]
	public class PrintJsonFile : Command
	{
		private readonly IFileActionStrategy<string> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ICurrentUser _systemWrapper;

		public PrintJsonFile(ICurrentUser systemWrapper, string[] args)
		{
			_filePath = args[0];
			_systemWrapper = systemWrapper;
			_fileActionStrategy = new JsonTableFileActionStrategy();
		}

		public override void Execute()
		{
			try
			{
				var table = _systemWrapper.GetFileSystem().Execute(_filePath, _fileActionStrategy);
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