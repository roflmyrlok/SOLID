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

		private readonly ICurrentUser _currentUser;

		public SummaryFileInputAction(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		protected override SummaryFile GetCommandInternal(string[] args)
		{
			return new SummaryFile(_currentUser, args);
		}
	}

	[Command]
	public class SummaryFile : Command
	{
		private readonly IFileActionStrategy<List<string>> _fileActionStrategy;
		private readonly string _filePath;
		private readonly ICurrentUser _currentUser;
		
		public SummaryFile(ICurrentUser currentUser, string[] args)
		{
			_filePath = args[0];
			_currentUser = currentUser;

			if (currentUser.GetFileSystem().GetFileExtension(_filePath) == ".txt")
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
				var table = _currentUser.GetFileSystem().Execute(_filePath, _fileActionStrategy);
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