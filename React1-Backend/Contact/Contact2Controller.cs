using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace React1_Backend.Contact;

[ApiController]
[Route("api/[controller]")]
public class Contact2Controller(ILogger<ContactController> logger, ContactService contactService, [FromServices] IValidator<ContactData> validator) : ControllerBase
{
    private readonly ILogger<ContactController> _logger = logger;
    private readonly ContactService _contactService = contactService;

    [HttpPost]
    public async Task<IActionResult> SendMail([FromBody] ContactData req)
    {
        ValidationResult validationResult = validator.Validate(req);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        try
        {
            await _contactService.SendMail(req);
            await _contactService.InsertIntoDb(req);
        }
        catch (HttpRequestException ex)
        {
            int statusCode = (int)ex.StatusCode;
            return StatusCode(statusCode, $"Error {statusCode}: {ex.Message}");
        }
        catch (MongoWriteException ex)
        {
            return StatusCode(503, $"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(503, ex.Message);
        }
        return Ok();
    }
}
