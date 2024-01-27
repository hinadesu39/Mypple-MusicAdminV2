using CommonHelper;
using ImTools;
using MusicAdminV2.Model;
using Mypple_MusicAdminV2.Event;
using Mypple_MusicAdminV2.Extension;
using Mypple_MusicAdminV2.Model;
using Mypple_MusicAdminV2.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
