namespace FolderSync;

public static class Logger
{
    public static string logPath { get; private set; }

    public static void Write(string message)
    {
        string line = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}: {message}";
        Console.WriteLine(line);
        File.AppendAllText(logPath, line + Environment.NewLine);
    }
}