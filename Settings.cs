using HttpMethods= Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace CloudMiddleService
{
    public class Settings
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HttpsForce { get; set; }
        /// <summary>
        /// Authentication token
        /// </summary>
        public string AuthToken { get; set; }

        public string NodeURL { get; set; }

        public string AgentName { get; set; }

        public int IntervalSeconds { get; set; }

        public PubsubSetting Pubsubs { get; set; }
    }

    public class PubsubSetting
    {
        public bool Enabled { get; set; }
        public PubsubConfig[] Requests { get; set; }
    }

    public class PubsubConfig {
       
        public string Topic { get; set; }
        public string ExecuteUrl { get; set; }
        /// <summary>
        /// 0: Get; 1: PUT,2:Delete, 3:Post
        /// </summary>
        public HttpMethods  ExecuteMethod { get; set; }
        public string CallbackUrl { get; set; }
        public HttpMethods CallbackMethod { get; set; }
    }

    public class PostModel
    {
        public string Url { get; set; }
        public string ApiMethod { get; set; }
        public object? Body { get; set; }

        public PostModel()
        {
            Url = string.Empty;
            ApiMethod = string.Empty;
            Body = null;
        }
    }
}
