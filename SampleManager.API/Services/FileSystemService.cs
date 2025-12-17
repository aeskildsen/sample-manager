namespace SampleManager.API.Services;

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
    public IEnumerable<string> GetRootFolders()
    {
        return Directory.EnumerateDirectories(_settings.SampleLibraryPath);
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

    private string ValidateAndResolvePath(string relativePath)
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
