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

		private readonly ISystemWrapper _systemWrapper;

		public LoginInputAction(ISystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override Login GetCommandInternal(string[] args)
		{
			return new Login(_systemWrapper, args);
		}
	}

	[Command]
	public class Login : Command
	{
		private string _accountName;
		private readonly ISystemWrapper _systemWrapper;

		public Login(ISystemWrapper systemWrapper, string[] args)
		{
			_accountName = args[0];
			_systemWrapper = systemWrapper;
		}

		public override void Execute()
		{
			try
			{
				var attempt = _systemWrapper.Login(_accountName);
				if (attempt == false)
				{
					Console.WriteLine("wrong password");
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