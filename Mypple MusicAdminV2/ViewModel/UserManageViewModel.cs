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
using System.Windows.Documents;

namespace Mypple_MusicAdminV2.ViewModel
{
    public class UserManageViewModel : BindableBase
    {
        #region Field

        private readonly IDialogHostService dialogHostService;
        private readonly IEventAggregator eventAggregator;
        private readonly UserManageService userManageService;
        private readonly UploaderService uploaderService;

        #endregion

        #region Property

        public DelegateCommand<SimpleUser> DeleteCommand { get; set; }

        public DelegateCommand<SimpleUser> SaveCommand { get; set; }

        public DelegateCommand<SimpleUser> ChangeAvatarCommand { get; set; }

        private ObservableCollection<SimpleUser> users;

        public ObservableCollection<SimpleUser> Users
        {
            get { return users; }
            set
            {
                users = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Ctor
        public UserManageViewModel(
            UserManageService userManageService,
            UploaderService uploaderService,
            IDialogHostService dialogHostService,
            IEventAggregator eventAggregator
        )
        {
            this.eventAggregator = eventAggregator;
            this.userManageService = userManageService;
            this.dialogHostService = dialogHostService;
            this.uploaderService = uploaderService;
            DeleteCommand = new DelegateCommand<SimpleUser>(Delete);
            SaveCommand = new DelegateCommand<SimpleUser>(Save);
            ChangeAvatarCommand = new DelegateCommand<SimpleUser>(ChangeAvatar);
            FindAllUser();
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
                var res = await userManageService.UpdateUser(simpleUser);
                eventAggregator.SendMessage($"{res}");
            }
        }

        private async void Delete(SimpleUser simpleUser)
        {
            var dialogRes = await dialogHostService.Question("警告", $"将会永久删除该用户(真的很久!!)");
            if (dialogRes.Result == ButtonResult.OK)
            {
                var res = await userManageService.DeleteUser(simpleUser.Id);
                if (res == "Ok")
                {
                    Users.Remove(simpleUser);
                }
                eventAggregator.SendMessage($"{res}");
            }
        }

        public async Task FindAllUser()
        {
            var users = await userManageService.FindAllUsers();
            if (users != null)
                Users = new ObservableCollection<SimpleUser>(users);
        }
        #endregion
    }
}
