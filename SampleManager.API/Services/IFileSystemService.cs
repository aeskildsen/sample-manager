namespace SampleManager.API.Services;

public interface IFileSystemService
{
    IEnumerable<string> GetSubFolders(string relativePath);
    IEnumerable<string> GetAudioFilePathsInFolder(string relativePath);
    string ValidateAndResolvePath(string relativePath);
}