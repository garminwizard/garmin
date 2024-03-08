using System.Data.SQLite;
using System.Text;

public class Specification
{
    public string? SpecGroupKeyDisplayName { get; set; }
    public string? SpecKey { get; set; }
    public string? SpecDisplayName { get; set; }
}

public class HtmlTableGeneratorCommand
{
    private string connectionString;

    public HtmlTableGeneratorCommand()
    {
        connectionString = $"Data Source={Config.databaseFilePath};Version=3;";
    }

    public void Execute()
    {
        string htmlTables = ConvertDatabaseToHtmlTable();
        string filePath = "index.html";
        File.WriteAllText(filePath, htmlTables);
        Console.WriteLine($"HTML tables saved to file: {filePath}");            
    }

    private string ConvertDatabaseToHtmlTable()
    {
        StringBuilder html = new StringBuilder();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Query to select distinct specifications from the database
            string query = "SELECT DISTINCT SpecGroupKeyDisplayName, SpecKey, SpecDisplayName FROM Products";

            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                // Dictionary to hold grouped specifications
                Dictionary<string, List<Specification>> groupedSpecs = new Dictionary<string, List<Specification>>();

                // Read specifications from the database
                while (reader.Read())
                {
                    string specGroupKeyDisplayName = reader.GetString(0);
                    string specKey = reader.GetString(1);
                    string specDisplayName = reader.GetString(2);

                    Specification spec = new Specification
                    {
                        SpecGroupKeyDisplayName = specGroupKeyDisplayName,
                        SpecKey = specKey,
                        SpecDisplayName = specDisplayName
                    };

                    // Ensure that the list exists for the group
                    if (!groupedSpecs.ContainsKey(specGroupKeyDisplayName))
                    {
                        groupedSpecs[specGroupKeyDisplayName] = new List<Specification>();
                    }

                    // Add the specification to the list
                    groupedSpecs[specGroupKeyDisplayName].Add(spec);
                }

                // Generate HTML tables for each group of specifications
                foreach (var group in groupedSpecs)
                {
                    html.AppendLine($"<h2>{group.Key}</h2>");

                    html.AppendLine("<table>");
                    html.AppendLine("<tr><th>Spec Key</th><th>Spec Display Name</th></tr>");

                    foreach (var spec in group.Value)
                    {
                        html.AppendLine($"<tr><td>{spec.SpecKey}</td><td>{spec.SpecDisplayName}</td></tr>");
                    }

                    html.AppendLine("</table>");
                }
            }
        }

        return html.ToString();
    }
}
