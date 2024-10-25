using Microsoft.AspNetCore.Mvc;

namespace react1_backend.Paypal;

[ApiController]
[Route("api/[controller]")]
public class PaypalController : ControllerBase
{
  private readonly PaypalService _paypalService;
  public PaypalController
  (
    PaypalService paypalService
  )
  {
    _paypalService = paypalService;
  }

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
