using System.Security.Cryptography;
using System.Security.Principal;

namespace FolderSync;

public class FolderCheck
{
    public string sourcePath { get; private set; }
    public string targetPath { get; private set; }
    public string interval { get; private set; } //should i use timespan?
    public string log { get; set; }

    public FolderCheck(string source, string target, string interval, string log)
    {
        sourcePath = source;
        this.targetPath = target;
        this.interval = interval;
        this.log = log;
    }

    private string[] GetFolders(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            string[] folders = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories);

            return folders;
        }

        return null;
    }

    private string[] GetFiles(string filePath)
    {
        if (Directory.Exists(filePath))
        {
            string[] files = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories);

            return files;
        }

        return null;
    }

    public byte[] GetHash(string source)
    {
        using (MD5 md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(source))
            {
                return md5.ComputeHash(stream);
            }
        }
    }

    public void CompareFiles()
    {
        var srcFiles = GetFiles(sourcePath);
        var srcFolders = GetFolders(sourcePath);
        //go through folders first
        foreach (var srcFolder in srcFolders)
        {
            string relativeFolder = Path.GetRelativePath(sourcePath, srcFolder);
            string targetFolder = Path.Combine(targetPath, relativeFolder);
            Directory.CreateDirectory(targetFolder); //creates folder unless it exists
            // Console.WriteLine(targetFolder);
        }


        foreach (var srcFile in srcFiles)
        {
            string file = Path.GetRelativePath(sourcePath, srcFile); //includes subfolders
            if (File.Exists(targetPath + "\\" + file))
            {
                //checksum
                var srcHash = GetHash(srcFile);
                var trgHash = GetHash(targetPath + "\\" + file);


                if (!srcHash.SequenceEqual(trgHash))
                {
                    File.Copy(srcFile, targetPath + "\\" + file, overwrite: true);
                    Console.WriteLine($"File {file} updated in folder {targetPath}");
                    //replace
                }
            }
            else
            {
                File.Copy(srcFile, targetPath + "\\" + file);
                Console.WriteLine($"File {file} copied to folder {targetPath}");
                //copy file 
            }
        }

        var trgFolders = GetFolders(targetPath);
        //foreach each folder
        //if doesnt exist in source, delete (incl. files)
        foreach (var trgFolder in
                 trgFolders.OrderByDescending(f =>
                     f.Length)) //order by descending to delete subfolders first, then parent folders
        {
            string relativeFolder = Path.GetRelativePath(targetPath, trgFolder);
            string sourceFolder = Path.Combine(sourcePath, relativeFolder);
            if (!Directory.Exists(sourceFolder))
            {
                //delete trgFolder
                Directory.Delete(trgFolder, true);
                Console.WriteLine($"Directory {trgFolder} deleted.");
            }
        }
        var trgFiles = GetFiles(targetPath); //get files after deleting folders
        foreach (var trgFile in trgFiles)
        {
            string file = Path.GetRelativePath(targetPath, trgFile);
            string srcFile = Path.Combine(sourcePath, file);
            if (!File.Exists(srcFile))
            {
                File.Delete(trgFile);
                Console.WriteLine($"File {trgFile} deleted.");
            }
        }
        
    }
}