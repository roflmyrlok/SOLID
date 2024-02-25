using SF.Core;
using SF.Domain;
using SF.Domain.Actions;
using System;

namespace SF.Commands
{
	[InputAction]
	public class PrintJsonInputAction : InputAction<PrintJson>
	{
		protected override string Action => "print";
		protected override string HelpString => "print csv/json file";

		private readonly SystemWrapper _systemWrapper;

		public PrintJsonInputAction(SystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override PrintJson GetCommandInternal(string[] args)
		{
			return new PrintJson(_systemWrapper, args);
		}
	}

	[Command]
	public class PrintJson : Command
	{
		private readonly IFileActionStrategy<string> _fileActionStrategy;
		private readonly string _filePath;
		private readonly SystemWrapper _systemWrapper;

		public PrintJson(SystemWrapper systemWrapper, string[] args)
		{
			_filePath = args[0];
			_systemWrapper = systemWrapper;
			_fileActionStrategy = new JsonTableFileActionStrategy();
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