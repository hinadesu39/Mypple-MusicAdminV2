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
    public class SelectArtistViewModel : BindableBase, IDialogHostAware
    {
        #region Field
        private readonly ArtistAdminService artistAdminService;
        private ObservableCollection<Artist> tempArtists;
        #endregion

        #region Property
        public string DialogHostName { get; set; }
        public DelegateCommand<string> SearchCommand { get; set; }
        public DelegateCommand TextEmptyCommand { get; set; }
        public DelegateCommand<Artist> SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        DelegateCommand IDialogHostAware.SaveCommand
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

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

        #endregion

        #region Ctor
        public SelectArtistViewModel(ArtistAdminService artistAdminService)
        {
            this.artistAdminService = artistAdminService;
            SearchCommand = new DelegateCommand<string>(Search);
            TextEmptyCommand = new DelegateCommand(TextEmpty);
            SaveCommand = new DelegateCommand<Artist>(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }
        #endregion

        #region Command
        private void TextEmpty()
        {
            if (tempArtists != null)
            {
                ArtistList = tempArtists;
            }
        }

        private void Search(string obj)
        {
            var searchedArtistList = ArtistList.Where(
                m => m.Name.Contains(obj, StringComparison.InvariantCultureIgnoreCase)
            );
            ArtistList = new ObservableCollection<Artist>(searchedArtistList);
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }

        private void Save(Artist artist)
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters para = new DialogParameters();
                para.Add("Artist", artist);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, para));
            }
        }

        public async Task OnDialogOpendAsync(IDialogParameters parameters)
        {
            ArtistList = new ObservableCollection<Artist>(await artistAdminService.GetAllAsync());
            tempArtists = ArtistList;
        }

        #endregion
    }
}
