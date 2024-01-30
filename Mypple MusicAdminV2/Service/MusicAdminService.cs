using Mypple_MusicAdminV2.Model;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public class MusicAdminService : BaseService<Music>
    {
        private readonly HttpRestClient client;

        public MusicAdminService(HttpRestClient client) : base(client, "/Music.Admin/api/Musics")
        {
            this.client = client;
        }     

    }
}
