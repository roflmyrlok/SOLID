namespace SF.Domain
{
    public class SystemWrapper : ISystemWrapper
    {
        private Dictionary<string, FileSystem> _userFileSystem = new Dictionary<string, FileSystem>();
        private Dictionary<string, List<string>> _typeSupportedActions = new Dictionary<string, List<string>>();
        private AccountStorage _accountStorage = new AccountStorage();
        private string _currentAccount;
        private FileSystem _currentfileSystem;
        
        
        //public interface granted methods
        public void SetUp(Dictionary<string, List<string>> typeSupportedActions)
        {
            _typeSupportedActions = typeSupportedActions;
        }

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
                    _currentfileSystem = _userFileSystem[accountName];
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
                _currentfileSystem = _userFileSystem[accountName];
                return true;
            }
            throw new Exception("Bad Error");
            
        }

        public List<string> GetSupportedCommands(string type)
        {
            IsLogged();
            return _typeSupportedActions[type];
        }

        public void Add(string filePath, string fileName)
        {
            IsLogged();
            
            if (!_currentfileSystem.ExistByPath(filePath))
            {
                throw new Exception("File is missing");
            }
            
            var fileSize = _currentfileSystem.GetFileSizeInBytesFileNotRegistered(filePath);
            IsAllowedUserFile(fileSize);
            _accountStorage.AddFile(_currentAccount, fileName, fileSize);
            _currentfileSystem.Add(filePath, fileName);
            
        }

        public void Remove(string fileName)
        {
            IsLogged();
            if (!_accountStorage.IsAllowed(_currentAccount, fileName))
            {
                throw new Exception("You are not allowed to remove this file.");
            }
            if (!_currentfileSystem.ExistByName(fileName))
            {
                throw new Exception($"No file with name {fileName}");
            }
            _accountStorage.RemoveFile(_currentAccount, fileName);
            _currentfileSystem.Remove(fileName);
        }

        public List<string> GetAllFiles()
        {
            IsLogged();
            var tmp = _currentfileSystem.GetAll();
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
            if (!_accountStorage.IsAllowed(_currentAccount, fileName))
            {
                throw new Exception("You are not allowed to access this file.");
            }

            if (!_currentfileSystem.ExistByName(fileName))
            {
                throw new Exception($"No file with name {fileName}");
            }
            return _currentfileSystem.GetFileSizeInBytes(fileName);
        }

        public string GetFileFullPath(string fileName)
        {
            IsLogged();
            if (!_accountStorage.IsAllowed(_currentAccount, fileName))
            {
                throw new Exception("You are not allowed to access this file.");
            }

            if (!_currentfileSystem.ExistByName(fileName))
            {
                throw new Exception($"No file with name {fileName}");
            }
            return _currentfileSystem.GetFileFullPath(fileName);
        }

        public string GetFileExtension(string fileName)
        {
            IsLogged();
            if (!_accountStorage.IsAllowed(_currentAccount, fileName))
            {
                throw new Exception("You are not allowed to access this file.");
            }

            if (!_currentfileSystem.ExistByName(fileName))
            {
                throw new Exception($"No file with name {fileName}");
            }
            return _currentfileSystem.GetFileExtension(fileName);
        }

        public T Execute<T>(string fileName, IFileActionStrategy<T> strategy)
        {
            IsLogged();
            if (!_accountStorage.IsAllowed(_currentAccount, fileName))
            {
                throw new Exception("You are not allowed to access this file.");
            }

            if (!_currentfileSystem.ExistByName(fileName))
            {
                throw new Exception($"No file with name {fileName}");
            }
            return _currentfileSystem.Execute(fileName, strategy);
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
