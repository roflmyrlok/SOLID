using SF.Core;
using SF.Domain;


namespace SF.Commands
{
    [InputAction]
    public class InfoFileInputAction : InputAction<InfoFile>
    {
        protected override string Action => "info";
        protected override string HelpString => "Info of a file";
        
        protected override string[] SupportedExtensions => ["any"];

        private readonly ICurrentUser _systemWrapper;
        private readonly ISupportedCommands _supportedCommands;

        public InfoFileInputAction(ICurrentUser systemWrapper, ISupportedCommands supportedCommands)
        {
            _systemWrapper = systemWrapper;
            _supportedCommands = supportedCommands;
        }

        protected override InfoFile GetCommandInternal(string[] args)
        {
            return new InfoFile(_systemWrapper, _supportedCommands, args);
        }
    }

    [Command]
    public class InfoFile : Command
    {
        private readonly ICurrentUser _systemWrapper;
        private readonly ISupportedCommands _supportedCommands;
        private readonly string _filePath;
    
        public InfoFile(ICurrentUser systemWrapper, ISupportedCommands supportedCommands, string[] args)
        {
            _systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));
            _supportedCommands = supportedCommands;

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
            var answerList = new List<string>();
            try
            {
                fullPath = _systemWrapper.GetFileSystem().GetFileFullPath(_filePath);
                fileSize = _systemWrapper.GetFileSystem().GetFileSizeInBytes(_filePath);
                var fileExstention = _systemWrapper.GetFileSystem().GetFileExtension(_filePath);
                var supportedCommands = _supportedCommands.GetSupportedCommands(fileExstention);
                answerList.Add("Supported Commands: + \n");
                foreach (var command in supportedCommands)
                {
                    answerList.Add(command + "\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            answerList.Add($"File path: {fullPath}\n");
            answerList.Add($"File size: {fileSize} bytes\n");

            foreach (var line in answerList)
            {
                Console.Write(line);
            }
        }
    }
}
