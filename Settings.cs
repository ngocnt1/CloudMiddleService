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

        public PubsubSetting PubSub { get; set; }
    }

    public class PubsubSetting
    {
        public bool Enabled { get; set; }
        public PubsubConfig[] Requests { get; set; }
    }

    public class PubsubConfig {
       
        public string Topic { get; set; }
        public string ExecuteApi { get; set; }        
        
        public string ForwardUrl { get; set; }
        public string ForwardAuthKey { get; set; }
    }

    public class PostModel
    {        
        public string ApiMethod { get; set; }
        public object? Body { get; set; }

        public PostModel()
        {            
            ApiMethod = string.Empty;
            Body = null;
        }
    }
}
