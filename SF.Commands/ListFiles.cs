using SF.Core;
using SF.Domain;
using System;
using System.Collections.Generic;

namespace SF.Commands
{
	[InputAction]
	public class ListFilesInputAction : InputAction<ListFilesCommand>
	{
		protected override string Action => "list";
		protected override string HelpString => "list all files";
		
		protected override string[] SupportedExtensions => ["any"];

		private readonly ISystemWrapper _systemWrapper;

		public ListFilesInputAction(ISystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));
		}

		protected override ListFilesCommand GetCommandInternal(string[] args)
		{
			return new ListFilesCommand(_systemWrapper);
		}
	}

	[Command]
	public class ListFilesCommand : Command
	{
		private readonly ISystemWrapper _systemWrapper;

		public ListFilesCommand(ISystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));
		}

		public override void Execute()
		{
			try
			{
				var files = _systemWrapper.GetAllFiles();
				if (files.Count == 0)
				{
					Console.WriteLine("You have no files");
					return;
				}
				foreach (var file in files)
				{
					Console.WriteLine(file);
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