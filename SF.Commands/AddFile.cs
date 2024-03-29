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
		private readonly IAccountStorage _accountStorage;

		public AddFileInputAction(ICurrentUser currentUser, IAccountStorage accountStorage)
		{
			this._currentUser = currentUser;
			this._accountStorage = accountStorage;
		}

		protected override AddFileCommand GetCommandInternal(string[] args)
		{
			return new AddFileCommand(_currentUser, _accountStorage, args);
		}
	}

	[Command]
	public class AddFileCommand : Command
	{
		private readonly string _filePath;
		private readonly ICurrentUser _currentUser;
		private readonly string _fileName;
		private readonly IAccountStorage _accountStorage;

		public AddFileCommand(ICurrentUser currentUser, IAccountStorage accountStorage, string[] args)
		{
			_currentUser = currentUser;
			_accountStorage = accountStorage;
			_filePath = args[0];
			if (args.Length == 1)
			{
				_fileName = args[0];
			}
			else
			{
				_fileName = args[1];
			}
		}

		public override void Execute()
		{
			try
			{
				 _currentUser.IsLogged();
				 var fs = _currentUser.GetFileSystem();
				if (!fs.ExistByPath(_filePath))
				{
					throw new Exception("File is missing");
				}
            
				var fileSize = fs.GetFileSizeInBytesFileNotRegistered(_filePath);
				_currentUser.IsAllowedUserFile(fileSize, _accountStorage);
				_accountStorage.AddFile(_currentUser.GetUser(), _fileName, fileSize);
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