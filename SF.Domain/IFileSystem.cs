using SF.Domain.Actions;

namespace SF.Domain;

using System;
using System.IO;

public interface IFileSystem
{ 
    bool Add(string filePath, string name);
    bool Remove(string filePath);
    List<FileDescriptor> GetAll();
    long GetFileSizeInBytes(string filePath);
    string GetFileFullPath(string filePath);

    string GetFileExtension(string filePath);

    bool Exist(string filePath);
    public T Execute<T>(string fileName, IFileActionStrategy<T> strategy);
    

}

