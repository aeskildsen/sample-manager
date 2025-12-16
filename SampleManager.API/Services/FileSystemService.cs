namespace SampleManager.API.Services;
using Microsoft.Extensions.Options;
using SampleManager.API.Configuration;

public class FileSystemService : IFileSystemService
{
    private readonly LibrarySettings _settings;

    public FileSystemService(IOptions<LibrarySettings> options)
    {
        // Initialize the service
        _settings = options.Value;
    }
    public IEnumerable<string> GetRootFolders()
    {

        return Directory.EnumerateDirectories(_settings.SampleLibraryPath);
    }
}
