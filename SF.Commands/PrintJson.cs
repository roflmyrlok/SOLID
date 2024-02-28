using SF.Core;
using SF.Domain;
using System;
using SF.Commands.Actions;

namespace SF.Commands
{
	[InputAction]
	public class PrintJsonInputAction : InputAction<PrintJson>
	{
		protected override string Action => "print";
		protected override string HelpString => "print csv/json file";
		
		protected override string[] SupportedExtensions => [".json"];

		private readonly ISystemWrapper _systemWrapper;

		public PrintJsonInputAction(ISystemWrapper systemWrapper)
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
		private readonly ISystemWrapper _systemWrapper;

		public PrintJson(ISystemWrapper systemWrapper, string[] args)
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