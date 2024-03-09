using System.Text.RegularExpressions;

namespace com.erlendthune.garmin;

public class ExtractProductsFromHtmlFilesCommand
{
    public static void Execute()
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

            Console.WriteLine($"Content of {htmlFilePath} saved to file: {Config.jsonProductsDirectory}");
        }
        else
        {
            Console.WriteLine("AppData.productData variable not found in the HTML file.");
        }
    }

}