namespace SF.Domain
{
    public class FileSystem : IFileSystem, IFileSystemLogger
    {
        public readonly List<FileDescriptor> files = new List<FileDescriptor>();
        public IEventCollector EventCollector;
        
        private string _dataDirectoryPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "test_data");
        
        public void Add(string filePath, string name)
        {
            filePath = GetFileConfiguredPath(filePath);
            var fileDescriptor = new FileDescriptor(name, filePath);
            files.Add(fileDescriptor);
             EventCollector.CollectEvent("file_added", DateTime.Now, new Dictionary<string, List<string>>
            {
                { "shortcut", new List<string> { name } },
                { "filetype", new List<string> {  GetFileExtension(name) } }
            });
        }

        public void Remove(string name)
        {
            files.Remove(files.FirstOrDefault(e => e.Name == name));
            EventCollector.CollectEvent("file_removed", DateTime.Now, new Dictionary<string, List<string>>
            {
                { "shortcut", new List<string> { name } },
                { "filetype", new List<string> { GetFileExtension(name) } }
            });
        }

        public List<FileDescriptor> GetAll()
        {
            return files;
        }

        public string GetFileFullPath(string name)
        {
            FileInfo fileInfo = new FileInfo(GetFilePathByName(name));
            return fileInfo.FullName;
        }

        public long GetFileSizeInBytes(string name)
        {
            var filePath = GetFilePathByName(name);
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                long fileSizeInBytes = fileInfo.Length;
                return fileSizeInBytes;
            }
            return 0;
        }
        
        public long GetFileSizeInBytesFileNotRegistered(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                long fileSizeInBytes = fileInfo.Length;
                return fileSizeInBytes;
            }
            return 0;
        }

        public string GetFileExtension(string name)
        {
            var filePath = GetFilePathByName(name);
            return Path.GetExtension(filePath);
        }

        public bool ExistByName(string name)
        {
            var file = files.FirstOrDefault(e => e.Name == name);
            if (file == null)
            {
                return false;
            }
            if (File.Exists(file.FilePath))
            {
                return true;
            }
            return false;
        }
        
        public bool ExistByPath(string filePath)
        {
            filePath = GetFileConfiguredPath(filePath);
            if (File.Exists(filePath))
            {
                return true;
            }
            return false;
        }

        public T Execute<T>(string name, IFileActionStrategy<T> strategy)
        {
            return strategy.Execute(GetFilePathByName(name));
        }

        private string GetFilePathByName(string name)
        {
            var file = files.FirstOrDefault(e => e.Name == name);
            if (file == null)
            {
                throw new FileNotFoundException($"File '{name}' not found.");
            }
            return file.FilePath;
        }
        
        private string GetFileConfiguredPath(string fileName)
        {
            return Path.Combine(_dataDirectoryPath, fileName);
        }

        public void PathEventCollector(IEventCollector eventCollector)
        {
            EventCollector = eventCollector;
        }

    }
}
