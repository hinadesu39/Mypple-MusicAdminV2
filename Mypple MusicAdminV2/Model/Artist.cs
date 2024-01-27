using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mypple_MusicAdminV2.Model
{
    public class Artist : BindableBase
    {
        /// <summary>
        /// 艺人id
        /// </summary>
        /// 
        public Guid Id { get; set; }

        /// <summary>
        /// 专辑图片
        /// </summary>

        private Uri? picUrl;

        public Uri? PicUrl
        {
            get { return picUrl; }
            set
            {
                picUrl = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 专辑名
        /// </summary>

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }
    }
}
