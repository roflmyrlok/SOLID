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

		private readonly ICurrentUser _systemWrapper;

		public RemoveFileInputAction(ICurrentUser systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override RemoveFileCommand GetCommandInternal(string[] args)
		{
			if (args.Length < 1)
			{
				throw new ArgumentException("File name is required for remove command.");
			}

			return new RemoveFileCommand(_systemWrapper, args[0]);
		}
	}

	[Command]
	public class RemoveFileCommand : Command
	{
		private readonly ICurrentUser _systemWrapper;
		private readonly string _filePath;

		public RemoveFileCommand(ICurrentUser systemWrapper, string filePath)
		{
			_systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));
			_filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
		}

		public override void Execute()
		{
			try
			{
				_systemWrapper.IsLogged();
				var _accountStorage = _systemWrapper.GetAccountStorage();
				var _currentFileSystem = _systemWrapper.GetFileSystem();
				if (!_accountStorage.IsAllowedToAccessFile(_systemWrapper.GetUser(), _filePath))
				{
					throw new Exception("You are not allowed to remove this file.");
				}
				if (!_currentFileSystem.ExistByName(_filePath))
				{
					throw new Exception($"No file with name {_filePath}");
				}
				_accountStorage.RemoveFile(_systemWrapper.GetUser(), _filePath);
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