namespace SF.Domain
{
    public class FileSystem
    {
        private readonly List<FileDescriptor> files = new List<FileDescriptor>();

        public void Add(string filePath, string name)
        {
            var fileDescriptor = new FileDescriptor(name, filePath);
            files.Add(fileDescriptor);
        }

        public void Remove(string name)
        {
            files.Remove(files.FirstOrDefault(e => e.Name == name));
        }

        public List<FileDescriptor> GetAll()
        {
            return files;
        }

        public string GetFileFullPath(string name)
        {
            FileInfo fileInfo = new FileInfo(GetFilePath(name));
            return fileInfo.FullName;
        }

        public long GetFileSizeInBytes(string name)
        {
            var filePath = GetFilePath(name);
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
            
            var filePath = GetFilePath(name);
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
            if (File.Exists(filePath))
            {
                return true;
            }
            return false;
        }

        public T Execute<T>(string name, IFileActionStrategy<T> strategy)
        {
            return strategy.Execute(GetFilePath(name));
        }

        private string GetFilePath(string name)
        {
            var file = files.FirstOrDefault(e => e.Name == name);
            if (file == null)
            {
                throw new FileNotFoundException($"File '{name}' not found.");
            }
            return file.FilePath;
        }
    }
}
