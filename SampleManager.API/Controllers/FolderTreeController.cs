namespace SampleManager.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SampleManager.API.Models;
using SampleManager.API.Services;

[ApiController]
[Route("api/[controller]")]
public class FolderTreeController : ControllerBase
{
    private readonly IFileSystemService _fileSystemService;
    public FolderTreeController(IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Ok(_fileSystemService.GetFolderTree(""));
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