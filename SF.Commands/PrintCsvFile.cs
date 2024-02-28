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

		private readonly ISystemWrapper _systemWrapper;

		public PrintCsvFileInputAction(ISystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override PrintCsvFile GetCommandInternal(string[] args)
		{
			return new PrintCsvFile(_systemWrapper, args);
		}
	}

	[Command]
	public class PrintCsvFile : Command
	{
		private readonly IFileActionStrategy<string> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ISystemWrapper _systemWrapper;

		public PrintCsvFile(ISystemWrapper systemWrapper, string[] args)
		{
			_filePath = args[0];
			_systemWrapper = systemWrapper;
			_fileActionStrategy = new CsvTableFileActionStrategy();
		}

		public override void Execute()
		{
			try
			{
				var table = _systemWrapper.Execute(_filePath, _fileActionStrategy);
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