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
            // 1. Calculate the Jaccard Index
            double index = CalculateJaccardIndex(companyName1, companyName2);

            // 2. Return true if the index value is above the threshold
            return index >= threshold;
        }

        // Calculate Jaccard similarity between two strings
        // This code snippet is from Googling and StackOverFlow, adjusted to be suitable for the solution
        // https://stackoverflow.com/questions/9453731/how-to-calculate-distance-similarity-measure-of-given-2-strings
        // https://medium.com/@mayurdhvajsinhjadeja/jaccard-similarity-34e2c15fb524

        private static double CalculateJaccardIndex(string string1, string string2)
        {
            // 1. Split the string into sets of words by whitespace
            HashSet<string> set1 = new HashSet<string>(string1.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            HashSet<string> set2 = new HashSet<string>(string2.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            // 2. Find the intersection and union values
            int intersection = set1.Intersect(set2).Count();
            int union = set1.Union(set2).Count();

            // 3. Compute and return the Jaccard Index
            return (double)intersection / union;
        }
    }
}
