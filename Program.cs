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
                    await GetGarminProductsCommand.ExecuteAsync();
                    break;
                case "create-table":
                    CreateProductsTableCommand.Execute();
                    break;
                case "extract-products-to-json-from-html-files":
                    ExtractProductsFromHtmlFilesCommand.Execute();
                    break;
                case "get-prices":
                    await GetPricesCommand.ExecuteAsync();
                    break;
                case "update-prices":
                    UpdateDatabasePricesCommand.Execute();
                    break;
                case "populate-database-from-json-files":
                    PopulateDatabaseCommand.Execute();
                    break;
                case "generate-html":
                    HtmlTableGeneratorCommand.Execute();
                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
    }
}
