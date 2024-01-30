using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Model.Request
{
    public record AlbumAddRequest(
         Uri? PicUrl,
         string Title,
         Guid ArtistId,
         string Artist,
         string? Type,
         int PublishTime
     );
}
