using Newtonsoft.Json;

public class GetGarminProductsCommand
{
    public static async Task ExecuteAsync() 
    {
        try
        {
            // Read the JSON content from the file
            string jsonContent = File.ReadAllText(Config.jsonProductListFilePath);

            // Deserialize JSON into root object
            RootObject? root = JsonConvert.DeserializeObject<RootObject>(jsonContent);

            if(root is null)
            {
                Console.WriteLine($"Deserialization of {Config.jsonProductListFilePath} gave null value.");
                return;
            }

            if(root.Products is null)
            {
                Console.WriteLine($"{Config.jsonProductListFilePath} did not contain any products.");
                return;
            }

            using (var client = new HttpClient())
            {
                // Iterate over each product
                foreach (var product in root.Products)
                {
                    if (product.Group == false)
                    {
                        string compareUrl = $"https://www.garmin.com/en-US/compare/?compareProduct={product.Id}";

                        var response = await client.GetAsync(compareUrl);
                        // Check if the response is successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the content of the response
                            string productContent = await response.Content.ReadAsStringAsync();

                            // Save the resulting content to a HTML file named after the product ID
                            string fileName = $"{Config.htmlProductsDirectory}/{product.Id}.html";
                            File.WriteAllText(fileName, productContent);

                            Console.WriteLine($"Content for product ID {product.Id} saved to file: {fileName}");

                        }
                        else
                        {
                            Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }            
    }
}