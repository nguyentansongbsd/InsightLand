using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPhanHoi : ContentPage
    {
        public static bool? NeedToRefresh = null;
        private readonly ListPhanHoiViewModel viewModel;
        public ListPhanHoi()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListPhanHoiViewModel();
            LoadingHelper.Show();
            NeedToRefresh = false;
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadOnRefreshCommandAsync();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
        }

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new PhanHoiForm());
            LoadingHelper.Hide();
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListPhanHoiModel val = e.Item as ListPhanHoiModel;
            LoadingHelper.Show();
            PhanHoiForm newPage = new PhanHoiForm(val.incidentid);
            newPage.CheckPhanHoi = async (CheckPhanHoi) =>
            {
                if (CheckPhanHoi == true)
                {
                    await Navigation.PushAsync(newPage);                  
                }
                LoadingHelper.Hide();
            };
        }

        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                SearchBar_SearchButtonPressed(null, EventArgs.Empty);
            }
        }
    }
}
