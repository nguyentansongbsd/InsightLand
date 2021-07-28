﻿using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.IServices;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable
{
    public partial class Login : ContentPage
    {
        private string _userName;
        public string UserName { get => _userName ; set { _userName = value;OnPropertyChanged(nameof(UserName)); } }
        private string _password;
        public string Password { get=>_password; set { _password = value;OnPropertyChanged(nameof(Password)); } }
        private bool _eyePass =false;
        public bool EyePass { get => _eyePass; set { _eyePass = value; OnPropertyChanged(nameof(EyePass)); } }
        private bool _issShowPass = true;
        public bool IsShowPass { get => _issShowPass; set { _issShowPass = value; OnPropertyChanged(nameof(IsShowPass)); } }

        public string ImeiNum { get; set; }
        public Login()
        {
            InitializeComponent();
            this.BindingContext = this;

            if (UserLogged.IsLogged)
            {
                checkboxRememberAcc.IsChecked = true;
                UserName = UserLogged.User;
                Password = UserLogged.Password;
                SetGridUserName();
                SetGridPassword();
            }
            else
            {
                checkboxRememberAcc.IsChecked = false;
            }

        }

        protected override bool OnBackButtonPressed()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return true;
        }

        private void IsRemember_Tapped(object sender, EventArgs e)
        {
            checkboxRememberAcc.IsChecked = !checkboxRememberAcc.IsChecked;
        }

        private void UserName_Focused(object sender, EventArgs e)
        {
            entryUserName.Placeholder = "";
            SetGridUserName();
        }

        private void UserName_UnFocused(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                Grid.SetRow(lblUserName, 0);
                Grid.SetRow(entryUserName, 0);
                Grid.SetRowSpan(entryUserName, 2);
                entryUserName.Placeholder = "Tên đăng nhập";
            }
        }

        private void SetGridUserName()
        {
            Grid.SetRow(lblUserName, 0);
            Grid.SetRow(entryUserName, 1);
            Grid.SetRowSpan(entryUserName, 1);
        }

        private void Password_Focused(object sender, EventArgs e)
        {
            entryPassword.Placeholder = "";
            SetGridPassword();
            lblEyePass.Margin = new Thickness(0, 0, 0, 0);
        }

        private void Password_UnFocused(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                Grid.SetRow(lblPassword, 1);
                Grid.SetRow(entryPassword, 1);
                Grid.SetRowSpan(entryPassword, 2);
                lblEyePass.Margin = new Thickness(0, 0, 0, -10);
                EyePass = false;
                entryPassword.Placeholder = "Mật khẩu";
            }
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            if (!EyePass)
            {
                EyePass = string.IsNullOrWhiteSpace(Password) ? false : true;
            }
        }

        private void SetGridPassword()
        {
            Grid.SetRow(lblPassword, 0);
            Grid.SetRow(entryPassword, 1);
            Grid.SetRowSpan(entryPassword, 1);
        }

        private void ShowHidePass_Tapped(object sender, EventArgs e)
        {
            IsShowPass = !IsShowPass;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                await DisplayAlert("", "Tên đăng nhập không được để trống", "Đóng");
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                await DisplayAlert("", "Mật khẩu không được để trống", "Đóng");
                return;
            }
            try
            {
                LoadingHelper.Show();
                var client = BsdHttpClient.Instance();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/common/oauth2/token");//OrgConfig.LinkLogin
                var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("resource", OrgConfig.Resource),
                        //new KeyValuePair<string, string>("client_id", OrgConfig.ClientId),
                        //new KeyValuePair<string, string>("client_secret", OrgConfig.ClientSecret),
                        //new KeyValuePair<string, string>("grant_type", "client_credentials")
                        new KeyValuePair<string, string>("client_id", "2ad88395-b77d-4561-9441-d0e40824f9bc"),
                        new KeyValuePair<string, string>("username", OrgConfig.UserName),
                        new KeyValuePair<string, string>("password", OrgConfig.Password),
                        new KeyValuePair<string, string>("grant_type", "password"),
                        
                    });
                request.Content = formContent;
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                    App.Current.Properties["Token"] = tokenData.access_token;

                    EmployeeModel employeeModel = await LoginUser();
                    if (employeeModel != null)
                    {
                        if (string.IsNullOrWhiteSpace(employeeModel.bsd_imeinumber))
                        {
                            ImeiNum = await DependencyService.Get<INumImeiService>().GetImei();
                            await UpdateImei(employeeModel.bsd_employeeid);
                        }

                        if (checkboxRememberAcc.IsChecked)
                        {
                            UserLogged.User = employeeModel.bsd_name;
                            UserLogged.Password = employeeModel.bsd_password;
                            UserLogged.IsLogged = true;
                        }
                        else
                        {
                            UserLogged.User = string.Empty;
                            UserLogged.Password = string.Empty;
                            UserLogged.IsLogged = false;
                        }

                        await Shell.Current.Navigation.PopAsync(false);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        await DisplayAlert("", "Thông tin đăng nhập không đúng. Vui lòng thử lại", "Đóng");
                    }
                }
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                await DisplayAlert("Thông báo", "Lỗi kết nối đến Server. \n" + ex.Message, "Đóng");
            }
        }

        public async Task<EmployeeModel> LoginUser()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='bsd_employee'>
                    <attribute name='bsd_employeeid' />
                    <attribute name='bsd_name' />
                    <attribute name='createdon' />
                    <attribute name='bsd_password' />
                    <attribute name='bsd_imeinumber' />
                    <order attribute='bsd_name' descending='false' />
                    <filter type='and'>
                      <condition attribute='bsd_name' operator='eq' value='{UserName}' />
                      <condition attribute='bsd_password' operator='eq' value='{Password}' />
                    </filter>
                  </entity>
                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EmployeeModel>>("bsd_employees", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return null;
            }
            else
            {
                return result.value.FirstOrDefault();
            }
        }
        
        public async Task UpdateImei(string employeeId)
        {
            string path = $"/bsd_employees({employeeId})";
            var content = await getContent();
            CrmApiResponse crmApiResponse = await CrmHelper.PatchData(path, content);
            if (!crmApiResponse.IsSuccess)
            {
                LoadingHelper.Hide();
                await DisplayAlert("", "Không cập nhật được thông tin Imei", "Đóng");
                return ;
            }
        }

        private async Task<object> getContent()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_imeinumber"] = ImeiNum;
            return data;
        }
    }
}