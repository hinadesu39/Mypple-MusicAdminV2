using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Service
{
    public record MusicAddRequest(
       Uri AudioUrl,
       Uri? MusicPicUrl,
       string Title,
       double Duration,
       string Artist,
       string Album,
       string? Type,
       string? Lyric,
       int PublishTime

   );
}
