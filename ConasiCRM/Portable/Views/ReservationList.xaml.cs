using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Telerik.XamarinForms.DataGrid;
using Telerik.XamarinForms.DataGrid.Commands;
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
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
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
    }
}