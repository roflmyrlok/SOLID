using SF.Core;
using SF.Domain;

namespace SF.Commands
{
	[InputAction]
	public class RemoveFileInputAction : InputAction<RemoveFileCommand>
	{
		protected override string Action => "remove";
		protected override string HelpString => "remove a file";
		
		protected override string[] SupportedExtensions => ["any"];

		private readonly ICurrentUser _currentUser;
		private readonly IAccountStorage _accountStorage;

		public RemoveFileInputAction(ICurrentUser currentUser, IAccountStorage accountStorage)
		{
			_currentUser = currentUser;
			_accountStorage = accountStorage;
		}

		protected override RemoveFileCommand GetCommandInternal(string[] args)
		{
			if (args.Length < 1)
			{
				throw new ArgumentException("File name is required for remove command.");
			}

			return new RemoveFileCommand(_currentUser, _accountStorage, args[0]);
		}
	}

	[Command]
	public class RemoveFileCommand : Command
	{
		private readonly ICurrentUser _currentUser;
		private readonly string _filePath;
		private readonly IAccountStorage _accountStorage;

		public RemoveFileCommand(ICurrentUser currentUser, IAccountStorage accountStorage, string filePath)
		{
			_currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
			_filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
			_accountStorage = accountStorage;
		}

		public override void Execute()
		{
			try
			{
				_currentUser.IsLogged();
				var _currentFileSystem = _currentUser.GetFileSystem();
				if (!_accountStorage.IsAllowedToAccessFile(_currentUser.GetUser(), _filePath))
				{
					throw new Exception("You are not allowed to remove this file.");
				}
				if (!_currentFileSystem.ExistByName(_filePath))
				{
					throw new Exception($"No file with name {_filePath}");
				}
				_accountStorage.RemoveFile(_currentUser.GetUser(), _filePath);
				_currentFileSystem.Remove(_filePath);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}