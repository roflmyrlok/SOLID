using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SF.Domain.Actions
{
	public class SummaryFileActionStrategy : IFileActionStrategy<List<string>>
	{
		public List<string> Execute(string filePath)
		{
			List<string> summary = new List<string>();
			
			var fileLines = File.ReadAllLines(filePath);
			var symbolCount = fileLines.Sum(line => line.Length);
			var wordCount = fileLines.SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)).Count();
			var paragraphCount = fileLines.Count(line => string.IsNullOrWhiteSpace(line));

			summary.Add($"Number of Symbols: {symbolCount}");
			summary.Add($"Number of Words: {wordCount}");
			summary.Add($"Number of Paragraphs: {paragraphCount}");

			return summary;
		}
	}
}