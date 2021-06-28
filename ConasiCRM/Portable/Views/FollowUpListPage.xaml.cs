using System;
using System.Collections.Generic;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class FollowUpListPage : ContentPage
    {
        public FollowUpListPageViewModel viewModel;
        public FollowUpListPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new FollowUpListPageViewModel();
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadData();
        }

        private void listView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            viewModel.IsBusy = true;
            var item = e.Item as FollowUpListPageModel;
            FollowDetailPage followDetailPage = new FollowDetailPage(item.bsd_followuplistid);
            followDetailPage.OnLoaded = async(isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(followDetailPage);
                    viewModel.IsBusy = false;
                }
                else
                {
                    await DisplayAlert("", "Không tìm thấy dữ liệu", "Đóng");
                    viewModel.IsBusy = false;
                }
            };
            
        }

        private async void Search_Pressed(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.IsBusy = false;
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                Search_Pressed(null, EventArgs.Empty);
            }
        }
    }
}
