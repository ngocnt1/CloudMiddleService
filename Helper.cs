using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CloudMiddleService
{
    public static class Helper
    {
        public static string? AuthorizationToken(this StringValues authorization)
        {
            return authorization.Count > 0 ? authorization[0] : string.Empty;
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        /// <summary>
        /// Post as json body
        /// </summary>
        /// <param name="data"></param>
        /// <param name="apiMethod"></param>
        /// <param name="nodeUrl">Url of node server or empty</param>
        /// <param name="authorization"></param>
        /// <param name="autoOptimize">Auto detect to optimize a big text request, don't set true if content is base64</param>
        /// <returns></returns>
        public static async Task<string> PostJson(this object data,
            string url,
            string authorization
            )
        {
            Uri uri = new Uri(url);

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = uri;
                if (!string.IsNullOrEmpty(authorization))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", authorization);
                }

                //  httpClient.Timeout = TimeSpan.FromSeconds(MaxTimeout);

                var rawString = data is string ? (string)data : JsonConvert.SerializeObject(data);

                var content = new StringContent(
                    rawString,
                    Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync(uri,
                    content
                  );

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var res = await result.Content.ReadAsStringAsync();
                    return res;//string.IsNullOrWhiteSpace(res) ? null : JsonConvert.DeserializeObject(res);
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized || result.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new UnauthorizedAccessException("Invalid license or expired");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException(await result.Content.ReadAsStringAsync());
                }
                else if (result.StatusCode == HttpStatusCode.BadGateway)
                {
                    throw new Exception("Could not connect to server, please check your network connection then try again.");
                }
                else
                {
                    var errorContent = await result.Content.ReadAsStringAsync();

                    throw new HttpRequestException(errorContent);

                }
            }


        }

    }
}
