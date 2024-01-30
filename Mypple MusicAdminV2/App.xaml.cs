using DryIoc;
using Mypple_Music.Extensions;
using Mypple_Music.ViewModels.Dialogs;
using Mypple_Music.Views.Dialogs;
using Mypple_MusicAdminV2.Service;
using Mypple_MusicAdminV2.View;
using Mypple_MusicAdminV2.View.Dialog;
using Mypple_MusicAdminV2.ViewModel;
using Mypple_MusicAdminV2.ViewModel.Dialog;
using Prism.DryIoc;
using Prism.Ioc;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;


namespace Mypple_MusicAdminV2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost", serviceKey: "webUrl");

            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.Register<UploaderService>();
            containerRegistry.Register<MusicAdminService>();
            containerRegistry.Register<UserManageService>();
            containerRegistry.Register<AlbumAdminService>();
            containerRegistry.Register<ArtistAdminService>();

            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<MusicUploadView, MusicUploadViewModel>();
            containerRegistry.RegisterForNavigation<MusicManageView, MusicManageViewModel>();
            containerRegistry.RegisterForNavigation<UserManageView, UserManageViewModel>();
            containerRegistry.RegisterForNavigation<QuestionView, QuestionViewModel>();
            containerRegistry.RegisterForNavigation<AlbumManageView,AlbumManageViewModel>();
            containerRegistry.RegisterForNavigation<ArtistManageView, ArtistManageViewModel>();
            containerRegistry.RegisterForNavigation<SelectAlbumView, SelectAlbumViewModel>();
            containerRegistry.RegisterForNavigation<SelectArtistView, SelectArtistViewModel>();

        }

        private void PrismApplication_Exit(object sender, ExitEventArgs e)
        {
            try
            {
               
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex);
            }

        }
    }
}
