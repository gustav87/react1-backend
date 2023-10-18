using Microsoft.AspNetCore.Mvc;

namespace react1_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase
{
    private readonly ILogger<S3Controller> _logger;

    public S3Controller(ILogger<S3Controller> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public S3Response GetS3Response()
    {
        S3Response x = new() { Name = "xxx" };
        return x;
    }

    public class S3Response
    {
        public string? Name { get; set; }
    }
}
