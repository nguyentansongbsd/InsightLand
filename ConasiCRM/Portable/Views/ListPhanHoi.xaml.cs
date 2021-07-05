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
            NeedToRefresh = false;
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                viewModel.IsBusy = true;
                await viewModel.LoadOnRefreshCommandAsync();
                NeedToRefresh = false;
                viewModel.IsBusy = false;
            }
        }

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhanHoiForm());
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListPhanHoiModel val = e.Item as ListPhanHoiModel;
            viewModel.IsBusy = true;
            PhanHoiForm newPage = new PhanHoiForm(val.incidentid);
            newPage.CheckPhanHoi = async (CheckPhanHoi) =>
            {
                if (CheckPhanHoi == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                viewModel.IsBusy = false;
            };
        }

        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            viewModel.IsBusy = true;
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.IsBusy = false;
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
