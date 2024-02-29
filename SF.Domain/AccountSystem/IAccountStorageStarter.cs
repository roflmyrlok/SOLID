namespace SF.Domain;

public interface IAccountStorageStarter
{
	void PathAccountStorage(IEventCollector eventCollector, Dictionary<string, User> users,
		Dictionary<string, string> accountsPasswords);
}