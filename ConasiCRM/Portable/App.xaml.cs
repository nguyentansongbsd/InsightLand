using ConasiCRM.Portable.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            App.Current.MainPage.Navigation.PushModalAsync(new Login());
            //MainPage = new NavigationPage(new FollowDetailPage());
            //MainPage = new NavigationPage(new UnitImageGallery("123", "04d1867a-1d1e-e911-a98e-000d3aa30add"));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
