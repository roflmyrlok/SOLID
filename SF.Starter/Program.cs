using DI.Core;
using SF.Domain;
using SF.Domain.CurrentUser;
using SF.Starter;

// currently all files will be searched in this directory:
/*
 
 
Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "test_data");


at this file system save will be stored

Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "save") + "/save_1";


 */

var diContainer = new DiContainer();
var defaultSaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "save") + "/save_1";
diContainer.Register<IEventCollector, EventCollector>(Scope.Singleton);
diContainer.Register<IAccountStorage, AccountStorage>(Scope.Singleton);
diContainer.Register<IAccountStorageStarter, AccountStorage>(Scope.Singleton);
diContainer.Register<IFileSystem, FileSystem>(Scope.Singleton);
diContainer.Register<ICurrentUser,CurrentUser>(Scope.Singleton);
diContainer.Register<ICurrentUserStarter, CurrentUser>(Scope.Singleton);
diContainer.Register<ISupportedCommands, SupportedCommands>(Scope.Singleton);
var eventLogger = diContainer.Resolve<IEventCollector>();
var actionStrategyFactory = new ActionStrategyFactory(diContainer);
actionStrategyFactory.RegisterAllStrategies();
var inputActionsFactory = new InputActionsFactory(diContainer);
var accountStorageStarter = diContainer.Resolve<IAccountStorageStarter>();
accountStorageStarter.PathAccountStorage( eventLogger, new Dictionary<string, User>(), new Dictionary<string, string>());
var currentUser = diContainer.Resolve<ICurrentUserStarter>();
currentUser.PathCurrentUser(eventLogger, diContainer.Resolve<IAccountStorage>(), new Dictionary<string, FileSystem>());
var actions = inputActionsFactory.GetAllActions();
inputActionsFactory.SetUp();



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

