using System.Text.RegularExpressions;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace com.erlendthune.garmin
{
    class Program
    {

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
                return;
            }

            Config.CreateDirectories();

            string command = args[0];

            switch (command)
            {
                case "get-product-list":
                    await GetProductListCommand.ExecuteAsync();
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
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
    }
}
