// See https://aka.ms/new-console-template for more information

using FolderSync;

Console.WriteLine("Hello, World!");
Console.WriteLine(args.Length);

if (args.Length != 4)
{
    Console.WriteLine("Wrong number of arguments");
    return;
}

string sourceFolder = args[0]; //check if exists
string targetFolder = args[1]; //check if exists or create
string interval = args[2]; // check if interval valid
string logPath = args[3]; //check if exists or create

FolderCheck fc = new FolderCheck(sourceFolder, targetFolder, interval, logPath);
fc.GetSourceFiles();



