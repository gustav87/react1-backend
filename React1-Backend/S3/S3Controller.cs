using Microsoft.AspNetCore.Mvc;
using React1_Backend.Contracts;
using React1_Backend.Filters.ActionFilters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace React1_Backend.S3;

[ApiController]
[Route("api/[controller]")]
public class S3Controller(S3Service s3Service) : ControllerBase
{
    private readonly S3Service _s3Service = s3Service;

    [HttpGet]
    public async Task<List<CloudFile>> ListFiles()
    {
        var fileList = await _s3Service.ListFiles();
        return fileList;
    }

    [HttpGet("test")]
    public S3Response GetS3Response()
    {
        S3Response x = new() { Name = "xxx" };
        return x;
    }

    [HttpPost("upload/name")]
    [EndpointDisabledFilter]
    public void UploadFileViaName([FromBody] UploadFileViaNameRequest req)
    {
        _s3Service.UploadFile(req.FilePath);
    }

    [HttpPost("upload")]
    [EndpointDisabledFilter]
    public async Task<IActionResult> UploadFile([FromBody] UploadFileRequest req)
    {
        try
        {
            var fileList = await _s3Service.ListFiles();
            if (fileList.Select(f => f.Name).Contains(req.Name))
            {
                throw new ValidationException($"Could not upload file. File {req.Name} already exists.");
            }
            _s3Service.UploadFile(req);
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
        var file = await _s3Service.DownloadFile(fileName);
        return File(file, "application/octet-stream", fileName);
    }
}

public class S3Response
{
    public string Name { get; set; }
}

public class UploadFileViaNameRequest
{
    public string FilePath { get; set; } = null!;
}
