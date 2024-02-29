namespace SF.Domain.CurrentUser;

public class CurrentUser : ICurrentUser, ICurrentUserStarter
{
	public string _currentUser;
	public IFileSystem _currentFileSystem;
	public IEventCollector _eventCollector;
	public IAccountStorage _accountStorage;
	public Dictionary<string, FileSystem> _userFileSystem = new Dictionary<string, FileSystem>();

	public void Login(string accountName)
	{
		Login(accountName, password:"");
	}
	public void Login(string accountName, string password)
	{
		_eventCollector.CollectEvent("user_logged_in", DateTime.Now, new Dictionary<string, List<string>> { { "user_name", new List<string> { accountName } } });
		if (accountName.Length <= 5)
		{
			throw new Exception("Account name must be at least 5 symbols");
		}
		if (_accountStorage.AccountExist(accountName))
		{
			if (_accountStorage.Login(accountName, password))
			{
				_currentUser = accountName;
				_currentFileSystem = _userFileSystem[accountName];
				return;
			}
			//wrong password
			throw new Exception("Password is incorrect");
		}
		//new user
		if (_accountStorage.Login(accountName, password))
		{
			_currentUser = accountName;
			var newFileSys = new FileSystem();
			_userFileSystem.Add(accountName, newFileSys);
			_currentFileSystem = _userFileSystem[accountName];
			_currentFileSystem.PathEventCollector(_eventCollector);
			return;
		}
		throw new Exception("Bad Error");
	}
	public void IsLogged()
	{
		if (_currentUser is null)
		{
			throw new Exception("You have to login in your account");
		}
	}
	
	public bool IsAllowedUserFile(long fileSize)
	{
		var allowedSize = _accountStorage.GetAllowedSize(_currentUser);
		var allowedNumber = _accountStorage.GetAllowedNumber(_currentUser);
		if (allowedNumber <= 0)
		{
			_eventCollector.CollectEvent("limit_reached", DateTime.Now, new Dictionary<string, List<string>>
			{
				{ "limit_type", new List<string> { "files_amount" } }
			});

			throw new Exception("You have reached files number limit");
		}
		if (allowedSize <= fileSize)
		{
			_eventCollector.CollectEvent("limit_reached", DateTime.Now, new Dictionary<string, List<string>>
			{
				{ "limit_type", new List<string> { "storage" } }
			});
			throw new Exception("You have reached files size limit");
		}
		return true;
	}
	
	public bool IsAllowedToChangePlan(string planName)
	{
		IsLogged();
		Plan newPlan;
		switch (planName)
		{
			case "gold": newPlan = Plan.Gold; break;
			case "basic": newPlan = Plan.Basic; break;
			default: throw new Exception("Plan is not available");
		}

		try
		{
			return _accountStorage.ChangePlan(_currentUser, newPlan);
		}
		catch (Exception e)
		{
			return false;
		}
			
	}
	
	public void ChangePlan(string planName)
	{
		IsLogged();
		Plan newPlan;
		switch (planName)
		{
			case "gold": newPlan = Plan.Gold; break;
			case "basic": newPlan = Plan.Basic; break;
			default: throw new Exception("Plan is not available");
		}
		_accountStorage.ChangePlan(_currentUser, newPlan);
		_eventCollector.CollectEvent("plan_changed", DateTime.Now, new Dictionary<string, List<string>>
		{
			{ "user_name", new List<string> { _currentUser } },
			{ "plan_name", new List<string> { newPlan.ToString() } }
		});
	}

	public IFileSystem GetFileSystem()
	{
		return _currentFileSystem;
	}

	public IAccountStorage GetAccountStorage()
	{
		return _accountStorage;
	}

	public string GetUser()
	{
		return _currentUser;
	}

	public void PathCurrentUser(IEventCollector eventCollector,
		IAccountStorage accountStorage,
		Dictionary<string, FileSystem> userFileSystem)
	{
		_eventCollector = eventCollector;
		_accountStorage = accountStorage;
		_userFileSystem = userFileSystem;
	}
}