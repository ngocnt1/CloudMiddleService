using CQDT.CloudClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace CloudMiddleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectController : ControllerBase
    {
        private readonly ILogger<ConnectController> _logger;
        private readonly Settings _mySettings;
        public ConnectController(ILogger<ConnectController> logger, IOptions<Settings> mySettings)
        {
            _logger = logger;
            _mySettings = mySettings.Value;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string query)
        {
            _logger.LogInformation($"GET: {query}");
            HttpHelper.AgentName = _mySettings.AgentName;
            var authToken = Request.Headers.Authorization.FirstOrDefault() ?? _mySettings.AuthToken;
            var response = await query.GetAsJson<object>(authToken, _mySettings.NodeURL);
            var content = response != null ? JsonConvert.SerializeObject(response) : string.Empty;
            return Content(content, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostModel data)
        {
            if (data.Body != null)
            {
               
                    _logger.LogInformation($"POST: {data.ApiMethod}");
                    HttpHelper.AgentName = _mySettings.AgentName;
                    var authToken = Request.Headers.Authorization.FirstOrDefault() ?? _mySettings.AuthToken;
                    var response = await data.Body.PostAsJson(data.ApiMethod, authToken, _mySettings.NodeURL);
                    var responseContent = JsonConvert.SerializeObject(response);
                    return Content(responseContent, "application/json");
                
            }
            else
            {
                return Content("{'message':'body null'}", "application/json");
            }
        }
    }
}
