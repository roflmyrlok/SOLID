using SF.Core;
using SF.Domain;


namespace SF.Commands
{
    [InputAction]
    public class InfoFileInputAction : InputAction<InfoFile>
    {
        protected override string Action => "InfoFile";
        protected override string HelpString => "InfoFile of a file";

        private readonly SystemWrapper _systemWrapper;

        public InfoFileInputAction(SystemWrapper systemWrapper)
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
        private readonly SystemWrapper _systemWrapper;
        private readonly string _filePath;

        public InfoFile(SystemWrapper systemWrapper, string[] args)
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
