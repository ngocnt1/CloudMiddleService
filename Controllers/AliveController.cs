using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
                Settings = _mySettings.Pubsubs,
                Now = DateTime.Now
            };
        }
    }
}
