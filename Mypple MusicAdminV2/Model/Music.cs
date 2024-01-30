using Prism.Mvvm;
using System;

namespace Mypple_MusicAdminV2.Model
{
    public class Music : BindableBase
    {
        public Guid Id { get; set; }     

        /// <summary>
        /// 歌曲url
        /// </summary>
        private Uri audioUrl;
        public Uri AudioUrl
        {
            get { return audioUrl; }
            set
            {
                audioUrl = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 本地文件路径
        /// </summary>
        private string localAudioUrl;
        public string LocalAudioUrl
        {
            get { return localAudioUrl; }
            set
            {
                localAudioUrl = value;
                RaisePropertyChanged();

            }
        }

        /// <summary>
        /// 歌曲图片
        /// </summary>
        private Uri picUrl;
        public Uri PicUrl
        {
            get { return picUrl; }
            set
            {
                picUrl = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 本地下载后歌曲名片的路径
        /// </summary>
        private string localPicUrl;
        public string LocalPicUrl
        {
            get { return localPicUrl; }
            set
            {
                localPicUrl = value;
                RaisePropertyChanged() ;
            }
        }

        /// <summary>
        /// 歌曲名
        /// </summary>
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 歌曲时长
        /// </summary>

        private double duration;

        public double Duration
        {
            get { return duration; }
            set { duration = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 歌手
        /// </summary>
        private string artist;

        public string Artist
        {
            get { return artist; }
            set { artist = value; RaisePropertyChanged(); }
        }

        private Guid artistId;

        public Guid ArtistId
        {
            get { return artistId; }
            set { artistId = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 专辑
        /// </summary>
        private string album;

        public string Album
        {
            get { return album; }
            set { album = value; RaisePropertyChanged(); }
        }

        private Guid albumId;

        public Guid AlbumId
        {
            get { return albumId; }
            set { albumId = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 类型
        /// </summary>
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 歌词
        /// </summary>
        private string lyric;

        public string Lyric
        {
            get { return lyric; }
            set { lyric = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 发行时间
        /// </summary>
        private int publishTime;

        public int PublishTime
        {
            get { return publishTime; }
            set { publishTime = value; RaisePropertyChanged(); }
        }




    }
}
