using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AWS_SQS_Integration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        
        [HttpPost]
        public IActionResult Post(WeatherForecast date)
        {
            var credentials = new BasicAWSCredentials("ACCESS KEY", "SECRET ACCESS KEY");
            var client = new AmazonSQSClient(credentials, RegionEndpoint.USEast1);



            var request = new SendMessageRequest
            {

                MessageBody = JsonSerializer.Serialize(date),
                QueueUrl = "URL IN SQS AWS CONSOLE"
            };
            client.SendMessageAsync(request).Wait();
            return NoContent();
        }



        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var credentials = new BasicAWSCredentials("ACCESS KEY", "SECRET ACCESS KEY");
            var client = new AmazonSQSClient(credentials, RegionEndpoint.USEast1);



            var request = new ReceiveMessageRequest
            {
                // MessageBody = JsonSerializer.Serialize(emp),



                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 10,
                QueueUrl = "URL IN SQS AWS CONSOLE"
            };
            var req = client.ReceiveMessageAsync(request).Result;
            var emplist = new List<WeatherForecast>();
            foreach (var item in req.Messages)
            {
                var m = JsonSerializer.Deserialize<WeatherForecast>(item.Body);
                emplist.Add(m);



            }
            return emplist;
        }
    }
}

    
