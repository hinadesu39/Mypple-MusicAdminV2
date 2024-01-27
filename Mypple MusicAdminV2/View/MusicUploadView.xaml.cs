using Mypple_MusicAdminV2.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mypple_MusicAdminV2.View
{
    /// <summary>
    /// MusicUploadView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicUploadView : UserControl
    {
        private readonly IEventAggregator eventAggregator;
        public MusicUploadView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            // 获取拖入的文件路径
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            eventAggregator.GetEvent<FileCreatedEvent>().Publish(new FileCreatedModel(files));
        }
    }
}
