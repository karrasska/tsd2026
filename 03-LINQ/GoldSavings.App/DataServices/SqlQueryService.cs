using GoldSavings.App.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;


namespace GoldSavings.App.Services
{
	public static class SqlQueryService
	{
		public static (List<GoldPrice> top3Highest, List<GoldPrice> top3Lowest) QueryTop3HighestAndLowest(
			List<GoldPrice> goldPrices, DateTime startDate, DateTime endDate)
		{
			if (goldPrices == null) throw new ArgumentNullException(nameof(goldPrices));

			var topHigh = new List<GoldPrice>();
			var topLow = new List<GoldPrice>();

			using var connection = new SqliteConnection("Data Source=:memory:");
			connection.Open();

			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = "CREATE TABLE GoldPrices (Date TEXT NOT NULL, Price REAL NOT NULL);";
				cmd.ExecuteNonQuery();
			}

			using (var tran = connection.BeginTransaction())
			using (var ins = connection.CreateCommand())
			{
				ins.CommandText = "INSERT INTO GoldPrices(Date, Price) VALUES(@d, @p);";
				var pDate = ins.CreateParameter(); pDate.ParameterName = "@d"; ins.Parameters.Add(pDate);
				var pPrice = ins.CreateParameter(); pPrice.ParameterName = "@p"; ins.Parameters.Add(pPrice);

				foreach (var g in goldPrices)
				{
					pDate.Value = g.Date.ToString("s", CultureInfo.InvariantCulture);
					pPrice.Value = g.Price;
					ins.ExecuteNonQuery();
				}
				tran.Commit();
			}

			// TOP 3 highest
			using (var q = connection.CreateCommand())
			{
				q.CommandText = "SELECT Date, Price FROM GoldPrices WHERE Date BETWEEN @start AND @end ORDER BY Price DESC LIMIT 3;";
				q.Parameters.AddWithValue("@start", startDate.ToString("s", CultureInfo.InvariantCulture));
				q.Parameters.AddWithValue("@end", endDate.ToString("s", CultureInfo.InvariantCulture));
				using var r = q.ExecuteReader();
				while (r.Read())
				{
					var d = DateTime.Parse(r.GetString(0), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
					topHigh.Add(new GoldPrice { Date = d, Price = r.GetDouble(1) });
				}
			}

			// TOP 3 lowest
			using (var q = connection.CreateCommand())
			{
				q.CommandText = "SELECT Date, Price FROM GoldPrices WHERE Date BETWEEN @start AND @end ORDER BY Price ASC LIMIT 3;";
				q.Parameters.AddWithValue("@start", startDate.ToString("s", CultureInfo.InvariantCulture));
				q.Parameters.AddWithValue("@end", endDate.ToString("s", CultureInfo.InvariantCulture));
				using var r = q.ExecuteReader();
				while (r.Read())
				{
					var d = DateTime.Parse(r.GetString(0), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
					topLow.Add(new GoldPrice { Date = d, Price = r.GetDouble(1) });
				}
			}

			connection.Close();
			return (topHigh, topLow);
		}
	}
}