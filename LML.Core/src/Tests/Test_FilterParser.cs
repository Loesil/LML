using LML.Core.Models;

namespace LML.Core.Filters
{
    public static class Test_FilterParser
    {
        public static void DemonstrateFilters()
        {
            // Example filters:
            var filters = new[]
            {
                // Filter by genre
                "genres <= \"Hardstyle\"",
                
                // Filter by tags and check if file exists
                "tags <= \"classic\" && exists",
                
                // Complex filter with multiple conditions
                "(genres <= \"Hardstyle\" && tags <= \"classic\" && (!local || exists))",
                
                // Filter by artist
                "artists <= \"Die Ärzte\"",
                
                // Filter by album
                "album == \"Unknown Album\"",
                
                // Filter by multiple conditions
                "genres <= \"Rock\" && artists <= \"Gorillaz\" && exists && local"
            };

            Console.WriteLine("Filter Examples:");
            foreach (var filterString in filters)
            {
                try
                {
                    var filter = FilterParser.ParseFilter(filterString);
                    Console.WriteLine($"\nFilter: {filterString}");
                    Console.WriteLine("Successfully parsed!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nFilter: {filterString}");
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public static void TestFilter(string filterString, MediaFile file)
        {
            try
            {
                var filter = FilterParser.ParseFilter(filterString);
                var result = filter.Apply(file);
                Console.WriteLine($"\nFilter: {filterString}");
                Console.WriteLine($"Result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFilter: {filterString}");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}