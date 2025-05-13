// See https://aka.ms/new-console-template for more information

using FolderSync;

Console.WriteLine("Hello, World!");
Console.WriteLine(args.Length);

if (args.Length != 4)
{
    Console.WriteLine("Application needs exactly 4 arguments: [source folder] [target folder] [interval (s)] [log file path]");
    Console.WriteLine("Example: FolderSync.exe C:\\source C:\\target 300 C:\\log.txt");
    return;
}

string sourceFolder = args[0]; //check if exists
string targetFolder = args[1]; //check if exists or create
string interval = args[2]; // check if interval valid
string logPath = args[3]; //check if exists or create
//should i check here or in foldercheck? probably here so i can

if (!Directory.Exists(sourceFolder))
{
    Console.WriteLine("Source folder does not exist!");
    return;
}

if (!Directory.Exists(targetFolder))
{
    Directory.CreateDirectory(targetFolder);
}

if (!File.Exists(logPath))
{
    File.Create(logPath).Close();
}

if (!int.TryParse(interval, out int intervalInt))
{
    Console.WriteLine("Interval is invalid!");
}


FolderCheck fc = new FolderCheck(sourceFolder, targetFolder, interval, logPath);
fc.CompareFiles();



