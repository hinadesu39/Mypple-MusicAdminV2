using Mypple_MusicAdminV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class UploaderService 
    {
        private readonly HttpRestClient client;
        public UploaderService(HttpRestClient client)
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

        public async Task<FileExistsResponse> FileExists(long fileSize, string sha256Hash)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"/FileService/api/Uploader/FileExists?fileSize={fileSize}&sha256Hash={sha256Hash}";
            var res = await client.ExecuteAsync<FileExistsResponse>(request);
            return res;
        }
    }
}
