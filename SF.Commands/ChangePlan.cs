using SF.Core;
using SF.Domain;
using System;
using System.Collections.Generic;

namespace SF.Commands
{
	[InputAction]
	public class ChangePlanInputAction : InputAction<ChangePlan>
	{
		protected override string Action => "change_plan";
		protected override string HelpString => "change plan";
		
		protected override string[] SupportedExtensions => [];

		private readonly ICurrentUser _currentUser;

		public ChangePlanInputAction(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		protected override ChangePlan GetCommandInternal(string[] args)
		{
			return new ChangePlan(_currentUser, args);
		}
	}

	[Command]
	public class ChangePlan : Command
	{
		private string _planName;
		private readonly ICurrentUser _currentUser;

		public ChangePlan(ICurrentUser currentUser, string[] args)
		{
			_planName = args[0];
			_currentUser = currentUser;
		}

		public override void Execute()
		{
			try
			{
				var isAllowedToChangePlan = _currentUser .IsAllowedToChangePlan(_planName);
				if (isAllowedToChangePlan)
				{
					 _currentUser .ChangePlan(_planName);
					return;
				}
				Console.WriteLine("Pls remove some files before changing to a new plan");
				
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}