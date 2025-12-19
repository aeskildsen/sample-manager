namespace SampleManager.API.Services;
using SampleManager.API.Models;
using System.Data;
using Microsoft.Extensions.Options;
using SampleManager.API.Configuration;

public class FileSystemService : IFileSystemService
{
    private readonly LibrarySettings _settings;
    
    public FileSystemService(IOptions<LibrarySettings> options)
    {
        _settings = options.Value;
    }
    public IEnumerable<string> GetSubFolders(string relativePath)
    {
        string folderPath = ValidateAndResolvePath(relativePath);

        return Directory.EnumerateDirectories(folderPath)
            .Select(dir => Path.GetRelativePath(_settings.SampleLibraryPath, dir));
    }

    public IEnumerable<string> GetAudioFilePathsInFolder(string relativePath)
    {
        string folderPath = ValidateAndResolvePath(relativePath);
        
        return Directory.EnumerateFiles(folderPath)
            .Where(file => _settings.AudioFileExtensions.Any(extension =>
                file.EndsWith(extension, StringComparison.OrdinalIgnoreCase)
            ))
            .Select(file => Path.GetRelativePath(_settings.SampleLibraryPath, file));
    }

    public FolderNode GetFolderTree(string relativePath = "")
    {
        string fullPath = ValidateAndResolvePath(relativePath);
        string name = relativePath == "" ? "Library" : Path.GetFileName(fullPath);
        var subDirs = Directory.GetDirectories(fullPath);
        List<FolderNode> subFolders = new();
        foreach (string subDir in subDirs)
        {
            string relativeSubDirPath = Path.Combine(relativePath, Path.GetFileName(subDir));
            subFolders.Add(GetFolderTree(relativeSubDirPath));
        }
        return new FolderNode {
            Name = name,
            Path = relativePath,
            SubFolders = subFolders,
        };
    }

    public string ValidateAndResolvePath(string relativePath)
    {
        string fullPath = Path.GetFullPath(
            Path.Combine(_settings.SampleLibraryPath, relativePath)
        );

        // Check for directory traversal attack
        if (!fullPath.StartsWith(_settings.SampleLibraryPath, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException(
                $"Access denied: Requested path {relativePath} is outside sample library root."
            );
        }

        return fullPath;
    }
}
