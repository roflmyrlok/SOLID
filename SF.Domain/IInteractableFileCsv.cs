using System.Globalization;

namespace SF.Domain;

using System.IO;

public interface IInteractableFileCsv : IInteractableFile
{
	List<string> GetCsvTable(string filePath);
	bool Validate(string filePath);
}

public class InteractableFileCsv : InteractableFile, IInteractableFileCsv
{
    public List<string> GetCsvTable(string filePath)
    {
        List<string> tableRows = new List<string>();
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                string headerRow = reader.ReadLine();
                tableRows.Add(headerRow);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    tableRows.Add(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CSV file: {ex.Message}");
        }
        return tableRows;
    }

    public bool Validate(string filePath)
    {
        try
        {
            if (!File.Exists(filePath) || !Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                return false;
            using (var reader = new StreamReader(filePath))
            {
                if (reader.EndOfStream)
                    return false;
                string headerLine = reader.ReadLine();
                if (string.IsNullOrEmpty(headerLine))
                    return false;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Split(',').Length != headerLine.Split(',').Length)
                        return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error validating CSV file: {ex.Message}");
            return false;
        }
    }
}

