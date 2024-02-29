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

		private readonly ICurrentUser _currentUser;

		public ListFilesInputAction(ICurrentUser currentUser)
		{
			_currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
		}

		protected override ListFilesCommand GetCommandInternal(string[] args)
		{
			return new ListFilesCommand(_currentUser);
		}
	}

	[Command]
	public class ListFilesCommand : Command
	{
		private readonly ICurrentUser _currentUser;

		public ListFilesCommand(ICurrentUser currentUser)
		{
			_currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
		}

		public override void Execute()
		{
			try
			{
				_currentUser.IsLogged();
				var tmp = _currentUser.GetFileSystem().GetAll();
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