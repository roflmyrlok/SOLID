using SF.Core;
using SF.Domain;
using System;

namespace SF.Commands
{
	[InputAction(new string[] {"any"})]
	public class RemoveFileInputAction : InputAction<RemoveFileCommand>
	{
		protected override string Action => "remove";
		protected override string HelpString => "remove a file";
		
		protected override string[] SupportedExtensions => ["any"];

		private readonly ISystemWrapper _systemWrapper;

		public RemoveFileInputAction(ISystemWrapper systemWrapper)
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
		private readonly ISystemWrapper _systemWrapper;
		private readonly string _filePath;

		public RemoveFileCommand(ISystemWrapper systemWrapper, string filePath)
		{
			_systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));
			_filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
		}

		public override void Execute()
		{
			try
			{
				_systemWrapper.Remove(_filePath);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}