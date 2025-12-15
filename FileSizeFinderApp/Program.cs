class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- High File Size Finder ---");
        Console.Write("Enter the path of the folder you want to scan (e.g., C:\\Users\\YourName): ");
        string path = Console.ReadLine();

        Console.WriteLine($"Path is: {path}");
    }
}