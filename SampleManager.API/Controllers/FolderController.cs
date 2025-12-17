namespace SampleManager.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SampleManager.API.Services;

[ApiController]
[Route("api/[controller]")]
public class FolderController : ControllerBase
{
    private readonly IAudioFileService _audioFileService;
    public FolderController(IAudioFileService audioFileService)
    {
        _audioFileService = audioFileService;
    }

    [HttpGet("{*path}")]
    public IActionResult Get(string path = "")
    {
        try
        {
            var audioFiles = _audioFileService.GetAudioFilesInFolder(path);
            return Ok(audioFiles);
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
