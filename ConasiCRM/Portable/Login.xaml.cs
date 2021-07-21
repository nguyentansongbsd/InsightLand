using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

            if (UserLogged.IsLogged)
            {
                checkboxRememberAcc.IsChecked = true;
                txtUsername.Text = UserLogged.User;
                txtPassword.Text = UserLogged.Password;
            }
            else
            {
                checkboxRememberAcc.IsChecked = false;
            }

        }
        private void IsRemember_Tapped(object sender, EventArgs e)
        {
            checkboxRememberAcc.IsChecked = !checkboxRememberAcc.IsChecked;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
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
                LoadingHelper.Show();
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
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                    if (checkboxRememberAcc.IsChecked)
                    {
                        UserLogged.User = txtUsername.Text;
                        UserLogged.Password = txtPassword.Text;
                        UserLogged.IsLogged = true;
                    }
                    else
                    {
                        UserLogged.User = string.Empty;
                        UserLogged.Password = string.Empty;
                        UserLogged.IsLogged = false;
                    }
                    App.Current.Properties["Token"] = tokenData.access_token;
                    await Navigation.PopModalAsync(false);
                    //await Task.Run(() =>
                    //{
                    //    Device.BeginInvokeOnMainThread(() =>
                    //    {
                    //        Xamarin.Forms.Application.Current.MainPage = new AppShell();
                    //    });
                    //});
                    LoadingHelper.Hide();

                }
                else
                {
                    LoadingHelper.Hide();
                    await DisplayAlert("", "Thông tin đăng nhập không đúng. Vui lòng thử lại", "Đóng");
                }
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                await DisplayAlert("Thông báo", "Lỗi kết nối đến Server. \n" + ex.Message, "Đóng");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return true;
        }

        private async void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            await Task.Run(() => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Xamarin.Forms.Application.Current.MainPage = new AppShell();
                });
            });
            LoadingHelper.Hide();
        }
    }
}