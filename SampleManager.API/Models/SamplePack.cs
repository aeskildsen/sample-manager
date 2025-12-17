namespace SampleManager.API.Models;

public class SamplePack
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required string Vendor { get; set; }
    public string? Creator { get; set; }
    public required uint FileCount { get; set; }
    public required string License { get; set; }
    public string? LicenseFile { get; set; }
    public string? URL { get; set; }
    public IEnumerable<string>? Categories { get; set; }
    public string? Notes {get; set; }
}
