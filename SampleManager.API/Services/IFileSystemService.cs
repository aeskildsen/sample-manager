namespace SampleManager.API.Services;
using SampleManager.API.Models;
public interface IFileSystemService
{
    IEnumerable<string> GetSubFolders(string relativePath);
    IEnumerable<string> GetAudioFilePathsInFolder(string relativePath);
    FolderNode GetFolderTree(string relativePath);
    string ValidateAndResolvePath(string relativePath);
}