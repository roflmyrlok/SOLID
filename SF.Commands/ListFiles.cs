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

		private readonly ICurrentUser _systemWrapper;

		public ListFilesInputAction(ICurrentUser systemWrapper)
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
		private readonly ICurrentUser _systemWrapper;

		public ListFilesCommand(ICurrentUser systemWrapper)
		{
			_systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));
		}

		public override void Execute()
		{
			try
			{
				_systemWrapper.IsLogged();
				var tmp = _systemWrapper.GetFileSystem().GetAll();
				var files  = new List<string>();
				foreach (var file in tmp)
				{
					files.Add(file.Name + " at " + file.FilePath);
				}
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