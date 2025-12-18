namespace SampleManager.API.Services;
using SampleManager.API.Models;
using SampleManager.API.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

public class AudioFileService : IAudioFileService
{
    private readonly IFileSystemService _fileSystemService;
    private readonly LibrarySettings _settings;
    private const string UNKNOWN_FIELD = "UNDEFINED";
    
    private static Dictionary<string, string> FormatMap { get; } = new() 
    {
        // Descriptions from TagLib# mapped to user friendly abbreviations
        { "PCM Audio", "WAV" },
        { "PCM Audio in IEEE floating-point format", "WAV" },
        { "AIFF Audio", "AIFF" },
        { "Flac Audio", "FLAC" },
        { "DSF Audio", "DSF" },
        { "WavPack Version 4 Audio", "WV" },
        { "Monkey's Audio APE Version 3.990", "APE" },
        { "MPEG-4 Audio (alac)", "ALAC" },
        { "ADTS AAC", "AAC" },
        { "MPEG-4 Audio (mp4a)", "AAC" },
        { "Vorbis Version 0 Audio", "OGG" },
        { "Opus Version 1 Audio", "OPUS" },
        { "MPEG Version 1 Audio, Layer 3", "MP3" },
        { "MPEG Version 2 Audio, Layer 3", "MP3" },
        { "ISO/MPEG Layer 3 Audio", "MP3" },
        { "MusePack Version 7 Audio", "MPC" },
        { "MusePack Version 8 Audio", "MPC" },
    };

    public AudioFileService(IFileSystemService fileSystemService, IOptions<LibrarySettings> options)
    {
        _fileSystemService = fileSystemService;
        _settings = options.Value;
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
            // TODO: Raise a warning here?
        }

        // TODO: Check validity of pack and license

        var metadata = TagLib.File.Create(Path.Combine(_settings.SampleLibraryPath, relativePath));
        int sampleRate = metadata.Properties.AudioSampleRate;
        int channels = metadata.Properties.AudioChannels;
        TimeSpan duration = metadata.Properties.Duration;
        int bitDepth = metadata.Properties.BitsPerSample;
        int bitRate = metadata.Properties.AudioBitrate;
        string rawFormat = metadata.Properties.Description;
        string format = FormatMap.GetValueOrDefault(
            rawFormat,
            Path.GetExtension(relativePath).TrimStart('.').ToUpperInvariant()
        );

        // We probably want to avoid this in production, or maybe enable it with a config option
        if (!FormatMap.ContainsKey(rawFormat) && !string.IsNullOrEmpty(rawFormat))
        {
            Console.WriteLine($"Unmapped format: '{rawFormat}' - using extension fallback");
        }
        
        // Tags are a mess, avoid for now...
        //var tags = metadata.Tag.ToString();
        
        return new AudioFile
        {
            Name = name,
            FilePath = relativePath,
            FileName = filename,
            PackSlug = packSlug,
            LicenseSlug = licenseSlug,
            Category = ExtractCategoryFromPath(relativePath),
            SampleRate = sampleRate,
            Channels = channels,
            Duration = duration,
            BitDepth = bitDepth,
            BitRate = bitRate,
            Format = format
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
