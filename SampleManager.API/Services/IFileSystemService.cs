namespace SampleManager.API.Services;

public interface IFileSystemService
{
    IEnumerable<string> GetRootFolders();
    IEnumerable<string> GetAudioFilePathsInFolder(string relativePath);
    //IEnumerable<string> GetAudioFileInfo(string relativePath);
}