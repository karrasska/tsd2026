using GoldSavings.App.Model;
using GoldSavings.App.Client;
using GoldSavings.App.Services;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Investor!");

        // Step 1: Get gold prices
        GoldDataService dataService = new GoldDataService();
        DateTime startDate = DateTime.Now.AddYears(-1);
		DateTime endDate = DateTime.Now;
        List<GoldPrice> goldPrices = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();

        if (goldPrices.Count == 0)
        {
            Console.WriteLine("No data found. Exiting.");
            return;
        }

        Console.WriteLine($"Retrieved {goldPrices.Count} records. Ready for analysis.");


		// Step 2: Perform analysis
		GoldAnalysisService analysisService = new GoldAnalysisService(goldPrices);
        var avgPrice = analysisService.GetAveragePrice();

        // Step 3: Print results
        GoldResultPrinter.PrintSingleValue(Math.Round(avgPrice, 2), "Average Gold Price Last Half Year");

		#region Task2a
		// Task 2a
		// Method: TOP 3 highest i TOP 3 lowest 
		Console.WriteLine("\nTask2a");
		var top3HighestList = goldPrices
			.Where(g => g.Date >= startDate && g.Date <= endDate)
			.OrderByDescending(g => g.Price)
			.Take(3)
			.ToList();

		var top3LowestList = goldPrices
			.Where(g => g.Date >= startDate && g.Date <= endDate)
			.OrderBy(g => g.Price)
			.Take(3)
			.ToList();

		GoldResultPrinter.PrintPrices(top3HighestList, "Top 3 highest prices (method)");
		GoldResultPrinter.PrintPrices(top3LowestList, "Top 3 lowest prices (method)");

	
		// Query syntax: TOP 3 highest i TOP 3 lowest 
		var top3HighestListQuery = (from g in goldPrices
						where g.Date >= startDate && g.Date <= endDate
						orderby g.Price descending
						select g)
					   .Take(3)
					   .ToList();

		var top3LowestListQuery = (from g in goldPrices
							   where g.Date >= startDate && g.Date <= endDate
							   orderby g.Price
							   select g)
							  .Take(3)
							  .ToList();

		GoldResultPrinter.PrintPrices(top3HighestListQuery, "Top 3 highest prices (query)");
		GoldResultPrinter.PrintPrices(top3LowestListQuery, "Top 3 lowest prices (query)");

		#endregion

		#region Task2b
		// Data for task2b
		// 2020-now
		Console.WriteLine("\nTask2b");
		Console.WriteLine("Retrieving data since 2020.");
		DateTime start2b = new DateTime(2020, 1, 1);
		DateTime end2b = DateTime.Now;
		List<GoldPrice> prices2b = new List<GoldPrice>();

		DateTime currentStart2b = start2b;
		while (currentStart2b <= end2b)
		{
			DateTime currentEnd2b = currentStart2b.AddDays(365);
			if (currentEnd2b > end2b) currentEnd2b = end2b;

			var chunk = dataService.GetGoldPrices(currentStart2b, currentEnd2b).GetAwaiter().GetResult();
			if (chunk != null)
			{
				prices2b.AddRange(chunk);
			}

			currentStart2b = currentEnd2b.AddDays(1);
		}
		Console.WriteLine($"Retrieved {prices2b.Count} since 2020 till now.");
		// Only jan 2020
		var jan2020Days = prices2b
			.Where(g => g.Date.Year == 2020 && g.Date.Month == 1)
			.ToList();

		// Comparing buying date from jan2020 with every day after
		var profitableDays = jan2020Days
			.SelectMany(buyDay => prices2b
				.Where(sellDay => sellDay.Date > buyDay.Date && sellDay.Price > buyDay.Price * 1.05)
				.Select(sellDay => new
				{
					BuyDate = buyDay.Date,
					BuyPrice = buyDay.Price,
					SellDate = sellDay.Date,
					SellPrice = sellDay.Price
				})
			)
			.ToList();

		if (profitableDays.Any())
		{
			Console.WriteLine($"\nFound {profitableDays.Count} combinations in which the profit would be more than 5%.");
			Console.WriteLine("First 15 examples:");

			foreach (var p in profitableDays.Take(15))
			{
				Console.WriteLine($"Bought: {p.BuyDate:yyyy-MM-dd} ({p.BuyPrice:F2}) -> Sold: {p.SellDate:yyyy-MM-dd} ({p.SellPrice:F2})");
			}
		}
		else
		{
			Console.WriteLine("No possible days comibnation with more than 5% profit.");
		}
		#endregion

		#region Task2c
		Console.WriteLine("\nTask2c");
		Console.WriteLine("\nRetrieving data from 2019-2022");

		DateTime start2c = new DateTime(2019, 1, 1);
		DateTime end2c = new DateTime(2022, 12, 31);
		List<GoldPrice> prices2c = new List<GoldPrice>();

		DateTime currentStart2c = start2c;
		while (currentStart2c <= end2c)
		{
			DateTime currentEnd2c = currentStart2c.AddDays(92);
			if (currentEnd2c > end2c)
			{
				currentEnd2c = end2c;
			}

			var chunk = dataService.GetGoldPrices(currentStart2c, currentEnd2c).GetAwaiter().GetResult();
			if (chunk != null)
			{
				prices2c.AddRange(chunk);
			}

			currentStart2c = currentEnd2c.AddDays(1);
		}

		// Descsending order, skipping first 10, taking next 3
		var secondTenDates = prices2c
			.OrderByDescending(p => p.Price)
			.Skip(10)
			.Take(3)
			.ToList();

		Console.WriteLine($"Retrieved {prices2c.Count} from 2019-2022.");

		int rank = 11;
		GoldResultPrinter.PrintPrices(secondTenDates, "3 dates that opens the second ten of the prices ranking");
		#endregion

		#region Task2d
		Console.WriteLine("\nTask2d");
		Console.WriteLine("\nRetrieving data from 2020-2024");

		DateTime start2d = new DateTime(2020, 1, 1);
		DateTime end2d = new DateTime(2024, 12, 31);
		List<GoldPrice> prices2d = new List<GoldPrice>();

		DateTime currentStart2d = start2d;
		while (currentStart2d <= end2d)
		{
			DateTime currentEnd2d = currentStart2d.AddDays(365);
			if (currentEnd2d > end2d)
			{
				currentEnd2d = end2d;
			}

			var chunk = dataService.GetGoldPrices(currentStart2d, currentEnd2d).GetAwaiter().GetResult();
			if (chunk != null)
			{
				prices2d.AddRange(chunk);
			}

			currentStart2d = currentEnd2d.AddDays(1);
		}

		// Query syntax 
		var yearlyAverages = from p in prices2d
							 where p.Date.Year == 2020 || p.Date.Year == 2023 || p.Date.Year == 2024
							 group p by p.Date.Year into g
							 select new
							 {
								 Year = g.Key,
								 AveragePrice = g.Average(x => x.Price)
							 };

		Console.WriteLine($"Retrieved {prices2d.Count} records");
		Console.WriteLine("Avg gold prices in 2020, 2023, 2024:");

		foreach (var item in yearlyAverages)
		{
			Console.WriteLine($"Year {item.Year}: {item.AveragePrice:F2}");
		}

		#endregion

		#region Task2e
		Console.WriteLine("\nTask2e");
		// Use data from task2d
		var chronologicallySortedPrices = prices2d.OrderBy(p => p.Date).ToList();

		// Comparing every possible buy day with every possible sell day after it
		var bestInvestment = chronologicallySortedPrices
			.SelectMany((buyDay, index) => chronologicallySortedPrices.Skip(index + 1)
				.Select(sellDay => new
				{
					BuyDate = buyDay.Date,
					BuyPrice = buyDay.Price,
					SellDate = sellDay.Date,
					SellPrice = sellDay.Price,
					// ROI
					ROI = ((sellDay.Price - buyDay.Price) / buyDay.Price) * 100
				})
			)
			.OrderByDescending(x => x.ROI) 
			.FirstOrDefault(); 

		if (bestInvestment != null)
		{
			Console.WriteLine("\nBest investment between 2020 and 2024:");
			Console.WriteLine($"Buy: {bestInvestment.BuyDate:yyyy-MM-dd} (Price: {bestInvestment.BuyPrice:F2})");
			Console.WriteLine($"Sell: {bestInvestment.SellDate:yyyy-MM-dd} (Price: {bestInvestment.SellPrice:F2})");
			Console.WriteLine($"Return on Investment (ROI): {bestInvestment.ROI:F2}%");
		}
		else
		{
			Console.WriteLine("No data.");
		}
		#endregion

		#region Task3-test
		Console.WriteLine("\nTask 3 - test");
		string xmlFilePath = "gold_prices2.xml";
		GoldDataExporter.SavePricesToXml(prices2d, xmlFilePath);
		#endregion

		#region Task4-test
		Console.WriteLine("\nTask 4 - test");
		var readPrices = GoldDataExporter.ReadPricesFromXml(xmlFilePath);
		if (readPrices != null && readPrices.Count > 0)
		{
			Console.WriteLine($"\nSuccessfully read {readPrices.Count} records from XML file.");
			Console.WriteLine("Three first records");

			foreach (var p in readPrices.Take(3))
			{
				Console.WriteLine($"- Date: {p.Date:yyyy-MM-dd}, Price: {p.Price:F2}");
			}
		}
		else
		{
			Console.WriteLine("Error");
		}
		#endregion

		Console.WriteLine("\nGold Analyis Queries with LINQ Completed.");

    }
}
