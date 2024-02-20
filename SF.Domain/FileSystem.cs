using System.Reflection;
using SF.Domain.Actions;

namespace SF.Domain
{
    public class FileSystem : IFileSystem
    {
        private readonly List<FileDescriptor> files = new List<FileDescriptor>();

        public bool Add(string filePath, string name)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            var fileDescriptor = new FileDescriptor(name, filePath);
            files.Add(fileDescriptor);
            return true;
        }

        public bool Remove(string arg)
        {
            var file = files.FirstOrDefault(e => e.Name == arg) ?? files.FirstOrDefault(e => e.FilePath == arg);
            if (file != null)
            {
                files.Remove(file);
                return true;
            }
            return false;
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

        public string GetFileExtension(string name)
        {
            
            var filePath = GetFilePath(name);
            return Path.GetExtension(filePath);
        }

        public bool Exist(string name)
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
