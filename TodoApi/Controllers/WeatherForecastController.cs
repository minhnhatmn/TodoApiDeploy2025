using Microsoft.AspNetCore.Mvc;

namespace TodoApiDeploy2025.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly IReadOnlyList<string> Summaries = new List<string>
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    }.AsReadOnly();

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Count)]
        })
        .ToArray();
    }
    [HttpPost]
    public ActionResult Post([FromBody] string summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            return BadRequest("Summary cannot be empty");
        }

        if (summary.Length > 100) // Add reasonable length limit
        {
            return BadRequest("Summary is too long");
        }

        if (Summaries.Contains(summary))
        {
            return Conflict("Summary already exists");
        }

        if (Summaries.Count >= 100) // Add reasonable size limit
        {
            return BadRequest("Maximum number of summaries reached");
        }

        try
        {
            Summaries.Add(summary);
            return Ok(new { message = "Summary added successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding summary");
            return StatusCode(500, "An error occurred while adding the summary");
        }
    }
}
