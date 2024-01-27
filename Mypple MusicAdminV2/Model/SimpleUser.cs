using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input.StylusPlugIns;

namespace Mypple_MusicAdminV2.Model
{
    public class SimpleUser : BindableBase
    {
        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }


        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private string? gender;

        public string? Gender
        {
            get { return gender; }
            set { gender = value; RaisePropertyChanged(); }
        }

        private Uri? userAvatar;
        public Uri? UserAvatar
        {
            get { return userAvatar; }
            set
            {
                userAvatar = value;
                RaisePropertyChanged();
            }
        }

        private DateTime? birthDay;

        public DateTime? BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; RaisePropertyChanged(); }
        }


        private string? email;

        public string? Email
        {
            get { return email; }
            set { email = value; RaisePropertyChanged(); }
        }

        private string? phoneNumber;

        public string? PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; RaisePropertyChanged(); }
        }

        private bool isAdmin;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; RaisePropertyChanged(); }
        }

    }
}
