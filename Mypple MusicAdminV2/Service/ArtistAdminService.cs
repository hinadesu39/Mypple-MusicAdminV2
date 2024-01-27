using MusicAdminV2.Model;
using Mypple_MusicAdminV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class ArtistAdminService : BaseService<Artist>
    {
        private readonly HttpRestClient client;

        public ArtistAdminService(HttpRestClient client) : base(client, "/Music.Admin/api/Artists")
        {
            this.client = client;
        }
    }
}
