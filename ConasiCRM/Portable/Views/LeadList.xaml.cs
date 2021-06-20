using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public partial class LeadList : ContentPage
    {
        private readonly LeadListViewModel viewModel;
        public LeadList()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LeadListViewModel();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }       

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            await Task.Delay(1000);
            await Navigation.PushAsync(new LeadForm());
            viewModel.IsBusy = false;
        }

        protected override void OnAppearing()
        {
            if (App.Current.Properties.ContainsKey("update") && App.Current.Properties["update"] as string == "1")
            {
                App.Current.Properties["update"] = "0";
                viewModel.RefreshCommand.Execute(null);
            }
        }

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.IsBusy = true;
            await Task.Delay(1000);
            var item = e.Item as LeadListModel;
            LeadForm newPage = new LeadForm(item.leadid);
            newPage.CheckSingleLead = async (checkSingleLead) =>
            {
                if (checkSingleLead == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                viewModel.IsBusy = false;
            };                        
        }      
    }
}