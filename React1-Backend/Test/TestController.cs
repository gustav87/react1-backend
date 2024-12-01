using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace React1_Backend.Test;

[ApiController]
[Route("api/[controller]")]
public class TestController(ILogger<TestController> logger) : ControllerBase
{
    private readonly ILogger<TestController> _logger = logger;

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

    [HttpGet("boynames")]
    public ActionResult<List<string>> GetBoyNames()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Test/BoyNames2023.json");
        string text = System.IO.File.ReadAllText(path);
        BoyNames json = JsonConvert.DeserializeObject<BoyNames>(text);
        return Ok(json?.Names);
        // using (StreamReader r = new StreamReader("Test/BoyNames2023.json"))
        // {
        //     string json = r.ReadToEnd();
        //     BoyNames? names = JsonConvert.DeserializeObject<BoyNames>(json);
        //     Console.WriteLine("tt");
        //     return names;
        // }
    }

    public class Test1
    {
        public string Summary { get; set; }
    }

    public class BoyNames
    {
        public int Year;
        public List<string> Names;
    }
}
