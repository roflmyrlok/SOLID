namespace SF.Domain;

using System;
using System.IO;

public interface IFileRegistry
{ 
    FileDescriptor Add(string filePath, string name);
    void Remove(string filePath);
    List<FileDescriptor> GetAll();
}

public class FileRegistry : IFileRegistry
{
    private static FileRegistry instance;

    public static FileRegistry Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FileRegistry();
            }

            return instance;
        }
    }

    private readonly List<FileDescriptor> files = new List<FileDescriptor>();
    
    public FileDescriptor Add(string name, string filePath)
    {
        var fileDescriptor = new FileDescriptor(name, filePath);
        
        files.Add(fileDescriptor);
        return fileDescriptor;
    }

    public void Remove(string filePath)
    {
        var file = files.FirstOrDefault(e => e.FilePath == filePath);
        if (file != null)
        {
            files.Remove(file);
        }
    }

    public List<FileDescriptor> GetAll()
    {
        return files;
    }
}