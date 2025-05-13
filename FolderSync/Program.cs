
using FolderSync;

if (args.Length != 4)
{
    Console.WriteLine("Application needs exactly 4 arguments: [source folder] [target folder] [interval (ms)] [log file path]");
    Console.WriteLine("Example: FolderSync.exe C:\\source C:\\target 300000 C:\\log.txt");
    return;
}

string sourceFolder = args[0];
string targetFolder = args[1];
string interval = args[2];
string logPath = args[3]; 
if (!Directory.Exists(sourceFolder)) //source folder has to exist, target folder can be created
{
    Console.WriteLine("Source folder does not exist!");
    return;
}

if (!Directory.Exists(targetFolder))
{
    Directory.CreateDirectory(targetFolder);
}

Logger.Init(logPath);

int intervalInt = 0;
if (!int.TryParse(interval, out intervalInt))
{
    Console.WriteLine("Interval is invalid!");
    return;
}

while (true)
{
    Logger.Write("Starting folder sync...");
    FolderCheck fc = new FolderCheck(sourceFolder, targetFolder);
    fc.CompareFiles();
    Logger.Write("Sync done!");
    Thread.Sleep(intervalInt);
}




