using System.Data.SQLite;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;

public class UpdateDatabasePricesCommand
{
    private static string connectionString = $"Data Source={Config.databaseFilePath};Version=3;";

    public static void Execute()
    {
        // Check if the specified directory exists
        if (!Directory.Exists(Config.jsonPriceDirectory))
        {
            Console.WriteLine($"Directory '{Config.jsonPriceDirectory}' does not exist.");
            return;
        }

        try
        {
            // Get all files in the specified directory
            string[] files = Directory.GetFiles(Config.jsonPriceDirectory);

            // Iterate over each file and perform operations
            using (var connection = new SQLiteConnection(connectionString))
            {
                // Open connection
                connection.Open();

                foreach (string filePath in files)
                {
                    Console.WriteLine($"Processing file: {filePath}");
                    UpdatePrice(connection, filePath);
                }
            }

            Console.WriteLine("All files processed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while processing files: {ex.Message}");
        }
    }

    private static void UpdatePrice(SQLiteConnection connection, string jsonPricePath)
    {
        try
        {
            // Read the JSON content from the file
            string jsonContent = File.ReadAllText(jsonPricePath);
            Console.WriteLine(jsonContent);

            // Deserialize the JSON content
            List<ProductPrice>? productPrices = JsonConvert.DeserializeObject<List<ProductPrice>>(jsonContent);

            if (productPrices is null)
            {
                Console.WriteLine($"Deserialization of {jsonPricePath} gave null value.");
                return;
            }

            // Extract price data
            var priceObject = productPrices[0];

            if (priceObject.unformatted == null)
            {
                Console.WriteLine($"{jsonPricePath} did not contain any unformatted price.");
                return;
            }

            var price = priceObject.unformatted.listPrice;
            var id = priceObject.id;

            if (price == null || id == null)
            {
                Console.WriteLine($"{jsonPricePath} did not contain any price.");
                return;
            }
            var updateSql = $"UPDATE products SET price = '{price}' WHERE productId = '{id}';";
            using (var updateCommand = new SQLiteCommand(updateSql, connection))
            {
                updateCommand.ExecuteNonQuery(); ;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}