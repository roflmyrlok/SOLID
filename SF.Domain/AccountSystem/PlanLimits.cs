namespace SF.Domain
{
	internal class PlanLimits
	{
		public long MaxFileSizeBytes { get; set; }
		public int MaxFiles { get; set; }

		public PlanLimits(long maxFileSizeBytes, int maxFiles)
		{
			MaxFileSizeBytes = maxFileSizeBytes;
			MaxFiles = maxFiles;
		}
	}
}