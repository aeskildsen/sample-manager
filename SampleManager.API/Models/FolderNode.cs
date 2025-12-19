namespace SampleManager.API.Models;

public class FolderNode
{
    public required string Name { get; set; }
    public required string Path { get; set; }
    public List<FolderNode> SubFolders { get; set; } = new();
}
