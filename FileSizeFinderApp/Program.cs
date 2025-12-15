class Program
{
    static void Main(string[] args)
    {
        // Input file path
        Console.WriteLine("--- High File Size Finder ---");
        Console.Write("Enter the path of the folder you want to scan (e.g., C:\\Users\\YourName): ");
        string path = Console.ReadLine();

        // Check that file path exists
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            Console.WriteLine("Invalid or non-existent directory path provided.");
            return;
        }

        // Confirm file path
        Console.WriteLine($"---------");
        Console.WriteLine($"Path is: {path}");

        // Get the files in the folder path
        var fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        var fileQuery = from file in fileList
                        let fileLen = new FileInfo(file).Length
                        where fileLen > 0
                        select fileLen;

        // Cache the results to avoid multiple trips to the file system.
        long[] fileLengths = fileQuery.ToArray();

        // Return the size of the largest file
        long largestFileSize = fileLengths.Max();

        // Return the total number of bytes in all the files under the specified folder.
        long totalBytesSize = fileLengths.Sum();

        // Translate to MB
        double totalBytesSizeMB = Math.Round(totalBytesSize / (1024.0 * 1024.0), 2);
        double largestFileSizeMB = Math.Round(largestFileSize / (1024.0 * 1024.0), 2);

        // Return the details for the 10 largest files
        var queryTenLargest = (from file in fileList
            let fileInfo = new FileInfo(file)
            let len = fileInfo.Length
            orderby len descending
            select fileInfo
            ).Take(10);
            
        // Print results
        Console.WriteLine($"There are {fileList.Count()} files in the path with a total size of {totalBytesSizeMB} MB.");
        Console.WriteLine($"The 10 largest files are:");
        foreach (var v in queryTenLargest) // List the name and size (converted to MB) of the 10 largest files
        {
            Console.WriteLine($"{v.FullName}: { Math.Round(v.Length / (1024.0 * 1024.0), 2)} MB");
        }
        Console.WriteLine($"---------");
        
    }
}