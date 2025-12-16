using System.Collections;

class Program
{
    static void Main(string[] args)
    {
        bool running = true;

        while (running)
            {
        
            // Menu screen
            Console.Clear();
            Console.WriteLine("--- High File Size Finder ---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Find 10 largest files");
            Console.WriteLine("2) List all files and their size, grouped by size");
            Console.WriteLine("3) Find up to 20 duplicate files");
            Console.WriteLine("4) Exit");
            Console.Write("\r\nSelect an option: ");

            string choice = Console.ReadLine();
        
            switch (choice)
            {
                case "1":
                    TenLargest();
                    break;
                case "2":
                    ListAll();
                    break;
                case "3":
                    FindDuplicates();
                    break;
                case "4":
                    Console.WriteLine("Exiting");
                    return;
                default:
                    break;
            } 
            // Prompt user if they want to go back to main menu
            Console.WriteLine("1) Back to main menu");
            switch (Console.ReadLine())
            {
                case "1":
                    break;
                default:
                    break;
            }
        }
        Console.WriteLine("Program has ended");
    }

    static void TenLargest()
    {
        // Input file path
        Console.WriteLine("--- Ten Largest ---");
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

        // Return the total number of bytes in all the files under the specified folder.
        long totalBytesSize = fileLengths.Sum();

        // Translate to MB
        double totalBytesSizeMB = Math.Round(totalBytesSize / (1024.0 * 1024.0), 2);

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

    static void ListAll() {
        // Input file path
        Console.WriteLine("--- List All ---");
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

        // Return the total number of bytes in all the files under the specified folder.
        long totalBytesSize = fileLengths.Sum();

        // Translate to MB
        double totalBytesSizeMB = Math.Round(totalBytesSize / (1024.0 * 1024.0), 2);

        // Group the files according to their size, leaving out
        // files that are less than 200000 bytes.
        var querySizeGroups = from file in fileList
            let fileInfo = new FileInfo(file)
            let len = fileInfo.Length
            where len > 0
            group fileInfo by (len / 100000) into fileGroup
            where fileGroup.Key >= 2
            orderby fileGroup.Key descending
            select fileGroup;
            
        // Print results
        Console.WriteLine($"There are {fileList.Count()} files in the path with a total size of {totalBytesSizeMB} MB.");
        Console.WriteLine($"The 10 largest files are:");
        foreach (var filegroup in querySizeGroups)
        {
            Console.WriteLine($"{Math.Round(filegroup.Key * 100000 / (1024.0 * 1024.0), 2)} MB");
            foreach (var item in filegroup)
            {
                Console.WriteLine($"\t{item.Name}: {Math.Round(item.Length / (1024.0 * 1024.0), 2)} MB");
            }
        }
        Console.WriteLine($"---------");
    }

    static void FindDuplicates()
    {
        // Input file path
        Console.WriteLine("--- Find Duplicate Files ---");
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

        DirectoryInfo dir = new DirectoryInfo(path);

        IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

        // Check each file to see if they are exact duplicates
        int charsToSkip = path.Length;
        var queryDupNames = from file in fileList
            group file.FullName.Substring(charsToSkip) by file.Name into fileGroup
            where fileGroup.Count() > 1
            select fileGroup;

        // Print results
        foreach (var queryDup in queryDupNames.Take(20))
        {
            Console.WriteLine($"Filename = {(queryDup.Key.ToString() == string.Empty ? "[none]" : queryDup.Key.ToString())}");

            foreach (var fileName in queryDup.Take(10))
            {
                Console.WriteLine($"\t{fileName}");
            }   
        }
        Console.WriteLine($"---------");
    }
}