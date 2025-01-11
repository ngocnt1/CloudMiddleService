using CloudMiddleService.Controllers;
using CQDT.CloudClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Web;

namespace CloudMiddleService.Services
{
    public class SimpleServices : BackgroundService
    {        
        private readonly IServiceScopeFactory _scopeFactory;
        bool processing = false;
        private readonly IServiceScopeFactory scopeFactory;
        public SimpleServices(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            var _mySettings = scope.ServiceProvider.GetService<IOptions<Settings>>()?.Value;
            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<SimpleServices>>();
            //Subscribe kênh nhận dữ liệu
            if (_mySettings.PubSub?.Enabled == true 
                && _mySettings.AuthToken.IsNotNullOrEmpty()
                && _mySettings.PubSub?.Requests?.Length > 0)
            {
                await PubSubHelper.Initialize(authToken: _mySettings.AuthToken, _mySettings.NodeURL);

                foreach (var item in _mySettings.PubSub.Requests)
                {
                    if (item.Topic.IsNullOrEmpty()) continue;
                    var topic = item.Topic;
                    var execApi = item.ExecuteApi;

                    PubSubHelper.Subcribe(topic,
                      async (sub, msgToken) =>
                      {
                          if (!processing)
                          {
                              try
                              {
                                  processing = true;
                                  Console.WriteLine($"Raised {sub} with token: {HttpUtility.UrlDecode(msgToken)}");
                                  var ketqua = await $"{execApi}?msgToken={msgToken}".GetAsJson<object>(_mySettings.AuthToken, _mySettings.NodeURL);

                                  var json = ketqua == null ? $"invalid {msgToken}" : JsonConvert.SerializeObject(ketqua);

                                  //Lưu ý:kết quả sẽ được giải phóng khỏi DS chờ sau khi kéo thành công, 
                                  //do đó nên lưu lại trước khi xử lý các nghiệp vụ nội bộ hoặc thực hiện các bước xl ở đây
                                  
                                  //Gửi tới dữ liệu thu được API nội bộ strong appSettings                                                                    

                                  if (ketqua != null)
                                  {
                                      if (item.ForwardUrl.IsNotNullOrEmpty())
                                      {                                          
                                          var res = await ketqua.PostJson(item.ForwardUrl, item.ForwardAuthKey);
                                         _logger.LogInformation($"Forwarded {item.ForwardUrl} from {execApi} for {topic}: {json}");
                                      }
                                  }



                                 
                              }
                              catch (Exception ex)
                              {
                                  _logger.LogError(ex, ex.Message);
                              }
                              finally
                              {
                                  processing = false;
                              }
                          }
                          return true;
                      },
                      (other) =>
                      {
                          //Console.WriteLine($"Listenning...");
                          return true;
                      }
                  );
                }

            }
        }
    }
}
