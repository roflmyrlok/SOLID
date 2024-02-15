using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;

namespace SF.Domain;

public interface IInteractableFileTxt : IInteractableFile
{
	List<string> GetFileSummary(string filePath);
}

public class InteractableFileTxt
{
	public List<string> GetFileSummary(string filePath)
	{
		var ans = new List<string>();
		string text = File.ReadAllText(filePath);

		int symbolCount = text.Length;

		string[] words = text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		int wordCount = words.Length;

		string[] paragraphs = Regex.Split(text, @"\r?\n\s*\r?\n");
		int paragraphCount = paragraphs.Length;

		ans.Add($"Number of Symbols: {symbolCount}");
		ans.Add($"Number of Words: {wordCount}");
		ans.Add($"Number of Paragraphs: {paragraphCount}");
		return ans;
	}
}