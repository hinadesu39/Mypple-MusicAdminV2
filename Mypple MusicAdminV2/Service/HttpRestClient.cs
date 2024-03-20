using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        private readonly ILogger logger;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl, ILogger logger)
        {
            this.apiUrl = apiUrl;
            this.logger = logger;
            client = new RestClient(apiUrl);
        }

        public async Task<Uri> UploadAsync(BaseRequest baseRequest)
        {
            var url = new Uri(apiUrl + baseRequest.Route);
            var request = new RestRequest(url, baseRequest.Method);
            request.AlwaysMultipartFormData = true;
            if (baseRequest.Parameter != null)
            {
                request.AddFile("File", baseRequest.Parameter.ToString());
            }

            try
            {
                RestResponse response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<Uri>(response.Content);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }
        }

        public async Task<T> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var url = new Uri(apiUrl + baseRequest.Route);
            var request = new RestRequest(url, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            request.AddHeader("Authorization", $"Bearer {AppSession.JwtToken}");
            if (baseRequest.Parameter != null)
            {
                string body = JsonConvert.SerializeObject(baseRequest.Parameter);
                request.AddStringBody(body, ContentType.Json);
            }

            try
            {
                RestResponse response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return default(T);
            }
        }
    }
}
