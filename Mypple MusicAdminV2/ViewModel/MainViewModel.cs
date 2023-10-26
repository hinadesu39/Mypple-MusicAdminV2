using MusicAdminV2.Model;
using Mypple_MusicAdminV2.Event;
using Mypple_MusicAdminV2.Extension;
using Mypple_MusicAdminV2.Model;
using Mypple_MusicAdminV2.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private string[] musicsPath;
        private readonly IContainerProvider containerProvider;
        public readonly IEventAggregator eventAggregator;
        private MusicAdminService musicAdminService;
        private UploaderService uploaderService;

        private bool isUploading;

        public bool IsUploading
        {
            get { return isUploading; }
            set
            {
                isUploading = value;
                RaisePropertyChanged();
            }
        }

        private bool isUploadingAll;

        public bool IsUploadingAll
        {
            get { return isUploadingAll; }
            set
            {
                isUploadingAll = value;
                RaisePropertyChanged();
            }
        }

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

        private Music selectedMusic;

        public Music SelectedMusic
        {
            get { return selectedMusic; }
            set
            {
                selectedMusic = value;
                RaisePropertyChanged();
            }
        }

        private string selectedMusicPicUrl;

        public string SelectedMusicPicUrl
        {
            get { return selectedMusicPicUrl; }
            set
            {
                selectedMusicPicUrl = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<IList> UploadCommand { get; set; }
        public DelegateCommand UploadAllCommand { get; set; }
        public DelegateCommand ClearAllCommand { get; set; }

        public MainViewModel(
            IContainerProvider containerProvider,
            UploaderService uploaderService,
            MusicAdminService musicAdminService
        )
        {
            this.containerProvider = containerProvider;
            this.uploaderService = uploaderService;
            this.musicAdminService = musicAdminService;
            eventAggregator = containerProvider.Resolve<IEventAggregator>();

            MusicList = new ObservableCollection<Music>();

            UploadCommand = new DelegateCommand<IList>(Upload);
            UploadAllCommand = new DelegateCommand(UploadAll);
            ClearAllCommand = new DelegateCommand(ClearAll);

            eventAggregator
                .GetEvent<FileCreatedEvent>()
                .Subscribe(arg =>
                {
                    musicsPath = arg.MusicsPath;
                    UpdateDateGrid();
                });
        }

        private void ClearAll()
        {
            MusicList.Clear();
        }

        private async void UploadAll()
        {
            foreach (Music music in MusicList)
            {
                try
                {
                    IsUploadingAll = true;
                    //上传文件
                    music.PicUrl = await uploaderService.UploadAsync(music.LocalPicUrl);                   
                    music.AudioUrl = await uploaderService.UploadAsync(music.LocalAudioUrl);
                    var res = await musicAdminService.AddAsync(
                        new MusicAddRequest(
                            music.AudioUrl,
                            music.PicUrl,
                            music.PicUrl,
                            music.PicUrl,
                            new MultilingualString(music.Title, ""),
                            music.Duration,
                            music.Artist,
                            music.Album,
                            music.Type,
                            music.Lyric,
                            music.PublishTime
                        )
                    );
                    Debug.WriteLine(res);
                    eventAggregator.SendMessage($"{music.Title} 上传成功！");
                    IsUploadingAll = false;
                }
                catch (Exception ex)
                {

                    eventAggregator.SendMessage($"{music.Title} 上传失败请稍后重试！");
                }
                
            }
        }

        private async void Upload(IList Musics)
        {

            foreach (Music music in Musics)
            {
                try
                {
                    IsUploading = true;
                    //上传文件
                    music.PicUrl = await uploaderService.UploadAsync(music.LocalPicUrl);                    
                    music.AudioUrl = await uploaderService.UploadAsync(music.LocalAudioUrl);
                    var res = await musicAdminService.AddAsync(
                        new MusicAddRequest(
                            music.AudioUrl,
                            music.PicUrl,
                            music.PicUrl,
                            music.PicUrl,
                            new MultilingualString(music.Title, ""),
                            music.Duration,
                            music.Artist,
                            music.Album,
                            music.Type,
                            music.Lyric,
                            music.PublishTime
                        )
                    );
                    Debug.WriteLine(res);
                    eventAggregator.SendMessage($"{music.Title} 上传成功！");
                    IsUploading = false;
                }
                catch (Exception)
                {

                    eventAggregator.SendMessage($"{music.Title} 上传失败请稍后重试！");
                }
               
            }
        }

        private async void UpdateDateGrid()
        {
            foreach (var path in musicsPath)
            {
                try
                {
                    Music musicToAdd = new Music();
                    musicToAdd.LocalAudioUrl = path;
                    TagLib.File music = TagLib.File.Create(path);
                    if (music.Tag.Title != null)
                        musicToAdd.Title = music.Tag.Title;
                    if (music.Tag.Album != null)
                        musicToAdd.Album = music.Tag.Album;
                    if (music.Tag.Performers.Length != 0)
                        musicToAdd.Artist = music.Tag.Performers[0];
                    if (music.Tag.FirstGenre != null)
                        musicToAdd.Type = music.Tag.FirstGenre;
                    if (music.Tag.Year != 0)
                        musicToAdd.PublishTime = (int)music.Tag.Year;
                    if (music.Properties.Duration.TotalSeconds != 0)
                        musicToAdd.Duration = music.Properties.Duration.TotalSeconds;
                    if (music.Tag.Lyrics != null)
                        musicToAdd.Lyric = music.Tag.Lyrics;
                    if (music.Tag.Pictures != null && music.Tag.Pictures.Length != 0)
                    {
                        var bin = music.Tag.Pictures[0].Data.Data;
                        //将 bin 保存为一个文件上传完成后自动删除
                        File.WriteAllBytes($@"temp/{musicToAdd.Title.Replace('/', '-')}.jpg", bin);
                        string absolute = Path.GetFullPath($@"temp/{musicToAdd.Title.Replace('/', '-')}.jpg");
                        musicToAdd.LocalPicUrl = absolute;
                    }
                    MusicList.Add(musicToAdd);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
               
            }
        }
    }
}
