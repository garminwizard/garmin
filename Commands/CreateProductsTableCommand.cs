using System.Data.SQLite;

public class CreateProductsTableCommand
{
    private static string connectionString = $"Data Source={Config.databaseFilePath};Version=3;";

    public static void Execute()
    {
            // Create SQLite database connection
        using (var connection = new SQLiteConnection(connectionString))
        {
            // Open connection
            connection.Open();

            // Create table
            string createTableSql = @"
                CREATE TABLE IF NOT EXISTS Products (
                    productId TEXT,
                    price REAL,
                    displayName TEXT,
                    productUrl TEXT,
                    specGroupKeyDisplayName TEXT,
                    specKey TEXT,
                    specValue TEXT,
                    specDisplayName TEXT,
                    specDisplayValue TEXT
                );
                
                CREATE INDEX IF NOT EXISTS idx_specKey_specValue ON products (specKey, specValue);
                ";


            using (var command = new SQLiteCommand(createTableSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
