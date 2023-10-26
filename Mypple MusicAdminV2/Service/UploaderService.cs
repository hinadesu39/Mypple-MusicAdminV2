using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class UploaderService : BaseService
    {
        private readonly HttpRestClient client;
        public UploaderService(HttpRestClient client) : base(client, "/FileService")
        {
            this.client = client;
        }
        public async Task<Uri> UploadAsync(string url)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"/FileService/api/Uploader/Upload";
            request.Parameter = url;
            var res = await client.UploadAsync(request);
            return res;
        }
    }
}
