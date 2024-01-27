using Mypple_MusicAdminV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class UserManageService : BaseService<SimpleUser>
    {
        private readonly HttpRestClient client;

        public UserManageService(HttpRestClient client) : base(client, "/IdentityService")
        {
            this.client = client;
        }

        public async Task<SimpleUser[]> FindAllUsers()
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"{serviceName}/api/Admin/FindAllUsers";
            var res = await client.ExecuteAsync<SimpleUser[]>(request);
            return res;
        }

        public async Task<string> DeleteUser(Guid id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Delete;
            request.Route = $"{serviceName}/api/Admin/DeleteUser/{id}";
            var res = await client.ExecuteAsync<string>(request);
            return res;
        }

        public async Task<string> UpdateUser(SimpleUser user)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Put;
            request.Parameter = user;
            request.Route = $"{serviceName}/api/Admin/UpdateUser/{user.Id}";
            var res = await client.ExecuteAsync<string>(request);
            return res;
        }
    }
}
