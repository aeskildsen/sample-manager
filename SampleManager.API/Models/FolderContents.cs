namespace SampleManager.API.Models;

public class FolderContents
{
    public required string Path { get; set; }
    public required IEnumerable<string> SubFolders { get; set; }
    public required IEnumerable<AudioFile> AudioFiles { get; set; }
}
