using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Model
{
    public record FileExistsResponse(bool IsExists, Uri? Url);
}
