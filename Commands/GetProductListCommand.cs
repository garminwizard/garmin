class GetProductListCommand
{
    public static async Task ExecuteAsync()
    {
        using (var client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync(Config.productListUrl);

                if (response.IsSuccessStatusCode)
                {
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
}