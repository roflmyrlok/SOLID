namespace SF.Domain
{
	public class User
	{
		public string AccountName { get; set; }
		public Plan Plan { get; set; }
		public ICollection<LocalFile> Files { get; set; } = new List<LocalFile>();
	}
}