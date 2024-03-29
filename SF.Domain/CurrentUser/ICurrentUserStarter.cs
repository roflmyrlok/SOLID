namespace SF.Domain;

public interface ICurrentUserStarter
{
	void PathCurrentUser(IEventCollector eventCollector, Dictionary<string, FileSystem> userFileSystem);

}