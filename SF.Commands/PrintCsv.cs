using SF.Core;
using SF.Domain;
using SF.Domain.Actions;
using System;

namespace SF.Commands
{
	[InputAction]
	public class PrintCsvInputAction : InputAction<PrintCsv>
	{
		protected override string Action => "print";
		protected override string HelpString => "print csv/json file";

		private readonly SystemWrapper _systemWrapper;

		public PrintCsvInputAction(SystemWrapper systemWrapper)
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
		private readonly SystemWrapper _systemWrapper;

		public PrintCsv(SystemWrapper systemWrapper, string[] args)
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