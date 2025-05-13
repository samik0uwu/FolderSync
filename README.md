# Test Task - Folder 
A C# console app synchronizing two folders periodically

## Features

- one-way synchronization (source -> replica)
- periodic sync, interval defined in arguments
- file/folder creation, copy, removal
- logging to console and file defined in arguments
- MD5 checksum file comparison

## How it works
- new files, folders from source copied to replica
- updated files in source replaced in replica
- files/folder in replica but not in source deleted
- every action logged with a timestamp

## How to use
```bash
FolderSync.exe <source_folder_path> <replica_folder_path> <interval_in_ms> <log_file_path>
```
### Example
```bash
FolderSync.exe C:\files\source C:\files\backup 300000 C:\logs\log.txt
```


## Assignment
> Please implement a program that synchronizes two folders: source and replica.
> The program should maintain a full, identical copy of source folder at replica
> folder.
>
> Solve the test task by writing a program in C#.
> 
> Synchronization must be one-way: after the synchronization content of the replica folder should be modified to exactly match content of the source folder;
> 
> Synchronization should be performed periodically;
>
> File creation/copying/removal operations should be logged to a file and to the
> console output;
>
> Folder paths, synchronization interval and log file path should be provided using the command line arguments;
>
> It is undesirable to use third-party libraries that implement folder synchronization;
>
> It is allowed (and recommended) to use external libraries implementing other well- known algorithms. For example, there is no point in implementing yet another function that calculates MD5 if you need it for the task – it is perfectly acceptable to use a third-party (or built-in) library;
>
> The solution should be presented in the form of a link to the public GitHub repository.
