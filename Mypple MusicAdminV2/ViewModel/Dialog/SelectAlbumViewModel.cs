using MaterialDesignThemes.Wpf;
using Mypple_Music.Extensions;
using Mypple_MusicAdminV2.Model;
using Mypple_MusicAdminV2.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.ViewModel.Dialog
{
    public class SelectAlbumViewModel : BindableBase, IDialogHostAware
    {
        #region Field
        private readonly AlbumAdminService albumAdminService;
        private ObservableCollection<Album> tempAlbums;
        #endregion

        #region Property
        public string DialogHostName { get; set; }
        public DelegateCommand<string> SearchCommand { get; set; }
        public DelegateCommand TextEmptyCommand { get; set; }
        public DelegateCommand<Album> SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        DelegateCommand IDialogHostAware.SaveCommand
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

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

        #endregion

        #region Ctor
        public SelectAlbumViewModel(AlbumAdminService albumAdminService)
        {
            this.albumAdminService = albumAdminService;
            SearchCommand = new DelegateCommand<string>(Search);
            TextEmptyCommand = new DelegateCommand(TextEmpty);
            SaveCommand = new DelegateCommand<Album>(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }
        #endregion

        #region Command
        private void TextEmpty()
        {
            if (tempAlbums != null)
            {
                AlbumList = tempAlbums;
            }
        }

        private void Search(string obj)
        {
            var searchedAlbumList = AlbumList.Where(
                m => m.Title.Contains(obj, StringComparison.InvariantCultureIgnoreCase)
            );
            AlbumList = new ObservableCollection<Album>(searchedAlbumList);
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }

        private void Save(Album album)
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters para = new DialogParameters();
                para.Add("Album", album);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, para));
            }
        }

        public async Task OnDialogOpendAsync(IDialogParameters parameters)
        {
            AlbumList = new ObservableCollection<Album>(await albumAdminService.GetAllAsync());
            tempAlbums = AlbumList;
        }

        #endregion
    }
}
