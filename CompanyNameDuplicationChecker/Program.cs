using CompanyNameDuplicationChecker.Services;

namespace SensorTower.CompanyNameDuplicationChecker
{
    public class Program
    {
        public static void Main()
        {
            /*
             * Input: A list of company names
             * Output: Identify duplicates
             * Concerns: Case sensitivity, extra spaces, minor variations
             */

            // 1. Parse the input.
            // 2. Run the processor to normalize the names
            // 3. Check for duplicates and store the result.
            // 4. Display the result.

            // Get the current directory of the executable.
            // Reason being the working directory might be set to the output directory which is like \\bin\\Debug\\net6.0
            string? currentDirectory = Directory.GetCurrentDirectory();

            // Navigate up until we find the parent directory containing the .csproj file
            while (!File.Exists(Path.Combine(currentDirectory, "CompanyNameDuplicationChecker.csproj")))
            {
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;

                // If there is no .csproj file, break the loop to avoid an infinite loop
                if (currentDirectory == null)
                {
                    Console.WriteLine("Error: Could not find the project directory.");
                    return;
                }
            };

            string path = Path.Combine(currentDirectory, "Input", "advertisers.txt");

            CompanyNameProcessor companyNameProcessor = new CompanyNameProcessor();
            List<string> normalizedCompanyNames = companyNameProcessor.ReadAndNormalizeCompanyNames(path);
            List<(string, string)> potentialDuplicates = companyNameProcessor.FindPotentialDuplicates(normalizedCompanyNames);

            Console.WriteLine("Potential Duplicates:");
            foreach (var (advertiserNameOne, advertiserNameTwo) in potentialDuplicates)
            {
                Console.WriteLine($"{advertiserNameOne} <-> {advertiserNameTwo}");
            }
        }
    }
}