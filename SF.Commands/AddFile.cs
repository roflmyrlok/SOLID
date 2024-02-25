using SF.Core;
using SF.Domain;

namespace SF.Commands
{
	[InputAction]
	public class AddFileInputAction : InputAction<AddFileCommand>
	{
		protected override string Action => "add";
		protected override string HelpString => "add a file";

		private readonly SystemWrapper _systemWrapper;

		public AddFileInputAction(SystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override AddFileCommand GetCommandInternal(string[] args)
		{
			return new AddFileCommand(_systemWrapper, args);
		}
	}

	[Command]
	public class AddFileCommand : Command
	{
		private readonly string _filePath;
		private readonly SystemWrapper _systemWrapper;
		private readonly string _name;

		public AddFileCommand(SystemWrapper systemWrapper, string[] args)
		{
			_systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));

			if (args == null || args.Length < 1)
			{
				throw new ArgumentException("File path must be provided.");
			}

			_filePath = args[0];
			_name = args.Length > 1 ? args[1] : _filePath;
		}

		public override void Execute()
		{
			try
			{
				_systemWrapper.Add(_filePath, _name);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}