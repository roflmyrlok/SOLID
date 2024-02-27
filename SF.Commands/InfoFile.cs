using SF.Core;
using SF.Domain;


namespace SF.Commands
{
    [InputAction]
    public class InfoFileInputAction : InputAction<InfoFile>
    {
        protected override string Action => "info";
        protected override string HelpString => "Infoof a file";
        
        protected override string[] SupportedExtensions => ["any"];

        private readonly ISystemWrapper _systemWrapper;

        public InfoFileInputAction(ISystemWrapper systemWrapper)
        {
            _systemWrapper = systemWrapper;
        }

        protected override InfoFile GetCommandInternal(string[] args)
        {
            return new InfoFile(_systemWrapper, args);
        }
    }

    [Command]
    public class InfoFile : Command
    {
        private readonly ISystemWrapper _systemWrapper;
        private readonly string _filePath;
    
        public InfoFile(ISystemWrapper systemWrapper, string[] args)
        {
            _systemWrapper = systemWrapper ?? throw new ArgumentNullException(nameof(systemWrapper));

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
                fullPath = _systemWrapper.GetFileFullPath(_filePath);
                fileSize = _systemWrapper.GetFileSizeInBytes(_filePath);
                var fileExstention = _systemWrapper.GetFileExtension(_filePath);
                var supportedCommands = _systemWrapper.GetSupportedCommands(fileExstention);
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
