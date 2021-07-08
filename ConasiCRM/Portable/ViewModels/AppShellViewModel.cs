using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    class AppShellViewModel
    {
        public ICommand LogoutCommand { get; }
        public ICommand Develop { get; }

        public AppShellViewModel()
        {
            LogoutCommand = new Command(Logout);
            Develop = new Command(Developing);
        }

        private void Logout()
        {
            Application.Current.MainPage = new NavigationPage(new Login());
        }

        private async void Developing()
        {
            await Application.Current.MainPage.DisplayAlert("Thông báo", "Chức năng đang được phát triển", "Đóng");
        }
    }
}
