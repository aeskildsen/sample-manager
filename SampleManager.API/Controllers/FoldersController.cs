namespace SampleManager.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SampleManager.API.Services;

[ApiController]
[Route("api/[controller]")]
public class FoldersController : ControllerBase
{
    private readonly IFileSystemService _fileSystemService;
    public FoldersController(IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
    }

    [HttpGet]
    public IActionResult GetRootFolders()
    {
        var folders = _fileSystemService.GetRootFolders();
        return Ok(folders);
    }
}
