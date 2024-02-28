using DI.Core;
using SF.Domain;
using SF.Domain.Actions;
using System.Reflection;
using SF.Domain.ExternalInterfaces;


namespace SF.Starter
{
	public class ActionStrategyFactory : IActionStrategyFactory
	{
		private readonly IDiContainer diContainer;

		public ActionStrategyFactory(IDiContainer diContainer)
		{
			this.diContainer = diContainer;
		}
		
		public void GetAllStrategies()
		{
			var strategies = GetAllAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => !type.IsAbstract)
				.Where(type => type.GetCustomAttribute<ActionStrategieAttribute>() != null);
        
			foreach (var strategyType in strategies)
			{
				var attribute = strategyType.GetCustomAttribute<ActionStrategieAttribute>();
				if (attribute != null)
				{
					//var path = attribute.Path;
					var interfaceType = typeof(IFileActionStrategy<>).MakeGenericType(strategyType);
					var implementationType = strategyType;
					diContainer.Register(interfaceType, implementationType, Scope.Transient);
				}
			}
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