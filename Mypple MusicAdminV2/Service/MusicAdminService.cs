using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class MusicAdminService : BaseService
    {
        private readonly HttpRestClient client;

        public MusicAdminService(HttpRestClient client) : base(client, "/Music.Admin")
        {
            this.client = client;
        }

        public async Task<Guid> AddAsync(MusicAddRequest entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"/Music.Admin/api/Musics/Add";
            request.Parameter = entity;
            var res = await client.ExecuteAsync<Guid>(request);
            return res;
        }
    }
}
