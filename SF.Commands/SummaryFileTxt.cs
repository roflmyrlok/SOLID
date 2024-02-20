using SF.Core;
using SF.Domain;
using SF.Domain.Actions;
using System;

namespace SF.Commands
{
	[InputAction]
	public class SummaryFileInputAction : InputAction<SummaryFile>
	{
		protected override string Module => "file";
		protected override string Action => "summary";
		protected override string HelpString => "print summary for txt file";

		private readonly IFileSystem _fileSystem;

		public SummaryFileInputAction(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}

		protected override SummaryFile GetCommandInternal(string[] args)
		{
			return new SummaryFile(_fileSystem, args);
		}
	}

	[Command]
	public class SummaryFile : Command
	{
		private readonly IFileActionStrategy<List<string>> _fileActionStrategy;
		private readonly string _filePath;
		private readonly IFileSystem _fileSystem;

		public SummaryFile(IFileSystem fileSystem, string[] args)
		{
			_filePath = args[0];
			_fileSystem = fileSystem;

			if (fileSystem.GetFileExtension(_filePath) == ".txt")
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
				var table = _fileSystem.Execute(_filePath, _fileActionStrategy);
				foreach (var line in table)
				{
					Console.WriteLine(line);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return;
			}
		}
	}
}