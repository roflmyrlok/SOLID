using System.Reflection;
using DI.Core;
using SF.Core;
using SF.Core.ExternalInterfaces;
using SF.Domain;

namespace SF.Starter
{
	public class InputActionsFactory : IInputCommandFactory
	{
		private readonly IDiContainer _diContainer;

		public InputActionsFactory(IDiContainer diContainer)
		{
			this._diContainer = diContainer;
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
				result.Add(_diContainer.Instantiate<IInputAction>(type));
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
		public void SetUp()
		{
			var actions = GetAllActions();
			var actionsByType = new Dictionary<string, List<string>>();
			var anyCommands = new List<string>();
			foreach (var action in actions)
			{
				var name = action.GetAction();
				var types = action.GetSupportedExtension();
				if (types.Length != 0 && types[0] == "any")
				{
					anyCommands.Add(name);
					continue;
				}
				foreach (var type in types)
				{
					if (!actionsByType.ContainsKey(type))
					{
						actionsByType.Add(type, new List<string>());
					}
					actionsByType[type].Add(name);
				}
			}

			foreach (var type in actionsByType)
			{
				foreach (var command in anyCommands)
				{
					type.Value.Add(command);
				}
			}

			var tmp = _diContainer.Resolve<ISupportedCommands>();
			tmp.SetUpActionStrategies(actionsByType);
		}
	}
	
}