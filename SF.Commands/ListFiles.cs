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

		private readonly SystemWrapper _systemWrapper;

		public ListFilesInputAction(SystemWrapper systemWrapper)
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
		private readonly SystemWrapper _systemWrapper;

		public ListFilesCommand(SystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));
		}

		public override void Execute()
		{
			try
			{
				var files = _systemWrapper.GetAllFiles();
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