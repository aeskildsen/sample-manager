namespace SampleManager.API.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SampleManager.API.Services;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileSystemService _fileSystemService;
    private readonly FileExtensionContentTypeProvider _provider;
    
    public FilesController(IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
        _provider = new FileExtensionContentTypeProvider();
    }

    [HttpGet("stream/{*relativePath}")]
    public IActionResult Get(string relativePath)
    {
        try
        {
            string fullPath = _fileSystemService.ValidateAndResolvePath(relativePath);
            // Invalid request handling
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound(new { error = $"Error: {relativePath} does not exist." });
            }
            
            if (!_provider.TryGetContentType(fullPath, out string? contentType))
            {
                return BadRequest(new { error = "Unknown file type" });
            }

            if (!contentType.StartsWith("audio/"))
            {
                return BadRequest(new { error = "Not an audio file" });
            }

            return File(System.IO.File.OpenRead(fullPath), contentType, true);
        }
        catch (IOException ex)
        {
            return StatusCode(500, new { error = $"Error reading file {relativePath}: {ex.Message}"});
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Internal server error: {ex.Message}"});
        }
    }
}
