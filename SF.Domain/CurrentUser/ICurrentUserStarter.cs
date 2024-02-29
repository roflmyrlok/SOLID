namespace SF.Domain;

public interface ICurrentUserStarter
{
	void PathCurrentUser(IEventCollector eventCollector, IAccountStorage accountStorage, Dictionary<string, FileSystem> userFileSystem);

}