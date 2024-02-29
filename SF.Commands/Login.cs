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

		private readonly ICurrentUser _systemWrapper;

		private readonly IAccountStorage _accountStorage;

		public LoginInputAction(ICurrentUser systemWrapper, IAccountStorage accountStorage)
		{
			_systemWrapper = systemWrapper;
			_accountStorage = accountStorage;
		}

		protected override Login GetCommandInternal(string[] args)
		{
			return new Login(_systemWrapper, _accountStorage, args);
		}
	}

	[Command]
	public class Login : Command
	{
		private string _accountName;
		private readonly ICurrentUser _systemWrapper;
		private readonly IAccountStorage _accountStorage;

		public Login(ICurrentUser systemWrapper, IAccountStorage accountStorage, string[] args)
		{
			_accountName = args[0];
			_systemWrapper = systemWrapper;
			_accountStorage = accountStorage;
		}

		public override void Execute()
		{
			try
			{
				_systemWrapper.Login(_accountName, _accountStorage);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}