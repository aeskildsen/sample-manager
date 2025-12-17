namespace SampleManager.API.Services;
using SampleManager.API.Configuration;
using Microsoft.Extensions.Options;
using SampleManager.API.Models;

public class PackService : IPackService
{
    private readonly LibrarySettings _settings;
    private Dictionary<string, SamplePack> _packs;

    public PackService(IOptions<LibrarySettings> options)
    {
        _settings = options.Value;
        _packs = LoadAllSamplePacks();
    }

    public SamplePack? GetPack(string packSlug)
    {
        _packs.TryGetValue(packSlug, out var pack);
        return pack;
    }

    public IEnumerable<SamplePack> GetAllPacks()
    {
        return _packs.Values;
    }

    private Dictionary<string, SamplePack> LoadAllSamplePacks()
    {
        return new Dictionary<string, SamplePack>
        {
            { "ajubamusic", new SamplePack {
                Slug = "ajubamusic",
                Name = "Ajubamusic",
                Vendor = "Freesound.org",
                FileCount = 55,
                License = "CCBY",
                Creator = "Freesound user 'ajubamusic'",
                LicenseFile = null,
                URL = "https://freesound.org/people/ajubamusic/",
                Categories = null,
            }},
            { "algorhythms", new SamplePack {
                Slug = "algorhythms",
                Name = "Algorhytms",
                Vendor = "Soundtrack Loops",
                FileCount = 55,
                License = "CCBY",
                Creator = "Freesound user 'ajubamusic'",
                LicenseFile = null,
                URL = "https://freesound.org/people/ajubamusic/",
                Categories = null,
            }}
        };
    }

}
/*
public required string Slug { get; set; }
public required string Name { get; set; }
public required string Vendor { get; set; }
public string? Creator { get; set; }
public required uint FileCount { get; set; }
public required string License { get; set; }
public string? LicenseFile { get; set; }
public string? URL { get; set; }
public IEnumerable<string>? Categories { get; set; }
*/