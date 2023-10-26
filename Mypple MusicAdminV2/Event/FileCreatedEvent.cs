using MusicAdminV2.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Event
{
    public class FileCreatedEvent : PubSubEvent<FileCreatedModel>
    {

    }
    public record FileCreatedModel(string[] MusicsPath);
}
