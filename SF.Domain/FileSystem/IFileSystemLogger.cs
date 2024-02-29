namespace SF.Domain;

public interface IFileSystemLogger
{
	public void PathEventCollector(IEventCollector eventCollector);
}