using DryIoc;
using Mypple_MusicAdminV2.Service;
using Mypple_MusicAdminV2.View;
using Mypple_MusicAdminV2.ViewModel;
using Prism.DryIoc;
using Prism.Ioc;
using System.Configuration;
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

            containerRegistry.Register<UploaderService>();
            containerRegistry.Register<MusicAdminService>();
            

            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();

        }

        private void PrismApplication_Exit(object sender, ExitEventArgs e)
        {
            File.Delete(@"/temp");
        }
    }
}
