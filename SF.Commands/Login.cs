using SF.Commands.Actions;
using SF.Core;
using SF.Domain;


namespace SF.Commands
{
	[InputAction]
	public class LoginInputAction : InputAction<Login>
	{
		protected override string Action => "login";
		protected override string HelpString => "login";
		
		protected override string[] SupportedExtensions => [];

		private readonly ICurrentUser _currentUser;

		private readonly IAccountStorage _accountStorage;

		public LoginInputAction(ICurrentUser currentUser, IAccountStorage accountStorage)
		{
			_currentUser = currentUser;
			_accountStorage = accountStorage;
		}

		protected override Login GetCommandInternal(string[] args)
		{
			return new Login(_currentUser, _accountStorage, args);
		}
	}

	[Command]
	public class Login : Command
	{
		private string _accountName;
		private readonly ICurrentUser _currentUser;
		private readonly IAccountStorage _accountStorage;

		public Login(ICurrentUser currentUser, IAccountStorage accountStorage, string[] args)
		{
			_accountName = args[0];
			_currentUser = currentUser;
			_accountStorage = accountStorage;
		}

		public override void Execute()
		{
			try
			{
				_currentUser.Login(_accountName, _accountStorage);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}