using Microsoft.AspNetCore.Mvc;
using React1_backend.Filters.ActionFilters;

namespace React1_backend.Paypal;

[ApiController]
[Route("api/[controller]")]
[AsyncAdminTokenFilter(PermissionName = "hi")]
public class PaypalController(PaypalService paypalService) : ControllerBase
{
  private readonly PaypalService _paypalService = paypalService;

    [HttpGet("transactions")]
  public async Task<IActionResult> GetTransactions()
  {
    try
    {
      var transactions = await _paypalService.GetTransactions();
      return Ok(transactions);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }

  [HttpGet("balance")]
  public async Task<IActionResult> GetBalance()
  {
    try
    {
      var balance = await _paypalService.GetBalance();
      return Ok(balance);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }
}
