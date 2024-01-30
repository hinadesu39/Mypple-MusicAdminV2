using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Model.Request
{
    public record MusicUpdateRequest(
     Guid Id,
     Uri AudioUrl,
     Uri? MusicPicUrl,
     string Title,
     Guid ArtistId,
     string Artist,
     Guid AlbumId,
     string Album,
     string? Type,
     string? Lyric,
     int PublishTime
 );
}
