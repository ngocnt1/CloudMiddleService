using CloudMiddleService.Controllers;
using CQDT.CloudClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CloudMiddleService.Services
{
    public class ServiceA : BackgroundService
    {
        private readonly ILogger<ConnectController> _logger;
        private readonly Settings _mySettings;
        bool processing = false;
        public ServiceA(ILogger<ConnectController> logger, IOptions<Settings> mySettings)
        {

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await PubSubHelper.Initialize(authToken: _mySettings.AuthToken);

            //Subscribe kênh nhận dữ liệu
            var topic = "";
            var callbackApi = "";
            PubSubHelper.Subcribe(topic,
              async (sub, token) =>
              {
                  if (!processing)
                  {
                      try
                      {
                          processing = true;
                          var ketqua = await $"{callbackApi}?msgToken={token}".GetAsJson<object>(_mySettings.AuthToken);

                          var json = ketqua == null ? $"invalid {token}" : JsonConvert.SerializeObject(ketqua);

                          //Lưu ý:kết quả sẽ được giải phóng khỏi DS chờ sau khi kéo thành công, 
                          //do đó nên lưu lại trước khi xử lý các nghiệp vụ nội bộ hoặc thực hiện các bước xl ở đây

                          //--save to DB or log
                          _logger.LogInformation(json);

                          //Hoặc gửi tới dữ liệu thu được API nội bộ khác
                          /*
                          if (ketqua != null)
                          {
                              var apiUrl = "";
                              var apiSecretAuth = "";
                              var res = await ketqua.PostJson(apiUrl, apiSecretAuth);
                          }
                          */

                          Console.WriteLine($"Raised {sub} with result: {json}");
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
