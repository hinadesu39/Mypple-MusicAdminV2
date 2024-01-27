using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class BaseService<TEntity>
    {
        private readonly HttpRestClient client;
        protected readonly string serviceName;

        public BaseService(HttpRestClient client, string ServiceName)
        {
            this.client = client;
            serviceName = ServiceName;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"{serviceName}/GetAll";
            var res = await client.ExecuteAsync<List<TEntity>>(request);
            return res;
        }

        public async Task<string> DeleteById(Guid id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Delete;
            request.Route = $"{serviceName}/DeleteById/{id}";
            var res = await client.ExecuteAsync<string>(request);
            return res;
        }
    }
}
