using SF.Core;
using SF.Domain;
using SF.Commands.Actions;

namespace SF.Commands
{
	[InputAction]
	public class PrintCsvFileInputAction : InputAction<PrintCsvFile>
	{
		protected override string Action => "print";
		protected override string HelpString => "print csv/json file";
		
		protected override string[] SupportedExtensions => [".csv"];

		private readonly ICurrentUser _currentUser;

		public PrintCsvFileInputAction(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		protected override PrintCsvFile GetCommandInternal(string[] args)
		{
			return new PrintCsvFile(_currentUser, args);
		}
	}

	[Command]
	public class PrintCsvFile : Command
	{
		private readonly IFileActionStrategy<string> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ICurrentUser _currentUser;

		public PrintCsvFile(ICurrentUser currentUser, string[] args)
		{
			_filePath = args[0];
			_currentUser = currentUser;
			_fileActionStrategy = new CsvTableFileActionStrategy();
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