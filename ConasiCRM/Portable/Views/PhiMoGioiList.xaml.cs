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
	public partial class PhiMoGioiList : ContentPage
	{
        private readonly PhiMoGioiListViewModel viewModel;
		public PhiMoGioiList()
		{
			InitializeComponent ();
            BindingContext = viewModel = new PhiMoGioiListViewModel();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            PhiMoGioiListModel val = e.Item as PhiMoGioiListModel;
            viewModel.IsBusy = true;          
            PhiMoGioiForm newPage = new PhiMoGioiForm(val.bsd_brokeragefeesid);
            newPage.CheckPhiMoGioi = async (CheckPhiMoGioi) =>
            {
                if (CheckPhiMoGioi == true)
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