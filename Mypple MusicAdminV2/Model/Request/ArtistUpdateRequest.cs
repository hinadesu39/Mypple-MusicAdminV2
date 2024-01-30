using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Model.Request
{
    public record ArtistUpdateRequest(Guid id, Uri? PicUrl, string Name);
}
