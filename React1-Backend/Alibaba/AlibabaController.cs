using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using React1_Backend.Contracts;
using React1_Backend.Filters.ActionFilters;
using System.Threading.Tasks;
using System.Linq;

namespace React1_Backend.Alibaba;

[ApiController]
[Route("api/[controller]")]
public class AlibabaController(AlibabaService alibabaService) : ControllerBase
{
    private readonly AlibabaService _alibabaService = alibabaService;

    [HttpGet]
    public IActionResult ListFiles()
    {
        try
        {
            var fileList = _alibabaService.ListFiles();
            return Ok(fileList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("upload")]
    [EndpointDisabledFilter]
    public IActionResult UploadFile([FromBody] UploadFileRequest req)
    {
        try
        {
            var fileList = _alibabaService.ListFiles();
            if (fileList.Select(f => f.Name).Contains(req.Name))
            {
                throw new ValidationException($"Could not upload file. File {req.Name} already exists.");
            }
            _alibabaService.UploadFile(req);
            return Ok($"File {req.Name} uploaded!");
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("download/{fileName}")]
    [EndpointDisabledFilter]
    public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
    {
        try
        {
            var fileList = _alibabaService.ListFiles();
            if (!fileList.Select(f => f.Name).Contains(fileName))
            {
                throw new ValidationException($"Could not download file. File {fileName} does not exist.");
            }
            var file = await _alibabaService.DownloadFile(fileName);
            return File(file, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{fileName}")]
    [EndpointDisabledFilter]
    public IActionResult DeleteFile([FromRoute] string fileName)
    {
        try
        {
            var fileList = _alibabaService.ListFiles();
            if (!fileList.Select(f => f.Name).Contains(fileName))
            {
                throw new ValidationException($"Could not delete file. File {fileName} does not exist.");
            }
            var status = _alibabaService.DeleteFile(fileName);
            return Ok($"File {fileName} deleted.");
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
