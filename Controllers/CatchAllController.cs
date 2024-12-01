using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace React1_Backend.Controllers;

[ApiController]
[Route("{**catchAll}")]
public class CatchAllController(ILogger<CatchAllController> logger) : ControllerBase
{
    private readonly ILogger<CatchAllController> _logger = logger;

    [HttpGet]
    public IActionResult CatchAllGet(string? catchAll)
    {
        HttpError err = new HttpError();
        err.msg = $"Route '{catchAll}' does not exist";
        err.errorCode = HttpStatusCode.NotFound;
        string json = JsonConvert.SerializeObject(err);

        return NotFound(json);
        // return Content(json, "application/json");
    }

    [HttpPost]
    public IActionResult CatchAllPost(string catchAll)
    {
        HttpError err = new HttpError();
        err.msg = $"Route '{catchAll}' does not exist";
        err.errorCode = HttpStatusCode.NotFound;
        string json = JsonConvert.SerializeObject(err);

        return NotFound(json);
        // return Content(json, "application/json");
    }

    public class HttpError
    {
        public string? msg { get; set; }
        public HttpStatusCode? errorCode { get; set; }
    }
}
