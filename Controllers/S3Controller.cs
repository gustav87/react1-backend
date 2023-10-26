using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using react1_backend.CloudStorage;

namespace react1_backend.S3;

[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase
{
    public S3Controller()
    {
    }

    [HttpGet]
    public async Task<List<CloudFile>> ListFiles()
    {
        S3Service amazonS3Service = new S3Service();
        var fileList = await amazonS3Service.ListFiles();
        return fileList;
    }

    [HttpGet("test")]
    public S3Response GetS3Response()
    {
        S3Response x = new() { Name = "xxx" };
        return x;
    }

    [HttpPost("upload/name")]
    public void UploadFileViaName([FromBody] UploadFileViaNameRequest req)
    {
        S3Service amazonS3Service = new S3Service();
        amazonS3Service.UploadFile(req.FilePath);
    }

    [HttpPost("upload")]
    public void UploadFile([FromBody] UploadFileRequest req)
    {
        S3Service amazonS3Service = new S3Service();
        amazonS3Service.UploadFile(req);
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
    {
        S3Service amazonS3Service = new S3Service();
        var file = await amazonS3Service.DownloadFile(fileName);
        return File(file, "application/octet-stream", fileName);
    }
}

public class S3Response
{
    public string? Name { get; set; }
}

public class UploadFileViaNameRequest
{
    public string FilePath { get; set; }
}
