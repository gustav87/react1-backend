using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using react1_backend.CloudStorage;

namespace react1_backend.Alibaba;

[ApiController]
[Route("api/[controller]")]
public class AlibabaController : ControllerBase
{
  public AlibabaController()
  {
  }

  [HttpGet]
  public IActionResult ListFiles()
  {
    AlibabaService alibabaService = new AlibabaService();
    try
    {
      var fileList = alibabaService.ListFiles();
      return Ok(fileList);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }

  [HttpPost("upload")]
  public IActionResult UploadFile([FromBody] UploadFileRequest req)
  {
    AlibabaService alibabaService = new AlibabaService();
    try
    {
      var fileList = alibabaService.ListFiles();
      if (fileList.Select(f => f.Name).Contains(req.Name))
      {
        throw new ValidationException($"Could not upload file. File {req.Name} already exists.");
      }
      alibabaService.UploadFile(req);
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
  public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
  {
    AlibabaService alibabaService = new AlibabaService();
    try
    {
      var fileList = alibabaService.ListFiles();
      if (!fileList.Select(f => f.Name).Contains(fileName))
      {
        throw new ValidationException($"Could not download file. File {fileName} does not exist.");
      }
      var file = await alibabaService.DownloadFile(fileName);
      return File(file, "application/octet-stream", fileName);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }

  [HttpDelete("{fileName}")]
  public IActionResult DeleteFile([FromRoute] string fileName)
  {
    AlibabaService alibabaService = new AlibabaService();
    try
    {
      var fileList = alibabaService.ListFiles();
      if (!fileList.Select(f => f.Name).Contains(fileName))
      {
        throw new ValidationException($"Could not delete file. File {fileName} does not exist.");
      }
      var status = alibabaService.DeleteFile(fileName);
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
