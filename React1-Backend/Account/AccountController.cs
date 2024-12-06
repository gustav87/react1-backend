using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using React1_Backend.Filters.ActionFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace React1_Backend.Account;

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
        string token = await _accountService.LogIn(req);
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Username or password incorrect.");
        }
        Console.WriteLine($"{req.Username} logged in.");
        return Ok(token);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpData req)
    {
        try
        {
            List<Account> numberOfAccounts = await _accountService.GetAsync();
            if (numberOfAccounts.Count > 5)
            {
                return StatusCode(503, "Too many accounts.");
            }
            Account account = await _accountService.GetAsyncByUsername(req.Username);
            if (account != null)
            {
                return Conflict($"User {req.Username} already exists.");
            }
            await _accountService.CreateAccount(req);
            return Ok($"User {req.Username} created!");
        }
        catch (MongoWriteException ex)
        {
            return StatusCode(503, $"Error: {ex.Message}");
        }
        catch (TimeoutException)
        {
            return StatusCode(503, "Unable to connect to database.");
        }
        catch (Exception)
        {
            return StatusCode(503, "Something went wrong.");
        }
    }
}
