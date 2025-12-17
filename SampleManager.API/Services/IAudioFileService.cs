namespace SampleManager.API.Services;
using SampleManager.API.Models;
public interface IAudioFileService
{
    public AudioFile GetAudioFile(string relativePath);
    public IEnumerable<AudioFile> GetAudioFilesInFolder(string relativeFolderPath);
}
