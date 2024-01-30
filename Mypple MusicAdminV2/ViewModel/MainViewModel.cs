using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;


namespace Mypple_MusicAdminV2.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private IRegionManager regionManager;

        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand InitCommand { get; set; }

        public MainViewModel() { }

        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            InitCommand = new DelegateCommand(Init);
        }

        private void Init()
        {
            regionManager.Regions["MainView"].RequestNavigate("MusicUploadView");
        }

        private void Navigate(string obj)
        {
            regionManager.Regions["MainView"].RequestNavigate(obj);
        }
    }
}
