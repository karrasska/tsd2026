using GoldSavings.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GoldSavings.App.Services
{
	public static class GoldDataExporter
	{
		// Task 1.3
		public static void SavePricesToXml(List<GoldPrice> prices, string filePath)
		{
			// Validation
			if (string.IsNullOrWhiteSpace(filePath))
			{
				Console.WriteLine("Error: empty filepath.");
				return;
			}

			if (prices == null || !prices.Any())
			{
				Console.WriteLine("Error: no data to save empty list).");
				return;
			}

			// Try-catch to handle potential IO exceptions
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<GoldPrice>));

				// overwite by default
				using (StreamWriter writer = new StreamWriter(filePath))
				{
					serializer.Serialize(writer, prices);
				}

				Console.WriteLine($"\nSaved {prices.Count} records to: {filePath}");
			}
			catch (UnauthorizedAccessException)
			{
				Console.WriteLine($"\nError: No acces to save in: {filePath}");
			}
			catch (DirectoryNotFoundException)
			{
				Console.WriteLine($"\nError: No existing path: {filePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"\nUnexpected error in saving XML: {ex.Message}");

			}
		}

		// Task 1.4
		public static List<GoldPrice> ReadPricesFromXml(string filePath) => (List<GoldPrice>)new XmlSerializer(typeof(List<GoldPrice>)).Deserialize(new StreamReader(filePath));
	}
}
