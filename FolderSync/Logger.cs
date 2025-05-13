using System.Runtime.CompilerServices;

namespace FolderSync;

public static class Logger
{
    public static string logPath { get; set; }

    public static void Write(string message)
    {
        string line = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}: {message}";
        Console.WriteLine(line);
        File.AppendAllText(logPath, line + Environment.NewLine);
    }

    public static void Init(string path)
    {
        string logFolder = Path.GetDirectoryName(path);
        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
            Console.WriteLine("Created log folder(s)");
        }

        if (!File.Exists(path))
        {
            File.Create(path).Close();
            Console.WriteLine("Created log file");
        }
        
        logPath = path;
    }
}