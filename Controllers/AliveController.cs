using CQDT.CloudClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CloudMiddleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AliveController : ControllerBase
    {
        private readonly ILogger<AliveController> _logger;
        private readonly Settings _mySettings;
        public AliveController(ILogger<AliveController> logger, IOptions<Settings> mySettings)
        {
            _logger = logger;
            _mySettings = mySettings.Value;
        }

        [HttpGet(Name = "Get")]
        public object Get()
        {
            return new
            {
#if DEBUG
                Settings = _mySettings.PubSub,
#endif
                Now = DateTime.Now
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] object data)
        {
            Console.WriteLine($"Received data: {data}");
            return Ok();
        }
    }
}
