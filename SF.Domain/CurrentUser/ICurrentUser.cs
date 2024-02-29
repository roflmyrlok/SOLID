namespace SF.Domain;

public interface ICurrentUser
{
	void Login(string accountName, IAccountStorage accountStorage);
	void Login(string accountName, string password, IAccountStorage accountStorage);
	void IsLogged();
	bool IsAllowedUserFile(long fileSize, IAccountStorage accountStorage);
	bool IsAllowedToChangePlan(string planName, IAccountStorage accountStorage);
	void ChangePlan(string planName, IAccountStorage accountStorage);
	IFileSystem GetFileSystem();
	string GetUser();
	void PathCurrentUser(IEventCollector eventCollector, Dictionary<string, FileSystem> userFileSystem);
}