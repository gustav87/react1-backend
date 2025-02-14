using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace React1_Backend.Contact;

[ApiController]
[Route("api/[controller]")]
public class ContactController(ILogger<ContactController> logger) : ControllerBase
{
    private readonly ILogger<ContactController> _logger = logger;
    private readonly string MAILGUN_DOMAIN = Environment.GetEnvironmentVariable("MAILGUN_DOMAIN") ?? "";
    private readonly string MAILGUN_API_KEY = Environment.GetEnvironmentVariable("MAILGUN_API_KEY") ?? "";

    [HttpPost]
    public async Task<IActionResult> SendMail([FromBody] ContactData req, [FromServices] IValidator<ContactData> validator)
    {
        ValidationResult validationResult = validator.Validate(req);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        string url = $"https://api.mailgun.net/v3/{MAILGUN_DOMAIN}/messages";
        var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, url);

        byte[] authorizationBytes = Encoding.UTF8.GetBytes($"api:{MAILGUN_API_KEY}");
        string authorizationBase64 = Convert.ToBase64String(authorizationBytes);
        // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationBase64);
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authorizationBase64);

        string domain = $"domain={MAILGUN_DOMAIN}";
        string from = $"from=React1 Website <{req.Email}>";
        string to = "to=gustav87and@gmail.com";
        string subject = "subject=Message from react1 website";
        string msg = $"text=Name: {req.Name} \nEmail: {req.Email} \nMessage: {req.Message}";
        string body = $"{domain}&{from}&{to}&{subject}&{msg}";

        var httpcontent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
        request.Content = httpcontent;

        HttpResponseMessage response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            int statusCode = Convert.ToInt32(response.StatusCode);
            return StatusCode(statusCode, $"Error {statusCode}: {content}");
        }
        return Ok();
    }
}
