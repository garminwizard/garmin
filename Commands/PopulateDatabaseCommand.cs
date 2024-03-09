using System.Data.SQLite;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;

public class PopulateDatabaseCommand
{
    private static string connectionString = $"Data Source={Config.databaseFilePath};Version=3;";

    public static void Execute()
    {
        // Check if the specified directory exists
        if (!Directory.Exists(Config.jsonProductsDirectory))
        {
            Console.WriteLine($"Directory '{Config.jsonProductsDirectory}' does not exist.");
            return;
        }

        try
        {
            // Get all files in the specified directory
            string[] files = Directory.GetFiles(Config.jsonProductsDirectory);

            // Iterate over each file and perform operations
            using (var connection = new SQLiteConnection(connectionString))
            {
                // Open connection
                connection.Open();

                foreach (string filePath in files)
                {
                    Console.WriteLine($"Processing file: {filePath}");
                    InsertProductsIntoTable(connection, filePath);
                }
            }

            Console.WriteLine("All files processed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while processing files: {ex.Message}");
        }
    }

    private static void InsertProductsIntoTable(SQLiteConnection connection, string jsonProductPath)
    {
        try {
            // Read the JSON content from the file
            string jsonContent = System.IO.File.ReadAllText(jsonProductPath);

            // Deserialize the JSON content
            ProductRootObject? jsonObject = JsonConvert.DeserializeObject<ProductRootObject>(jsonContent);

            if(jsonObject is null)
            {
                Console.WriteLine($"Deserialization of {jsonProductPath} gave null value.");
                return;
            }

            // Extract products data
            var products = jsonObject.products;

            if(products is null)
            {
                Console.WriteLine($"{jsonProductPath} did not contain any products.");
                return;
            }

            if(jsonObject.productSpecs is null)
            {
                Console.WriteLine($"{jsonProductPath} did not contain any product specifications.");
                return;
            }

            var productSpecGroups = jsonObject.productSpecs.specGroups;
            if(productSpecGroups is null)
            {
                Console.WriteLine($"{jsonProductPath} did not contain any product specification groups.");
                return;
            }

            // Insert data into table
            string productId = products[0].pid.ToString();
            string displayName = products[0].displayName.ToString();
            string productUrl = products[0].productUrl.ToString();

            foreach (var specGroup in productSpecGroups)
            {
                string specGroupKeyDisplayName = specGroup.specGroupKeyDisplayName.ToString();

                foreach (var spec in specGroup.specs)
                {
                    string specKey = spec.specKey.ToString();
                    string specDisplayName = spec.specDisplayName.ToString();

                    foreach (var value in spec.values)
                    {
                        string specDisplayValue = value.specDisplayValue.ToString();
                        var specValue = StripHtml(specDisplayValue);

                        string insertSql = @"
                            INSERT INTO Products (productId, displayName, productUrl, specGroupKeyDisplayName, specKey, specValue, specDisplayName, specDisplayValue)
                            VALUES (@productId, @displayName, @productUrl, @specGroupKeyDisplayName, @specKey, @specValue, @specDisplayName, @specDisplayValue)";
                        using (var insertCommand = new SQLiteCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@productId", productId);
                            insertCommand.Parameters.AddWithValue("@displayName", displayName);
                            insertCommand.Parameters.AddWithValue("@productUrl", productUrl);
                            insertCommand.Parameters.AddWithValue("@specGroupKeyDisplayName", specGroupKeyDisplayName);
                            insertCommand.Parameters.AddWithValue("@specKey", specKey);
                            insertCommand.Parameters.AddWithValue("@specValue", specValue);
                            insertCommand.Parameters.AddWithValue("@specDisplayName", specDisplayName);
                            insertCommand.Parameters.AddWithValue("@specDisplayValue", specDisplayValue);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static string StripHtml(string input)
    {
       // Remove HTML tags
        string noHtml = Regex.Replace(input, "<.*?>", string.Empty);

        // Decode HTML entities
        string decodedString = WebUtility.HtmlDecode(noHtml);

        // Remove non-alphanumeric characters and make lowercase
        string cleanString = Regex.Replace(decodedString, "[^a-zA-Z0-9]", "").ToLower();

        return cleanString;
    }
}