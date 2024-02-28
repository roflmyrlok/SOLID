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

		private readonly ISystemWrapper _systemWrapper;

		public ChangePlanInputAction(ISystemWrapper systemWrapper)
		{
			_systemWrapper = systemWrapper;
		}

		protected override ChangePlan GetCommandInternal(string[] args)
		{
			return new ChangePlan(_systemWrapper, args);
		}
	}

	[Command]
	public class ChangePlan : Command
	{
		private string _planName;
		private readonly ISystemWrapper _systemWrapper;

		public ChangePlan(ISystemWrapper systemWrapper, string[] args)
		{
			_planName = args[0];
			_systemWrapper = systemWrapper;
		}

		public override void Execute()
		{
			try
			{
				var isAllowedToChangePlan = _systemWrapper.IsAllowedToChangePlan(_planName);
				if (isAllowedToChangePlan)
				{
					_systemWrapper.ChangePlan(_planName);
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