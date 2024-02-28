namespace SF.Domain
{
    public class AccountStorage
    {
        private static readonly Dictionary<Plan, PlanLimits> _planLimitsDictionary = new Dictionary<Plan, PlanLimits>()
        {
            { Plan.Basic, new PlanLimits(100 * 1024 * 1024, 10) },
            { Plan.Gold, new PlanLimits(1024 * 1024 * 1024, 100) }
        };

        private readonly Dictionary<string, User> _users = new Dictionary<string, User>();
        private readonly Dictionary<string, string> _accountsPasswords = new Dictionary<string, string>();

        public bool Login(string accountName, string password)
        {
            if (!_users.ContainsKey(accountName) && !_accountsPasswords.ContainsKey(accountName))
            {
                _users.Add(accountName, new User { AccountName = accountName, Plan = Plan.Basic });
                _accountsPasswords.Add(accountName, password);
                return true;
            }

            return _users.ContainsKey(accountName) && _accountsPasswords.ContainsKey(accountName) && _accountsPasswords[accountName] == password;
        }

        public bool IsAllowed(string accountName, string filePath)
        {
            var userFiles = _users[accountName].Files;
            return userFiles.Any(file => file.FilePath == filePath);
        }

        public long GetAllowedSize(string accountName)
        {
            var userPlan = _users[accountName].Plan;
            var planLimits = _planLimitsDictionary[userPlan];
            var remainingSize = planLimits.MaxFileSizeBytes;
            foreach (var file in _users[accountName].Files)
            {
                remainingSize -= file.SizeInBytes;
            }
            return remainingSize;
        }

        public long GetAllowedNumber(string accountName)
        {
            var userPlan = _users[accountName].Plan;
            var planLimits = _planLimitsDictionary[userPlan];
            var remainingSize = planLimits.MaxFiles;
            foreach (var file in _users[accountName].Files)
            {
                remainingSize -= 1;
            }
            return remainingSize;
        }
/*
        public bool ChangePlan(string accountName, Plan newPlan)
        {
            var currentPlan = _users[accountName].Plan;
            var currentPlanLimits = _planLimitsDictionary[currentPlan];
            var newPlanLimits = _planLimitsDictionary[newPlan];

            var userFiles = _users[accountName].Files;
            if (newPlan == Plan.Basic &&
                (userFiles.Count > currentPlanLimits.MaxFiles || GetTotalSizeOfFiles(userFiles) > currentPlanLimits.MaxFileSizeBytes))
            {
                throw new Exception("Downgrading to Basic plan is not allowed while exceeding file limits.");
            }

            _users[accountName].Plan = newPlan;
            return true;
        }
        */

        public bool AddFile(string accountName, string filePath, long size)
        {
            var userPlan = _users[accountName].Plan;
            var planLimits = _planLimitsDictionary[userPlan];
            if (_users[accountName].Files.Count >= planLimits.MaxFiles)
            {
                throw new Exception("Exceeding maximum number of files allowed for this plan.");
            }
            var localFile = new LocalFile { FilePath = filePath, SizeInBytes = size };
            _users[accountName].Files.Add(localFile);
            return true;
        }
        
        public bool RemoveFile(string accountName, string filePath)
        {
            var file = GetFileByPath(accountName, filePath);
            if (file is null)
            {
                return false;
            }
            _users[accountName].Files.Remove(file);
            return true;
        }
/*
        public bool UpdateFile(string accountName, string oldFilePath, string newFilePath, long newSize)
        {
            var user = _users[accountName];
            var fileToUpdate = user.Files.FirstOrDefault(file => file.FilePath == oldFilePath);
            if (fileToUpdate == null)
            {
                throw new Exception("The file to update does not exist or you do not have access to it.");
            }

            var userPlan = user.Plan;
            var planLimits = _planLimitsDictionary[userPlan];
            if (newSize > planLimits.MaxFileSizeBytes)
            {
                throw new Exception("Exceeding maximum file size allowed for this plan.");
            }

            fileToUpdate.FilePath = newFilePath;
            fileToUpdate.SizeInBytes = newSize;

            return true;
        }
*/
        public bool AccountExist(string accountName)
        {
            return _users.ContainsKey(accountName);
        }
        
        private LocalFile? GetFileByPath(string accountName, string filePath)
        {
            var userFiles = _users[accountName].Files;
            return userFiles.FirstOrDefault(file => file.FilePath == filePath);
        }
    }
}
