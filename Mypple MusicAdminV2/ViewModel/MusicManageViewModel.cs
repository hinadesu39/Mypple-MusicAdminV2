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
using TagLib.Id3v2;

namespace Mypple_MusicAdminV2.ViewModel
{
    public class MusicManageViewModel : BindableBase
    {
        #region Field

        private readonly IDialogHostService dialogHostService;
        private readonly IEventAggregator eventAggregator;
        private readonly MusicAdminService musicAdminService;
        private readonly UploaderService uploaderService;

        #endregion

        #region Property

        public DelegateCommand<Music> DeleteCommand { get; set; }

        public DelegateCommand<Music> SaveCommand { get; set; }

        public DelegateCommand<Music> ChangeAvatarCommand { get; set; }

        public DelegateCommand RefrashCommand { get; set; }

        public DelegateCommand<Music> SelectArtistCommand { get; set; }

        public DelegateCommand<Music> SelectAlbumCommand { get; set; }

        private ObservableCollection<Music> musicList;

        public ObservableCollection<Music> MusicList
        {
            get { return musicList; }
            set
            {
                musicList = value;
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
        public MusicManageViewModel(
            MusicAdminService musicAdminService,
            UploaderService uploaderService,
            IDialogHostService dialogHostService,
            IEventAggregator eventAggregator
        )
        {
            this.eventAggregator = eventAggregator;
            this.musicAdminService = musicAdminService;
            this.dialogHostService = dialogHostService;
            this.uploaderService = uploaderService;
            DeleteCommand = new DelegateCommand<Music>(Delete);
            SaveCommand = new DelegateCommand<Music>(Save);
            ChangeAvatarCommand = new DelegateCommand<Music>(ChangeAvatar);
            RefrashCommand = new DelegateCommand(Init);
            SelectArtistCommand = new DelegateCommand<Music>(SelectArtist);
            SelectAlbumCommand = new DelegateCommand<Music>(SelectAlbum);
            Init();
        }

        #endregion

        #region Command

        private async void SelectAlbum(Music music)
        {
            var dialogRes = await dialogHostService.ShowDialogAsync("SelectArtistView", null);
            if (dialogRes.Result == ButtonResult.OK && dialogRes.Parameters.ContainsKey("Album"))
            {
                var album = dialogRes.Parameters.GetValue<Album>("Album");
                music.AlbumId = album.Id;
                music.Album = album.Title;
            }
        }

        private async void SelectArtist(Music music)
        {
            var dialogRes = await dialogHostService.ShowDialogAsync("SelectArtistView", null);
            if (dialogRes.Result == ButtonResult.OK && dialogRes.Parameters.ContainsKey("Artist"))
            {
                var artist = dialogRes.Parameters.GetValue<Artist>("Artist");
                music.Artist = artist.Name;
                music.ArtistId = artist.Id;
            }
        }

        private async void ChangeAvatar(Music music)
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
                music.PicUrl = await uploaderService.UploadAsync(filePath);
            }
            else
            {
                music.PicUrl = picExist.Url;
            }
        }

        private async void Save(Music music)
        {
            if (
                music.Title == string.Empty
                || music.Album == string.Empty
                || music.Artist == string.Empty
                || music.PublishTime == 0
            )
            {
                eventAggregator.SendMessage($"请补充信息");
                return;
            }

            var dialogRes = await dialogHostService.Question("温馨提示", $"确定要保存吗，该操作不可撤回!!");
            if (dialogRes.Result == ButtonResult.OK)
            {
                //Save Album
                var res = await musicAdminService.UpdateAsync(
                    new MusicUpdateRequest(
                        music.Id,
                        music.AudioUrl,
                        music.PicUrl,
                        music.Title,
                        music.ArtistId,
                        music.Artist,
                        music.AlbumId,
                        music.Album,
                        music.Type,
                        music.Lyric,
                        music.PublishTime
                    )
                );
            }
        }

        private async void Delete(Music music)
        {
            var dialogRes = await dialogHostService.Question("警告", $"将会永久删除该歌曲(真的很久!!)");
            if (dialogRes.Result == ButtonResult.OK)
            {
                var res = await musicAdminService.DeleteByIdAsync(music.Id);
                if (res == "Ok")
                {
                    MusicList.Remove(music);
                }
                eventAggregator.SendMessage($"{res}");
            }
        }

        public async void Init()
        {
            var musics = await musicAdminService.GetAllAsync();
            if (musics != null)
            {
                MusicList = new ObservableCollection<Music>(await musicAdminService.GetAllAsync());
                Count = MusicList.Count();
            }

        }
        #endregion
    }
}
