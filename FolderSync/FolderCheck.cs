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
        this.sourcePath = source;
        this.targetPath = target;
        this.interval = interval;
        this.log = log;
    }
    
    private (string[], string[]) GetFiles(string filePath)
    {
        if (Directory.Exists(filePath))
        {
            string[] files = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            string[] folders = Directory.GetDirectories(filePath, "*", SearchOption.AllDirectories);
            foreach (var folder in folders)
            {
                Console.WriteLine(folder);
            }

            return (files, folders);
        }

        return (null, null)!;
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
        var (srcFiles, srcFolders) = GetFiles(sourcePath);

        //go through folders first
        foreach (var srcFolder in srcFolders)
        {
            string relativeFolder=Path.GetRelativePath(sourcePath, srcFolder);
            string targetFolder=Path.Combine(targetPath, relativeFolder);
            Directory.CreateDirectory(targetFolder); //creates folder unless it exists
            Console.WriteLine(targetFolder);
        }
        
        
        foreach (var srcFile in srcFiles)
        {
            string file = Path.GetRelativePath(sourcePath, srcFile); //includes subfolders
            if (File.Exists(targetPath+"\\"+file))
            {
                //checksum
                var srcHash= GetHash(srcFile);
                var trgHash=GetHash(targetPath+"\\"+file);
                
                
                if (!srcHash.SequenceEqual(trgHash))
                {
                    File.Copy(srcFile, targetPath+"\\"+file, overwrite:true);
                    Console.WriteLine($"File {file} was updated in folder {targetPath}");
                    //replace
                }
                
            }
            else
            {
                File.Copy(srcFile, targetPath+"\\"+file);
                Console.WriteLine($"File {file} was copied to folder {targetPath}");
               //copy file 
            }

            var (trgFiles, trgFolders) = GetFiles(targetPath);


            
        }
        

        //then go through replica
        //if file doesnt exist in source, delete it

        //dont forget to log everything into console, log file
    }
}