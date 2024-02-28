using System;
using System.IO;
using SF.Domain.ExternalInterfaces;

namespace SF.Domain.Actions
{
	[ActionStrategie]
	public class CsvValidationFileActionStrategy : IFileActionStrategy<bool>
	{
		public bool Execute(string filePath)
		{
			return Validate(filePath);
		}

		private bool Validate(string filePath)
		{
			try
			{
				if (!File.Exists(filePath) ||
				    !Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase))
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
}