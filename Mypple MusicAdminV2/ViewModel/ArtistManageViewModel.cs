using CommonHelper;
using Microsoft.Win32;
using Mypple_Music.Extensions;
using Mypple_MusicAdminV2.Extension;
using Mypple_MusicAdminV2.Model;
using Mypple_MusicAdminV2.Model.Request;
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

        public DelegateCommand<Artist> DeleteCommand { get; set; }

        public DelegateCommand<Artist> SaveCommand { get; set; }

        public DelegateCommand<Artist> ChangeAvatarCommand { get; set; }

        public DelegateCommand RefrashCommand { get; set; }

        public DelegateCommand AddCommand { get; set; }

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
            DeleteCommand = new DelegateCommand<Artist>(Delete);
            SaveCommand = new DelegateCommand<Artist>(Save);
            ChangeAvatarCommand = new DelegateCommand<Artist>(ChangeAvatar);
            RefrashCommand = new DelegateCommand(Init);
            AddCommand = new DelegateCommand(Add);
            Init();
        }


        #endregion

        #region Command

        private void Add()
        {
            ArtistList.Add(new Artist() { Name = "none" });
        }

        private async void ChangeAvatar(Artist artist)
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
                artist.PicUrl = await uploaderService.UploadAsync(filePath);
            }
            else
            {
                artist.PicUrl = picExist.Url;
            }
        }

        private async void Save(Artist artist)
        {
            if(artist.Name == "none" || artist.Name == string.Empty)
            {
                eventAggregator.SendMessage($"请补充信息");
                return;
            }
            var dialogRes = await dialogHostService.Question("温馨提示", $"确定要保存吗，该操作不可撤回!!");
            if (dialogRes.Result == ButtonResult.OK)
            {
                string res;
                if (Count == ArtistList.Count())
                {
                    //Save Artist
                    res = await artistAdminService.UpdateAsync(new ArtistUpdateRequest(artist.Id, artist.PicUrl, artist.Name));
                }
                else
                {
                    //Add Artist
                    res = await artistAdminService.AddAsync(new ArtistAddRequest(artist.PicUrl, artist.Name));
                }
                eventAggregator.SendMessage($"{res}");
            }
        }

        private async void Delete(Artist artist)
        {
            var dialogRes = await dialogHostService.Question("警告", $"将会永久删除该艺人(真的很久!!)");
            if (dialogRes.Result == ButtonResult.OK)
            {
                if (artist.Id != Guid.Empty)
                {
                    var res = await artistAdminService.DeleteByIdAsync(artist.Id);
                    if (res == "Ok")
                    {
                        ArtistList.Remove(artist);
                    }
                    eventAggregator.SendMessage($"{res}");
                }
                else
                {
                    ArtistList.Remove(artist);
                }
            }
        }

        public async void Init()
        {
            ArtistList = new ObservableCollection<Artist>(await artistAdminService.GetAllAsync());
            Count = ArtistList.Count();
        }

        #endregion
    }
}

