namespace SampleManager.API.Services;

public interface IFileSystemService
{
    IEnumerable<string> GetRootFolders();
}