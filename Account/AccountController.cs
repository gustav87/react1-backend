using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace React1_backend.Account;

[ApiController]
[Route("api/[controller]")]
[AsyncAdminTokenFilter(PermissionName = "hi")]
public class AccountController(ILogger<AccountController> logger, AccountService accountService) : ControllerBase
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly AccountService _accountService = accountService;

    [HttpPost("log-in")]
    public async Task<IActionResult> LogIn([FromBody] Account req)
    {
        if (IsEmpty(req))
        {
            return BadRequest("Username or Password cannot be empty.");
        }
        string token = await _accountService.LogIn(req);
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Username or password incorrect.");
        }
        Console.WriteLine($"{req.Username} logged in.");
        return Ok(token);
    }

    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount([FromBody] Account req)
    {
        if (IsEmpty(req))
        {
            return BadRequest("Username or Password cannot be empty.");
        }
        try
        {
            List<Account> numberOfAccounts = await _accountService.GetAsync();
            if (numberOfAccounts.Count > 5)
            {
                return StatusCode(503, "Too many accounts.");
            }
            Account? account = await _accountService.GetAsyncByUsername(req.Username);
            if (account != null)
            {
                return Conflict($"User {req.Username} already exists.");
            }
            await _accountService.CreateAccount(req);
            return Ok($"User {req.Username} created!");
        }
        catch (MongoWriteException ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(503, "Something went wrong.");
        }
    }

    private static bool IsEmpty(Account account)
    {
        return string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password);
    }
}
