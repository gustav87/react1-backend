using Microsoft.AspNetCore.Mvc;

namespace react1_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    [HttpPost("log-in")]
    public IActionResult LogIn([FromBody] LoginModel loginModel)
    {
        return Ok($"log in! username: {loginModel.Username}. password: {loginModel.Password}");
    }

    [HttpPost("create-account")]
    public IActionResult CreateAccount([FromBody] LoginModel loginModel)
    {
        return Ok($"create account! username: {loginModel.Username}. password: {loginModel.Password}");
    }

    public class LoginModel
    {
        public string? Username {get; set;}
        public string? Password {get; set;}
    }
}
