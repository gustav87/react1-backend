using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace React1_Backend.Contact;

[ApiController]
[Route("api/[controller]")]
public class Contact2Controller(ILogger<ContactController> logger, ContactService contactService) : ControllerBase
{
    private readonly ILogger<ContactController> _logger = logger;
    private readonly ContactService _contactService = contactService;

    [HttpPost]
    public async Task<IActionResult> SendMail([FromBody] ContactData req)
    {
        try
        {
            await _contactService.SendMail(req);
        }
        catch (HttpRequestException ex)
        {
            int statusCode = (int)ex.StatusCode;
            return StatusCode(statusCode, $"Error {statusCode}: {ex.Message}");
        }
        catch (Exception ex)
        {
            StatusCode(503, ex.Message);
        }
        return Ok();
    }
}
