using Mypple_MusicAdminV2.Event;
using Mypple_MusicAdminV2.Extension;
using Prism.Events;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Documents;

namespace Mypple_MusicAdminV2.View
{
    /// <summary>
    /// MainVeiw.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IEventAggregator eventAggregator;

        public MainView(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            InitializeComponent();
            //注册消息通知
            eventAggregator.RegisterMessage(arg =>
            {
                Snackbar.MessageQueue.Enqueue(arg.Message);
            });
        }

        
    }
}
