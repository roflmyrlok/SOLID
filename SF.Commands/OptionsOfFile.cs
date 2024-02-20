using SF.Core;
using SF.Domain;
using System;
using System.Collections.Generic;

namespace SF.Commands
{
    [InputAction]
    public class OptionsOfFileInputAction : InputAction<OptionsOfFileCommand>
    {
        protected override string Module => "file";
        protected override string Action => "options";
        protected override string HelpString => "options of a file";

        private readonly IFileSystem _fileSystem;

        public OptionsOfFileInputAction(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        protected override OptionsOfFileCommand GetCommandInternal(string[] args)
        {
            return new OptionsOfFileCommand(_fileSystem, args);
        }
    }

    [Command]
    public class OptionsOfFileCommand : Command
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _filePath;

        public OptionsOfFileCommand(IFileSystem fileSystem, string[] args)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

            if (args.Length < 1)
            {
                throw new ArgumentException("File path argument is missing.");
            }

            _filePath = args[0];
        }

        public override void Execute()
        {
            string fullPath;
            long fileSize;
            string extension;
            var answerList = new List<string>();
            try
            {
                fullPath = _fileSystem.GetFileFullPath(_filePath);
                fileSize = _fileSystem.GetFileSizeInBytes(_filePath);
                extension = _fileSystem.GetFileExtension(_filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            answerList.Add($"File path: {fullPath}\n");
            answerList.Add($"File size: {fileSize} bytes\n");

            switch (extension)
            {
                case ".txt":
                    answerList.Add("Available options: summary\n");
                    break;
                case ".csv":
                case ".json":
                    answerList.Add("Available options: print, validate\n");
                    break;
                default:
                    answerList.Add("No specific options available for this file type.\n");
                    break;
            }

            foreach (var line in answerList)
            {
                Console.Write(line);
            }
        }
    }
}
