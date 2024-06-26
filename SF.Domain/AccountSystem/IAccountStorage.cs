namespace  SF.Domain;


public interface IAccountStorage
{
	bool Login(string accountName, string password);

	bool IsAllowedToAccessFile(string accountName, string filePath);

	long GetAllowedSize(string accountName);

	long GetAllowedNumber(string accountName);

	bool ChangePlan(string accountName, string newPlan);

	bool AddFile(string accountName, string filePath, long size);

	bool RemoveFile(string accountName, string filePath);

	bool AccountExist(string accountName);
}