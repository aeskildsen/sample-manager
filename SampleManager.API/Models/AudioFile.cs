namespace SampleManager.API.Models;

public class AudioFile
{
    public required string Name { get; set; }
    public required string FilePath { get; set; }
    public required string FileName { get; set; }
    public required string Category { get; set; }
    public required string PackSlug { get; set; }
    public required string LicenseSlug { get; set; }
    //public required enum AudioFormat { get; set; }
    // Optional metadata
    public uint? SampleRate { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? MetaData { get; set; }
}
