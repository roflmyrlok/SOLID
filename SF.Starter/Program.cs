using DI.Core;
using SF.Domain;
using SF.Starter;


var diContainer = new DiContainer();
diContainer.Register<ISystemWrapper, SystemWrapper>(Scope.Singleton);

var actionStrategyFactory = new ActionStrategyFactory(diContainer);
actionStrategyFactory.RegisterAllStrategies();


var inputActionsFactory = new InputActionsFactory(diContainer);
var actions = inputActionsFactory.GetAllActions();
inputActionsFactory.SetUp();


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