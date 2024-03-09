//https://www.garmin.com/compare/api/pricing/getProductPrices?locale=nb-NO&app=com.garmin.www-compare&cacheKey=none&customerId=none&productIds=852159&productIds=852183&productIds=852217

using Newtonsoft.Json;

public class GetPricesCommand
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
                    // Check if the productIds list contains elements
                    if (product.ProductIds != null && product.ProductIds.Count > 0)
                    {
                        // Iterate over each product ID
                        foreach (var productId in product.ProductIds)
                        {
                            // Construct the comparison URL
                            string priceUrl = $"https://www.garmin.com/c/api/getProductsPrice?productIds={productId}&countryCode=US&storeCode=US&locale=en-US&categoryKey=10002&appName=www-category-pages&cg=none";

                            var response = await client.GetAsync(priceUrl);
                            // Check if the response is successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the content of the response
                                string productContent = await response.Content.ReadAsStringAsync();

                                // Save the resulting content to a HTML file named after the product ID
                                string fileName = $"{Config.jsonPriceDirectory}/{productId}.json";
                                File.WriteAllText(fileName, productContent);

                                Console.WriteLine($"Price for product ID {productId} saved to file: {fileName}");

                            }
                            else
                            {
                                Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                            }
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
