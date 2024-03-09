
class Config 
{
    public static string productListUrl = "https://www.garmin.com/c/api/getProducts?categoryKey=10002&locale=en-US&storeCode=US";
    public static string jsonProductListFilePath = "garminproductlist.json";
    public static string htmlProductsDirectory = "htmlproducts";
    public static string jsonProductsDirectory = "jsonproducts";
    public static string jsonPriceDirectory = "jsonprices";
    public static string databaseFilePath = "products.db";

    public static void CreateDirectories() 
    {
        CreateDirectory(htmlProductsDirectory);
        CreateDirectory(jsonProductsDirectory);
        CreateDirectory(jsonPriceDirectory);
    }

    public static void CreateDirectory(string directoryPath) 
    {
       if (!Directory.Exists(directoryPath))
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Directory {directoryPath} created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directory: {ex.Message}");
            }
        }
    }
}
