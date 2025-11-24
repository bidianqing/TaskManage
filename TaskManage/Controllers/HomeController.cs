using Microsoft.AspNetCore.Mvc;

namespace TaskManage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;
        private readonly ITaskService _taskService;

        public HomeController(ILogger<HomeController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(int num)
        {
            var cts = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    _logger.LogInformation($"{num}号任务执行");

                    await Task.Delay(1000);
                }
            }, cts.Token);
            _taskService.AddTask(num, task, cts);


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("CancelTask")]
        public async Task CancelTask(int num)
        {
            await _taskService.RemoveTask(num);
        }
    }
}
