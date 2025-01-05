namespace CompanyNameDuplicationChecker.Services
{
    public class CompanyNameProcessor
    {
        private readonly NormalizeNameService _normalizeName;

        public CompanyNameProcessor()
        {
            _normalizeName = new NormalizeNameService();
        }

        public List<string> ReadAndNormalizeCompanyNames(string filePath)
        {
            List<string> normalizedCompanyNames = new List<string>();

            try
            {
                // Read from the file
                List<string> companyNames = ReadAdvertisersFromFile(filePath);

                // Normalize each company name
                foreach (string companyName in companyNames)
                {
                    string normalized = _normalizeName.NormalizeName(companyName);
                    normalizedCompanyNames.Add(normalized);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading or processing the file: {ex.Message}");
            }

            return normalizedCompanyNames;
        }

        public List<(string, string)> FindPotentialDuplicates(List<string> normalizedCompanyNames)
        {
            List<(string, string)> potentialDuplicates = new List<(string, string)>();

            for (int i = 0; i < normalizedCompanyNames.Count; i++)
            {
                for (int j = i + 1; j < normalizedCompanyNames.Count; j++)
                {
                    if (IsPotentialDuplicate(normalizedCompanyNames[i], normalizedCompanyNames[j]))
                    potentialDuplicates.Add((normalizedCompanyNames[i], normalizedCompanyNames[j]));
                }
            }
            return potentialDuplicates;
        }

        private static List<string> ReadAdvertisersFromFile(string filePath)
        {
            return File.ReadAllLines(filePath).ToList();
        }


        // Check if two company names are potential duplicates based on Jaccard index
        // The threshold is adjustable, set to lower for more potential duplications
        private static bool IsPotentialDuplicate(string companyName1, string companyName2, double threshold = 0.8)
        {
            // Calculate the Jaccard Index
            double index = CalculateJaccardIndex(companyName1, companyName2);

            // Return true if index is above the threshold
            return index >= threshold;
        }

        // Calculate Jaccard similarity between two strings
        private static double CalculateJaccardIndex(string string1, string string2)
        {
            // Split the string into sets of words by whitespace
            HashSet<string> set1 = new HashSet<string>(string1.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            HashSet<string> set2 = new HashSet<string>(string2.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            // Find the intersection and union
            int intersection = set1.Intersect(set2).Count();
            int union = set1.Union(set2).Count();

            // Compute and return the Jaccard Index
            return (double)intersection / union;
        }
    }
}
