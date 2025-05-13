using System.Security.Cryptography;

namespace FolderSync;

public class FolderCheck
{
    public string sourcePath { get; private set; }
    public string targetPath { get; private set; }

    public FolderCheck(string source, string target)
    {
        this.sourcePath = source;
        this.targetPath = target;
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

        //create all folders first, then files
        foreach (var srcFolder in srcFolders)
        {
            string relativeFolder = Path.GetRelativePath(sourcePath, srcFolder);
            string targetFolder = Path.Combine(targetPath, relativeFolder);
            if (!Directory.Exists(targetFolder)) //only need to check to be able to log folder creation
            {
                Directory.CreateDirectory(targetFolder); //creates folder unless it exists
                Logger.Write($"Created folder {targetFolder}");
            }
        }


        foreach (var srcFile in srcFiles)
        {
            string file = Path.GetRelativePath(sourcePath, srcFile);
            string targetFile = Path.Combine(targetPath, file);
            if (File.Exists(Path.Combine(targetPath, file))) 
            {
                //checksum
                var srcHash = GetHash(srcFile);
                var trgHash = GetHash(targetFile); 

                if (!srcHash.SequenceEqual(trgHash))
                {
                    File.Copy(srcFile, targetFile, overwrite: true); //overwrite true to replace existing file
                    Logger.Write($"File {targetFile} updated"); 
                }
            }
            else
            {
                File.Copy(srcFile, targetFile);
                Logger.Write($"File {file} copied to folder {targetPath}");
            }
        }

        var trgFolders = GetFolders(targetPath);
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
                Logger.Write($"Directory {trgFolder} deleted.");
            }
        }
        var trgFiles = GetFiles(targetPath); //get files after deleting folders, because some files might have been deleted
        foreach (var trgFile in trgFiles)
        {
            string file = Path.GetRelativePath(targetPath, trgFile);
            string srcFile = Path.Combine(sourcePath, file);
            if (!File.Exists(srcFile))
            {
                File.Delete(trgFile);
                Logger.Write($"File {trgFile} deleted.");
            }
        }
        
    }
}