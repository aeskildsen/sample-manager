namespace SampleManager.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SampleManager.API.Models;
using SampleManager.API.Services;

[ApiController]
[Route("api/[controller]")]
public class FoldersController : ControllerBase
{
    private readonly IAudioFileService _audioFileService;
    private readonly IFileSystemService _fileSystemService;
    public FoldersController(IAudioFileService audioFileService, IFileSystemService fileSystemService)
    {
        _audioFileService = audioFileService;
        _fileSystemService = fileSystemService;
    }

    [HttpGet("{*relativePath}")]
    public IActionResult Get(string relativePath = "")
    {
        try
        {
            // Here, assume the requested relativePath is a folder
            var audioFiles = _audioFileService.GetAudioFilesInFolder(relativePath);
            var subFolders = _fileSystemService.GetSubFolders(relativePath);

            return Ok(new FolderContents
            {
                Path = relativePath,
                SubFolders = subFolders,
                AudioFiles = audioFiles
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { error = ex.Message });
        }
        catch (DirectoryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
