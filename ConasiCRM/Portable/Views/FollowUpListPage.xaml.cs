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

        private async void listView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = e.Item as FollowUpListPageModel;
            
            //await Navigation.PushAsync(new );
        }
    }
}
