using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationList : ContentPage
    {
        private readonly ReservationListViewModel viewModel;
        public ReservationList()
        {
            InitializeComponent();
            BindingContext = viewModel = new ReservationListViewModel();
            viewModel.IsBusy = true;
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            viewModel.IsBusy = false;
        }    

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ReservationListModel val = e.Item as ReservationListModel;
            viewModel.IsBusy = true;          
            ReservationForm newPage = new ReservationForm(val.quoteid);
            newPage.CheckReservation = async (CheckReservation) =>
            {
                if (CheckReservation == true)
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