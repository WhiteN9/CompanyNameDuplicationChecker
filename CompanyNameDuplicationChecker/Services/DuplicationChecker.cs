using System.Text.RegularExpressions;

namespace CompanyNameDuplicationChecker.Services
{
    public class NormalizeNameService
    {
        public string NormalizeName(string companyName)
        {
            // 1: Convert to lowercase
            companyName = companyName.ToLowerInvariant();

            // 2: Standardize punctuation (ensure commas are followed by a space)
            companyName = Regex.Replace(companyName, @"\s*,\s*", ", "); // Ensure proper spacing after commas

            // 3: Standardize whitespace (convert multiple spaces to a single space)
            companyName = Regex.Replace(companyName, @"\s+", " "); // Replace multiple spaces with a single one

            // 4: Standardize legal control terms (convert "Co." to "Company", "Inc." to "Incorporated", etc.)
            companyName = companyName.Replace(" co.", " company")
                                     .Replace(" inc.", " incorporated")
                                     .Replace(" llc", " limited liability company")
                                     .Replace(" ltd.", " limited")
                                     .Replace(" corp.", " corporation");

            // 5: Remove special characters
            companyName = Regex.Replace(companyName, @"[^a-zA-Z0-9\s]", "");

            // 6: Remove excess whitespaces
            companyName = Regex.Replace(companyName.Trim(), @"\s+", " ");

            return companyName;
        }
    }
}