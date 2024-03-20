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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.ViewModel
{
    public class AlbumManageViewModel : BindableBase
    {
        #region Field

        private readonly IDialogHostService dialogHostService;
        private readonly IEventAggregator eventAggregator;
        private readonly AlbumAdminService albumAdminService;
        private readonly UploaderService uploaderService;

        #endregion

        #region Property

        public DelegateCommand AddCommand { get; set; }

        public DelegateCommand<Album> DeleteCommand { get; set; }

        public DelegateCommand<Album> SaveCommand { get; set; }

        public DelegateCommand<Album> ChangeAvatarCommand { get; set; }

        public DelegateCommand RefrashCommand { get; set; }

        public DelegateCommand<Album> SelectArtistCommand { get; set; }

        private ObservableCollection<Album> albumList;

        public ObservableCollection<Album> AlbumList
        {
            get { return albumList; }
            set
            {
                albumList = value;
                RaisePropertyChanged();
            }
        }

        private int count;

        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Ctor
        public AlbumManageViewModel(
            AlbumAdminService albumAdminService,
            UploaderService uploaderService,
            IDialogHostService dialogHostService,
            IEventAggregator eventAggregator
        )
        {
            this.eventAggregator = eventAggregator;
            this.albumAdminService = albumAdminService;
            this.dialogHostService = dialogHostService;
            this.uploaderService = uploaderService;
            DeleteCommand = new DelegateCommand<Album>(Delete);
            SaveCommand = new DelegateCommand<Album>(Save);
            ChangeAvatarCommand = new DelegateCommand<Album>(ChangeAvatar);
            RefrashCommand = new DelegateCommand(Init);
            SelectArtistCommand = new DelegateCommand<Album>(SelectArtist);
            AddCommand = new DelegateCommand(Add);
            Init();
        }

        #endregion

        #region Command

        private void Add()
        {
            AlbumList.Add(new Album() { Title = "none" });
        }

        private async void SelectArtist(Album album)
        {
            var dialogRes = await dialogHostService.ShowDialogAsync("SelectArtistView", null);
            if (dialogRes.Result == ButtonResult.OK && dialogRes.Parameters.ContainsKey("Artist"))
            {
                var artist = dialogRes.Parameters.GetValue<Artist>("Artist");
                album.Artist = artist.Name;
                album.ArtistId = artist.Id;
            }
        }

        private async void ChangeAvatar(Album album)
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
                album.PicUrl = await uploaderService.UploadAsync(filePath);
            }
            else
            {
                album.PicUrl = picExist.Url;
            }
        }

        private async void Save(Album album)
        {
            if (album.Title == "none" || album.Title == string.Empty || album.Artist == null || album.PublishTime == 0)
            {
                eventAggregator.SendMessage($"请补充信息");
                return;
            }
            var dialogRes = await dialogHostService.Question("温馨提示", $"确定要保存吗，该操作不可撤回!!");
            if (dialogRes.Result == ButtonResult.OK)
            {
                string res;
                if (Count == AlbumList.Count())
                {
                    //Save Album
                    res = await albumAdminService.UpdateAsync(
                        new AlbumUpdateRequest(
                            album.Id,
                            album.PicUrl,
                            album.Title,
                            album.ArtistId,
                            album.Artist,
                            album.Type,
                            album.PublishTime
                        )
                    );
                }
                else
                {
                    //Add Album
                    res = await albumAdminService.AddAsync(
                        new AlbumAddRequest(
                            album.PicUrl,
                            album.Title,
                            album.ArtistId,
                            album.Artist,
                            album.Type,
                            album.PublishTime
                        )
                    );
                }
                eventAggregator.SendMessage($"{res}");
            }
        }

        private async void Delete(Album album)
        {
            var dialogRes = await dialogHostService.Question("警告", $"将会永久删除该专辑(真的很久!!)");
            if (dialogRes.Result == ButtonResult.OK)
            {
                if (album.Id != Guid.Empty)
                {
                    var res = await albumAdminService.DeleteByIdAsync(album.Id);
                    if (res == "Ok")
                    {
                        AlbumList.Remove(album);
                    }
                    eventAggregator.SendMessage($"{res}");
                }
                else
                {
                    AlbumList.Remove(album);
                }
            }
        }

        public async void Init()
        {
            var albums = await albumAdminService.GetAllAsync();
            if (albums != null)
            {
                AlbumList = new ObservableCollection<Album>(albums);
                Count = AlbumList.Count();
            }

        }
        #endregion
    }
}
