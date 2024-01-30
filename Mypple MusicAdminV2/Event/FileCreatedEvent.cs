using Prism.Events;

namespace Mypple_MusicAdminV2.Event
{
    public class FileCreatedEvent : PubSubEvent<FileCreatedModel>
    {

    }
    public record FileCreatedModel(string[] MusicsPath);
}
