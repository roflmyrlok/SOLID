﻿using DI.Core;
using SF.Domain;
using SF.Domain.Actions;
using SF.Starter;


var diContainer = new DiContainer();

diContainer.Register<IFileSystem, FileSystem>(Scope.Singleton);
diContainer.Register<IFileActionStrategy<string>, CsvTableFileActionStrategy>(Scope.Transient);
diContainer.Register<IFileActionStrategy<string>, JsonTableFileActionStrategy>(Scope.Transient);
diContainer.Register<IFileActionStrategy<List<string>>, SummaryFileActionStrategy>(Scope.Transient);
diContainer.Register<IFileActionStrategy<bool>, JsonValidationFileActionStrategy>(Scope.Transient);
diContainer.Register<IFileActionStrategy<bool>, CsvValidationFileActionStrategy>(Scope.Transient);


var factory = new InputActionsFactory(diContainer);
var actions = factory.GetAllActions();
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