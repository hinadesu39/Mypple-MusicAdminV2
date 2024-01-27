using CommonHelper;
using Microsoft.Win32;
using Mypple_Music.Extensions;
using Mypple_MusicAdminV2.Extension;
using Mypple_MusicAdminV2.Model;
using Mypple_MusicAdminV2.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.ViewModel
{
    public class ArtistManageViewModel : BindableBase
    {
        #region Field

        private readonly IDialogHostService dialogHostService;
        private readonly IEventAggregator eventAggregator;
        private readonly ArtistAdminService artistAdminService;
        private readonly UploaderService uploaderService;

        #endregion

        #region Property

        public DelegateCommand<SimpleUser> DeleteCommand { get; set; }

        public DelegateCommand<SimpleUser> SaveCommand { get; set; }

        public DelegateCommand<SimpleUser> ChangeAvatarCommand { get; set; }

        public DelegateCommand RefrashCommand { get; set; }

        private ObservableCollection<Artist> artistList;

        public ObservableCollection<Artist> ArtistList
        {
            get { return artistList; }
            set
            {
                artistList = value;
                RaisePropertyChanged();
            }
        }

        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Ctor
        public ArtistManageViewModel(
            ArtistAdminService artistAdminService,
            UploaderService uploaderService,
            IDialogHostService dialogHostService,
            IEventAggregator eventAggregator
        )
        {
            this.eventAggregator = eventAggregator;
            this.artistAdminService = artistAdminService;
            this.dialogHostService = dialogHostService;
            this.uploaderService = uploaderService;
            DeleteCommand = new DelegateCommand<SimpleUser>(Delete);
            SaveCommand = new DelegateCommand<SimpleUser>(Save);
            ChangeAvatarCommand = new DelegateCommand<SimpleUser>(ChangeAvatar);
            RefrashCommand = new DelegateCommand(Init);
            Init();
        }

        #endregion

        #region Command

        private async void ChangeAvatar(SimpleUser simpleUser)
        {
            // 创建一个 OpenFileDialog 的实例
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // 设置初始目录为 C:\
            openFileDialog.InitialDirectory = "C:\\";
            // 设置过滤器为图片文件
            openFileDialog.Filter =
                "Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";
            // 设置标题为 Select an image file
            openFileDialog.Title = "Select an image file";
            // 显示对话框，并判断返回值
            openFileDialog.ShowDialog();
            // 获取用户选择的文件路径
            string filePath = openFileDialog.FileName;
            if (filePath == string.Empty)
                return;

            var fileInfo = new FileInfo(filePath);
            var fileStream = File.Open(filePath, FileMode.Open);
            var picExist = await uploaderService.FileExists(
                fileInfo.Length,
                HashHelper.ComputeSha256Hash(fileStream)
            );
            fileStream.Close();
            if (picExist.IsExists == false)
            {
                simpleUser.UserAvatar = await uploaderService.UploadAsync(filePath);
            }
            else
            {
                simpleUser.UserAvatar = picExist.Url;
            }
        }

        private async void Save(SimpleUser simpleUser)
        {
            var dialogRes = await dialogHostService.Question("温馨提示", $"确定要保存吗，该操作不可撤回!!");
            if (dialogRes.Result == ButtonResult.OK)
            {

            }
        }

        private async void Delete(SimpleUser simpleUser)
        {
            var dialogRes = await dialogHostService.Question("警告", $"将会永久删除该用户(真的很久!!)");

        }

        public async void Init()
        {
            ArtistList = new ObservableCollection<Artist>(await artistAdminService.GetAllAsync());
            Count = ArtistList.Count();
        }
        #endregion
    }
}

