using SF.Core;
using SF.Domain;
using SF.Commands.Actions;

namespace SF.Commands
{
	[InputAction]
	public class PrintCsvInputAction : InputAction<PrintCsv>
	{
		protected override string Action => "print";
		protected override string HelpString => "print csv/json file";
		
		protected override string[] SupportedExtensions => [".csv"];

		private readonly ISystemWrapper _systemWrapper;

		public PrintCsvInputAction(ISystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override PrintCsv GetCommandInternal(string[] args)
		{
			return new PrintCsv(_systemWrapper, args);
		}
	}

	[Command]
	public class PrintCsv : Command
	{
		private readonly IFileActionStrategy<string> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ISystemWrapper _systemWrapper;

		public PrintCsv(ISystemWrapper systemWrapper, string[] args)
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