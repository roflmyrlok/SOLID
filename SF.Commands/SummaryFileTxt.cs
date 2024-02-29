using SF.Core;
using SF.Domain;
using System;
using SF.Commands.Actions;

namespace SF.Commands
{
	[InputAction]
	public class SummaryFileInputAction : InputAction<SummaryFile>
	{ 
		protected override string Action => "summary";
		protected override string HelpString => "print summary for txt file";
		
		protected override string[] SupportedExtensions => [".txt"];

		private readonly ICurrentUser _systemWrapper;

		public SummaryFileInputAction(ICurrentUser systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override SummaryFile GetCommandInternal(string[] args)
		{
			return new SummaryFile(_systemWrapper, args);
		}
	}

	[Command]
	public class SummaryFile : Command
	{
		private readonly IFileActionStrategy<List<string>> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ICurrentUser _systemWrapper;
		
		public SummaryFile(ICurrentUser systemWrapper, string[] args)
		{
			_filePath = args[0];
			_systemWrapper = systemWrapper;

			if (systemWrapper.GetFileSystem().GetFileExtension(_filePath) == ".txt")
			{
				_fileActionStrategy = new SummaryFileActionStrategy();
			}
			else
			{
				Console.WriteLine("non txt file summary is not supported.");
			}
		}

		public override void Execute()
		{
			try
			{
				var table = _systemWrapper.GetFileSystem().Execute(_filePath, _fileActionStrategy);
				foreach (var line in table)
				{
					Console.WriteLine(line);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}