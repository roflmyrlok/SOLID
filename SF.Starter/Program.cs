using DI.Core;
using SF.Domain;
using SF.Starter;


var diContainer = new DiContainer();

diContainer.Register<ISystemWrapper, SystemWrapper>(Scope.Singleton);

var factory2 = new ActionStrategyFactory(diContainer);
factory2.GetAllStrategies();
var factory = new InputActionsFactory(diContainer);
var actions = factory.GetAllActions();
factory.SetUp();
while (true)
{
    var input = Console.ReadLine().ToLower();
    
    if (!TryHandle(input))
    {
        Console.WriteLine($"Unknown command '{input}', please try again");
    }
}

bool TryHandle(string input)
{
    foreach (var inputAction in actions)
    {
        if (inputAction.CanHandle(input))
        {
            var command = inputAction.GetCommand(input);
            command.Execute();
            return true;
        }
    }
    

    return false;
}