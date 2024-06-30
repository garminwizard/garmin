using System.Data.SQLite;
using Newtonsoft.Json;
using System.Globalization;

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

            // Deserialize the JSON content
            ProductPrice? productPrice = JsonConvert.DeserializeObject<ProductPrice>(jsonContent);

            if (productPrice is null || productPrice.PricedSkus is null)
            {
                Console.WriteLine($"Deserialization of {jsonPricePath} gave null value.");
                return;
            }

            double price = 0.0;
            var priceFound = false;
            foreach (var sku in productPrice.PricedSkus)
            {
                if(sku is not null && sku.ListPrice != null)
                {
                    if(!priceFound)
                    {
                        priceFound = true;
                        price = (double)sku.ListPrice.Price;
                    } else {
                        price = Math.Min((double)sku.ListPrice.Price, price);
                    }
                }
            }

            var id = productPrice.Pid;

            var formattedPrice = price.ToString("F2", CultureInfo.InvariantCulture);
            var updateSql = $"UPDATE products SET price = '{formattedPrice}' WHERE productId = '{id}';";
            Console.WriteLine(updateSql);
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