using DI.Core;
using SF.Domain;
using SF.Starter;

// currently all files will be searched in this directory:
/*
 
 
Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "test_data");


at this file system save will be stored

Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "save") + "/save_1";


 */

var diContainer = new DiContainer();
var defaultSaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "save") + "/save_1";
diContainer.Register<ISystemWrapper, SystemWrapper>(Scope.Singleton);

var actionStrategyFactory = new ActionStrategyFactory(diContainer);
actionStrategyFactory.RegisterAllStrategies();


var inputActionsFactory = new InputActionsFactory(diContainer);
var actions = inputActionsFactory.GetAllActions();
inputActionsFactory.SetUp();

var saver = diContainer.Resolve<ISystemWrapper>();
saver.Restore(defaultSaveFilePath);

while (true)
{
    var input = Console.ReadLine().ToLower();
    if (input == "q")
    {
        break;
    }
    if (!TryHandle(input))
    {
        Console.WriteLine($"Unknown command '{input}', please try again");
    }
    saver.Save(defaultSaveFilePath);
}

saver.Save(defaultSaveFilePath);

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

