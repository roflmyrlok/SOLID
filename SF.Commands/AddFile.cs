using SF.Core;
using SF.Domain;

namespace SF.Commands
{
	[InputAction]
	public class AddFileInputAction : InputAction<AddFileCommand>
	{
		protected override string Action => "add";
		protected override string HelpString => "add a file";
		protected override string[] SupportedExtensions => ["any"];

		private readonly ICurrentUser _currentUser;

		public AddFileInputAction(ICurrentUser currentUser)
		{
			this._currentUser = currentUser;
		}

		protected override AddFileCommand GetCommandInternal(string[] args)
		{
			return new AddFileCommand(_currentUser, args);
		}
	}

	[Command]
	public class AddFileCommand : Command
	{
		private readonly string _filePath;
		private readonly ICurrentUser _currentUser;
		private readonly string _fileName;

		public AddFileCommand(ICurrentUser currentUser, string[] args)
		{
			_currentUser = currentUser;
			_filePath = args[0];
			_fileName = args.Length > 1 ? args[1] : _filePath;
		}

		public override void Execute()
		{
			try
			{
				 _currentUser.IsLogged();
				 var fs = _currentUser.GetFileSystem();
				 var ass = _currentUser.GetAccountStorage();
				if (!fs.ExistByPath(_filePath))
				{
					throw new Exception("File is missing");
				}
            
				var fileSize = fs.GetFileSizeInBytesFileNotRegistered(_filePath);
				_currentUser.IsAllowedUserFile(fileSize);
				ass.AddFile(_currentUser.GetUser(), _fileName, fileSize);
				fs.Add(_filePath, _fileName);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}