using ConasiCRM.Portable;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            txtUsername.Text = OrgConfig.Username;
            txtPassword.Text = OrgConfig.Password;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            this.activityIndicator.IsRunning = true;
            this.btnLogin.IsEnabled = false;
            if (txtUsername.Text.Trim() == "")
            {
                await DisplayAlert("", "Tên đăng nhập không được để trống", "Đóng");
                return;
            }
            if (txtPassword.Text.Trim() == "")
            {
                await DisplayAlert("", "Mật khẩu không được để trống", "Đóng");
                return;
            }
            try
            {
                var client = BsdHttpClient.Instance();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/common/oauth2/token");
                var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("client_id", "2ad88395-b77d-4561-9441-d0e40824f9bc"),
                        new KeyValuePair<string, string>("username", txtUsername.Text),
                        new KeyValuePair<string, string>("password", txtPassword.Text),
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("resource", OrgConfig.Resource)
                    });
                request.Content = formContent;
                var response = await client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                App.Current.Properties["Token"] = tokenData.access_token;
                //await Navigation.PushAsync(new MainPage());
                await Navigation.PushAsync(new MasterDetailPage1());
                //await Navigation.PushAsync(new ReservationForm(Guid.Parse("d1f38d96-2355-e911-a98b-000d3aa00fc2")));
                Navigation.RemovePage(this);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Thông báo", "Lỗi kết nối đến Server. \n" + ex.Message, "Đóng");
            }
            this.btnLogin.IsEnabled = true;
            this.activityIndicator.IsRunning = false;
        }

    }
}