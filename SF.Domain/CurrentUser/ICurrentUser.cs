namespace SF.Domain;

public interface ICurrentUser
{
	void Login(string accountName);
	void Login(string accountName, string password);
	void IsLogged();
	IFileSystem GetFileSystem();
	IAccountStorage GetAccountStorage();
	string GetUser();
	bool IsAllowedUserFile(long fileSize);
	bool IsAllowedToChangePlan(string planName);
	void ChangePlan(string planName);
	
	
}