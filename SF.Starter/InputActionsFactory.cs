using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DI.Core;
using SF.Commands;
using SF.Core;
using SF.Core.ExternalInterfaces;
using SF.Domain;

namespace SF.Starter
{
	public class InputActionsFactory : IInputCommandFactory
	{
		private readonly IDiContainer diContainer;

		public InputActionsFactory(IDiContainer diContainer)
		{
			this.diContainer = diContainer;
		}

		public List<IInputAction> GetAllActions()
		{
			var commandTypes = GetAllAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => !type.IsAbstract)
				.Where(type => type.GetCustomAttribute<InputActionAttribute>() != null)
				.Where(type => typeof(IInputAction).IsAssignableFrom(type));

			var result = new List<IInputAction>();
			foreach (var type in commandTypes)
			{
				result.Add(diContainer.Instantiate<IInputAction>(type));
			}
			return result;
			
			
		}

		private IEnumerable<Assembly> GetAllAssemblies()
		{
			foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll"))
			{
				yield return Assembly.LoadFrom(file);
			}
		}
	}
}
