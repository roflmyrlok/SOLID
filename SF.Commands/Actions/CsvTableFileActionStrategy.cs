using System;
using System.Collections.Generic;
using System.IO;

namespace SF.Domain.Actions
{
	public class CsvTableFileActionStrategy : IFileActionStrategy<string>
	{
		public string Execute(string filePath)
		{
			return GetCsvTable(filePath);
		}

		private string GetCsvTable(string filePath)
		{
			var tableRows = "";
			try
			{
				using (var reader = new StreamReader(filePath))
				{
					string headerRow = reader.ReadLine();
					tableRows += (headerRow + "\n");

					string line;
					while ((line = reader.ReadLine()) != null)
					{
						tableRows += (line + "\n");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error reading CSV file: {ex.Message}");
			}

			return tableRows;
		}
	}
}