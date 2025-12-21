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
    public int? SampleRate { get; set; }
    public double? Duration { get; set; }
    public int? Channels { get; set; }
    public int? BitDepth { get; set; }
    public int? BitRate { get; set; }
    public string? Tags { get; set; }
    public string? Format { get; set; }
}
