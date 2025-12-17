namespace SampleManager.API.Services;
using SampleManager.API.Models;
public class AudioFileService : IAudioFileService
{
    private readonly IFileSystemService _fileSystemService;
    private const string UNKNOWN_FIELD = "UNDEFINED";
    public AudioFileService(IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
    }

    public IEnumerable<AudioFile> GetAudioFilesInFolder(string relativeFolderPath)
    {
        var paths = _fileSystemService.GetAudioFilePathsInFolder(relativeFolderPath);
        return paths.Select(GetAudioFile);
    }

    public AudioFile GetAudioFile(string relativePath)
    {
        // remove the extension...?
        string filename = Path.GetFileName(relativePath);
        string extension = Path.GetExtension(relativePath);

        string name, packSlug, licenseSlug;

        var parsed = ParseFileName(filename);
        
        if (parsed.HasValue)
        {
            (name, packSlug, licenseSlug) = parsed.Value;
        }
        else
        {
            name = filename;
            packSlug = UNKNOWN_FIELD;
            licenseSlug = UNKNOWN_FIELD;
            // Maybe make a flag warning here?
        }

        // Check validity of pack and license
        
        return new AudioFile
        {
            Name = name,
            FilePath = relativePath,
            FileName = filename,
            PackSlug = packSlug,
            LicenseSlug = licenseSlug,
            Category = ExtractCategoryFromPath(relativePath)
        };
    }

    private static (string name, string packSlug, string licenseSlug)? ParseFileName(string filename)
    {
        string[] segments = Path.GetFileNameWithoutExtension(filename).Split("_");
        if (segments.Length < 3)
        {
            // In place of proper logging...
            Console.WriteLine($"Audio file name {filename} is improperly named - should contain at least two underscore characters to differentiate name, pack, and license.");
            return null;
        }

        string name = string.Join("_", segments[..^2]);
        string packSlug = segments[^2];
        string licenseSlug = segments[^1];

        return (name, packSlug, licenseSlug);
    }

    private static string ExtractCategoryFromPath(string relativePath)
    {
        string? dir = Path.GetDirectoryName(relativePath);
        
        if (dir == "")
        {
            return "uncategorized";
        }
        else if (dir is null)
        {
            return UNKNOWN_FIELD;
        }

        return dir.Replace(Path.DirectorySeparatorChar, '-');
    }
}
