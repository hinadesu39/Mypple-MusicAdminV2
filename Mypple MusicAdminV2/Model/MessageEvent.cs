using Prism.Events;

namespace Mypple_MusicAdminV2.Model
{
    /// <summary>
    /// 消息事件
    /// </summary>
    public class MessageModel
    {
        public string Filter { set; get; }
        public string Message { set; get; }
    }

    public class MessageEvent : PubSubEvent<MessageModel>
    {

    }
}
