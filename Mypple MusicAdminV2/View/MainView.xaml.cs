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

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            // 获取拖入的文件路径
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


            //// 遍历每个文件
            //foreach (string file in files)
            //{
            //    // 获取文件信息
            //    FileInfo fileInfo = new FileInfo(file);

            //    // 创建一个段落
            //    Paragraph paragraph = new Paragraph();

            //    // 添加文件名、大小、扩展名等信息到段落中
            //    paragraph.Inlines.Add($"文件名：{fileInfo.Name}\n");
            //    paragraph.Inlines.Add($"文件大小：{fileInfo.Length}字节\n");
            //    paragraph.Inlines.Add($"文件扩展名：{fileInfo.Extension}\n");
            //    paragraph.Inlines.Add($"文件创建时间：{fileInfo.CreationTime}\n");
            //    paragraph.Inlines.Add($"文件修改时间：{fileInfo.LastWriteTime}\n");

            //    // 添加段落到RichTextBox控件中
            //    lyricBox.Text = paragraph.ToString();

            //}
            eventAggregator.GetEvent<FileCreatedEvent>().Publish(new FileCreatedModel(files));
        }
    }
}
