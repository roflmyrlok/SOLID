namespace SF.Domain;

using System;
using System.IO;

public interface IInteractableFile
{ 
    void Add(string filePath, string name);
    void Remove(string filePath);
    List<FileDescriptor> GetAll();
    long GetFileSizeInBytes(string filePath);
    string GetFileFullPath(string filePath);

    string GetFileExtension(string filePath);

    bool Exist(string filePath);
}

