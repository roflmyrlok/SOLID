using Newtonsoft.Json;

namespace SF.Domain
{
	public class SystemWrapper : ISystemWrapper
	{
		[JsonProperty]
		protected Dictionary<string, FileSystem> _userFileSystem = new Dictionary<string, FileSystem>();
		[JsonProperty]
		private Dictionary<string, List<string>> _typeSupportedActions = new Dictionary<string, List<string>>();
		[JsonProperty]
		private AccountStorage _accountStorage = new AccountStorage();
		private string _currentAccount;
		private FileSystem _currentFileSystem;

		public void Restore(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return;
			}
			string json = File.ReadAllText(filePath);
			SystemWrapper restoredSystemWrapper = JsonConvert.DeserializeObject<SystemWrapper>(json);

			
			_userFileSystem = restoredSystemWrapper._userFileSystem;
			_typeSupportedActions = restoredSystemWrapper._typeSupportedActions;
			_accountStorage = restoredSystemWrapper._accountStorage;
			_currentAccount = restoredSystemWrapper._currentAccount;
			_currentFileSystem = restoredSystemWrapper._currentFileSystem;
		}

		public void Save(string filePath)
		{
			string json = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText(filePath, json);
			Console.WriteLine("System saved");
		}
        
		//public interface granted methods
		
		public void SetUp(Dictionary<string, List<string>> typeSupportedActions)
		{
			_typeSupportedActions = typeSupportedActions;
		}
		//account related commands
		public bool Login(string accountName)
		{
			return Login(accountName, password: "");
		}
		public bool Login(string accountName, string password)
		{
			if (accountName.Length <= 5)
			{
				throw new Exception("Account name must be at least 5 symbols");
			}
			//old user
			if (_accountStorage.AccountExist(accountName))
			{
				if (_accountStorage.Login(accountName, password))
				{
					_currentAccount = accountName;
					_currentFileSystem = _userFileSystem[accountName];
					return true;
				}
				//wrong password
				return false;
			}
			//new user
			if (_accountStorage.Login(accountName, password))
			{
				_currentAccount = accountName;
				var newFileSys = new FileSystem();
				_userFileSystem.Add(accountName, newFileSys);
				_currentFileSystem = _userFileSystem[accountName];
				return true;
			}
			throw new Exception("Bad Error");
            
		}

		public bool IsAllowedToChangePlan(string planName)
		{
			IsLogged();
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
				return _accountStorage.ChangePlan(_currentAccount, newPlan);
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
			_accountStorage.ChangePlan(_currentAccount, newPlan);
		}
		//files related commands
		public List<string> GetSupportedCommands(string type)
		{
			IsLogged();
			return _typeSupportedActions[type];
		}
		public void Add(string filePath, string fileName)
		{
			IsLogged();
            
			if (!_currentFileSystem.ExistByPath(filePath))
			{
				throw new Exception("File is missing");
			}
            
			var fileSize = _currentFileSystem.GetFileSizeInBytesFileNotRegistered(filePath);
			IsAllowedUserFile(fileSize);
			_accountStorage.AddFile(_currentAccount, fileName, fileSize);
			_currentFileSystem.Add(filePath, fileName);
            
		}

		public void Remove(string fileName)
		{
			IsLogged();
			if (!_accountStorage.IsAllowedToAccessFile(_currentAccount, fileName))
			{
				throw new Exception("You are not allowed to remove this file.");
			}
			if (!_currentFileSystem.ExistByName(fileName))
			{
				throw new Exception($"No file with name {fileName}");
			}
			_accountStorage.RemoveFile(_currentAccount, fileName);
			_currentFileSystem.Remove(fileName);
		}

		public List<string> GetAllFiles()
		{
			IsLogged();
			var tmp = _currentFileSystem.GetAll();
			var ans = new List<string>();
			foreach (var file in tmp)
			{
				ans.Add(file.Name + " at " + file.FilePath);
			}
			return ans;
		}

		public long GetFileSizeInBytes(string fileName)
		{
			IsLogged();
			if (!_accountStorage.IsAllowedToAccessFile(_currentAccount, fileName))
			{
				throw new Exception("You are not allowed to access this file.");
			}

			if (!_currentFileSystem.ExistByName(fileName))
			{
				throw new Exception($"No file with name {fileName}");
			}
			return _currentFileSystem.GetFileSizeInBytes(fileName);
		}

		public string GetFileFullPath(string fileName)
		{
			IsLogged();
			if (!_accountStorage.IsAllowedToAccessFile(_currentAccount, fileName))
			{
				throw new Exception("You are not allowed to access this file.");
			}

			if (!_currentFileSystem.ExistByName(fileName))
			{
				throw new Exception($"No file with name {fileName}");
			}
			return _currentFileSystem.GetFileFullPath(fileName);
		}

		public string GetFileExtension(string fileName)
		{
			IsLogged();
			if (!_accountStorage.IsAllowedToAccessFile(_currentAccount, fileName))
			{
				throw new Exception("You are not allowed to access this file.");
			}

			if (!_currentFileSystem.ExistByName(fileName))
			{
				throw new Exception($"No file with name {fileName}");
			}
			return _currentFileSystem.GetFileExtension(fileName);
		}

		public T Execute<T>(string fileName, IFileActionStrategy<T> strategy)
		{
			IsLogged();
			if (!_accountStorage.IsAllowedToAccessFile(_currentAccount, fileName))
			{
				throw new Exception("You are not allowed to access this file.");
			}

			if (!_currentFileSystem.ExistByName(fileName))
			{
				throw new Exception($"No file with name {fileName}");
			}
			return _currentFileSystem.Execute(fileName, strategy);
		}

        
		// private internal methods
		private void IsLogged()
		{
			if (_currentAccount is null)
			{
				throw new Exception("You have to login in your account");
			}
		}
        
		private bool IsAllowedUserFile(long fileSize)
		{
			var allowedSize = _accountStorage.GetAllowedSize(_currentAccount);
			var allowedNumber = _accountStorage.GetAllowedNumber(_currentAccount);
			if (allowedNumber <= 0)
			{
				throw new Exception("You have reached filesnumber limit");
			}
			if (allowedSize <= fileSize)
			{
				throw new Exception("You have reached files size limit");
			}
			return true;
		}
	}
}