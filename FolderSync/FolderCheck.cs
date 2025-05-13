using System.Security.Cryptography;
using System.Security.Principal;

namespace FolderSync;

public class FolderCheck
{
    public string source { get; private set; }
    public string target { get; private set; }
    public string interval { get; private set; } //should i use timespan?
    public string log { get; set; }

    public FolderCheck(string source, string target, string interval, string log)
    {
        this.source = source;
        this.target = target;
        this.interval = interval;
        this.log = log;
    }
    
    private (string[], string[]) GetSourceFiles()
    {
        if (Directory.Exists(source))
        {
            string[] files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            string[] folders = Directory.GetDirectories(source, "*", SearchOption.AllDirectories);
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
        var (srcFiles, srcFolders) = GetSourceFiles();

        foreach (var srcFile in srcFiles)
        {
            string file = Path.GetFileName(srcFile);
            //i need to get filename for the srcfile and then check it in replica
            if (File.Exists(target+"\\"+file))
            {
                //checksum
                var srcHash= GetHash(srcFile);
                var trgHash=GetHash(target+"\\"+file);
                
                
                if (!srcHash.SequenceEqual(trgHash))
                {
                    File.Copy(srcFile, target+"\\"+file, overwrite:true);
                    Console.WriteLine($"File {file} was updated in folder {target}");
                    //replace
                }
                
            }
            else
            {
                File.Copy(srcFile, target+"\\"+file);
                Console.WriteLine($"File {file} was copied to folder {target}");
               //copy file 
            }

            
            
        }
        

        //then go through replica
        //if file doesnt exist in source, delete it

        //dont forget to log everything into console, log file
    }
}