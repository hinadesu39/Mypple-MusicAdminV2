using Mypple_MusicAdminV2.Model;
using Mypple_MusicAdminV2.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class AlbumAdminService : BaseService<Album>
    {
        private readonly HttpRestClient client;

        public AlbumAdminService(HttpRestClient client) : base(client, "/Music.Admin/api/Albums")
        {
            this.client = client;
        }
        
    }
}
