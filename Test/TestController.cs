using Microsoft.AspNetCore.Mvc;

namespace React1_backend.Test;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("weather")]
    public IEnumerable<WeatherForecast> GetWeather()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = "cheese"
        })
        .ToArray();
    }

    [HttpGet("test1")]
    public Test1 GetTest1()
    {
        Test1 x = new() { Summary = "hi!" };
        return x;
    }

    public class Test1
    {
        public string? Summary { get; set; }
    }
}
