using System.Text.RegularExpressions;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace com.erlendthune.garmin
{
    class Program
    {
        private const string productListUrl = $"https://www.garmin.com/c/api/getProducts?categoryKey=10002&locale=en-US&storeCode=US";
        
        static async Task GetProductListAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(productListUrl);
                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the content of the response
                        string productListContent = await response.Content.ReadAsStringAsync();
                        File.WriteAllText(Config.jsonProductListFilePath, productListContent);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        static void ExtractProductDataFromHTML(string htmlFilePath, string jsonFilePath)
        {
            // Read the HTML content from the file
            string htmlContent = File.ReadAllText(htmlFilePath);

            // Extract the content of AppData.productData variable using regex
            string pattern = @"AppData\.productData\s*=\s*({.*?});";
            Match match = Regex.Match(htmlContent, pattern, RegexOptions.Singleline);

            if (match.Success)
            {
                // Get the content of the AppData.productData variable
                string productDataContent = match.Groups[1].Value;

                // Write the content to a JSON file
                File.WriteAllText(jsonFilePath, productDataContent);

                Console.WriteLine($"Content of AppData.productData variable saved to file: {Config.jsonProductsDirectory}");
            }
            else
            {
                Console.WriteLine("AppData.productData variable not found in the HTML file.");
            }
        }

        static void ExtractProductsFromHtmlFiles()
        {
            // Check if the specified directory exists
            if (!Directory.Exists(Config.htmlProductsDirectory))
            {
                Console.WriteLine($"Directory '{Config.htmlProductsDirectory}' does not exist.");
                return;
            }

            try
            {
                // Get all files in the specified directory
                string[] files = Directory.GetFiles(Config.htmlProductsDirectory);
                string currentDirectory = Directory.GetCurrentDirectory();

                // Iterate over each file and perform operations
                foreach (string htmlFilePath in files)
                {
                    Console.WriteLine($"Processing file: {htmlFilePath}");
                    string jsonFileName = Path.GetFileNameWithoutExtension(htmlFilePath);
                    string jsonFilePath = $"{currentDirectory}/{Config.jsonProductsDirectory}/{jsonFileName}.json";
                    ExtractProductDataFromHTML(htmlFilePath, jsonFilePath);
                }

                Console.WriteLine("All files processed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing files: {ex.Message}");
            }
        }


        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: dotnet run [command]");
                Console.WriteLine("Available commands:");
                Console.WriteLine("- get-product-list");
                Console.WriteLine("- get-products-html-files");
                Console.WriteLine("- get-prices");
                Console.WriteLine("- update-prices");
                Console.WriteLine("- create-table");
                Console.WriteLine("- extract-products-to-json-from-html-files");
                Console.WriteLine("- populate-database-from-json-files");
                Console.WriteLine("- generate-html");
                return;
            }

            string command = args[0];

            switch (command)
            {
                case "get-product-list":
                    await GetProductListAsync();
                    break;
                case "get-products-html-files":
                    var getGarminProductsCommand = new GetGarminProducts();
                    await getGarminProductsCommand.ExecuteAsync();
                    break;
                case "create-table":
                    var createTableCommand = new CreateProductsTableCommand();
                    createTableCommand.Execute();
                    break;
                case "extract-products-to-json-from-html-files":
                    ExtractProductsFromHtmlFiles();
                    break;
                case "get-prices":
                    var pricesCommand = new GetPrices();
                    await pricesCommand.ExecuteAsync();
                    break;
                case "update-prices":
                    var updatePricesCommand = new UpdateDatabasePricesCommand();
                    updatePricesCommand.Execute(Config.jsonPriceDirectory);
                    break;
                case "populate-database-from-json-files":
                    var populateDatabaseCommand = new PopulateDatabaseCommand();
                    populateDatabaseCommand.Execute(Config.jsonProductsDirectory);
                    break;
                case "generate-html":
                    var generateHtmlTableCommand = new HtmlTableGeneratorCommand();
                    generateHtmlTableCommand.Execute();

                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
    }
}
