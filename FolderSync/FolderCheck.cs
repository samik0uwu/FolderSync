using System.Security.Principal;

namespace FolderSync;

public class FolderCheck
{
    public string source { get; private set; }
    public string target { get; private set; }
    public string interval { get; private set; } 
    public string log { get; set; }

    public FolderCheck(string source, string target, string interval, string log)
    {
        this.source = source;
        this.target = target;
        this.interval = interval;
        this.log = log;
    }
    //(string source, string target, string interval, string log)
    public void GetSourceFiles()
    {
        if (Directory.Exists(source))
        {
            string[] files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }
    }

    public void CompareFiles()
    {
        
    }
}